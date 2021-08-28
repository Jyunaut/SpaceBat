using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MoveLibrary;

namespace NPC
{
    [RequireComponent(typeof(PathfindingHandler))]
    public class Controller : Actor
    {
        [SerializeField] public event EventHandler<OnMoveCompleteEventArgs> OnMoveComplete;
        public class OnMoveCompleteEventArgs : EventArgs { public Move nextMove { get; set; } }

        [SerializeField] public event EventHandler<OnPhaseCompleteEventArgs> OnPhaseComplete;
        public class OnPhaseCompleteEventArgs : EventArgs { public Phase nextPhase { get; set; } }

        public class PhaseHandler
        {
            public Phase Phase { get; set; }
            public bool IsPlayed { get; set; }

            public PhaseHandler(Phase phase, bool isPlayed = false)
            {
                Phase = phase;
                IsPlayed = isPlayed;
            }
        }

        public class MoveHandler
        {
            public Move Move { get; set; }
            public bool IsPlayed { get; set; }

            public MoveHandler(Move move, bool isPlayed = false)
            {
                Move = move;
                IsPlayed = isPlayed;
            }
        }

        [field: SerializeField] public List<Phase> Phases{ get; private set; }
        [field: SerializeField] public List<PhaseHandler> ScannedPhases { get; private set; }
        [field: SerializeField] public List<MoveHandler> ScannedMoves { get; private set; }
        
        [field: SerializeField] public Move CurrentMove { get; private set; }
        [field: SerializeField] public int MovesTraversed { get; private set; }
        [field: SerializeField] public int PhasesTraversed { get; private set; }
        [field: SerializeField] public State State { get; private set; }
        [field: SerializeField] private GameObject DeathEffect { get; }
        [field: SerializeField] private AnimationClip DeathAnimation { get; }

        public Vector2 SpawnPosition { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            State = new State(this);
            PhasesTraversed = MovesTraversed = 0;
            ScannedPhases = new List<PhaseHandler> { new PhaseHandler(Phases[PhasesTraversed]) };
            ScannedMoves = new List<MoveHandler> { new MoveHandler(ScannedPhases[PhasesTraversed].Phase.moves[MovesTraversed]) };
        }

        private void Start()
        {
            SpawnPosition = transform.position;
            OnPhaseComplete += (object sender, OnPhaseCompleteEventArgs eventArgs) =>
            {
                //TODO: Uncomment this IF looping from phase 1 to 3 is possible
                // if(ScannedPhases[PhasesTraversed].IsPlayed && !ScannedPhases[PhasesTraversed].Phase.looping)
                // {
                //     TriggeredOnPhaseComplete();
                // }
            };
            OnMoveComplete += (object sender, OnMoveCompleteEventArgs eventArgs) =>
            {
                if(eventArgs.nextMove != null)
                {
                    if(ScannedMoves[MovesTraversed].IsPlayed && !ScannedMoves[MovesTraversed].Move.looping)
                    {
                        TriggeredOnMoveComplete();
                    }
                    else
                    {
                        TriggerMove(eventArgs.nextMove);
                    }
                }
            };
            OnPhaseComplete(this, new OnPhaseCompleteEventArgs { nextPhase = ScannedPhases[PhasesTraversed].Phase });
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = ScannedPhases[PhasesTraversed].Phase.moves[MovesTraversed] });
        }
        
        private void Update()
        {
            CheckCondition();
            State?.Update();
            State?.Transitions();
        }

        private void FixedUpdate()
        {
            State?.FixedUpdate();
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            this.SetState(new Hurt(this));
            TriggeredOnMoveComplete();
        }

        public void TriggeredOnMoveComplete()
        {
            ScannedMoves[MovesTraversed].IsPlayed = true;

            //Check if next move is available for transition
            if(++MovesTraversed < ScannedPhases[PhasesTraversed].Phase.moves.Count)
            {
                if(ScannedPhases[PhasesTraversed].Phase.moves.Count >= ScannedMoves.Count)
                {
                    ScannedMoves.Add(new MoveHandler(ScannedPhases[PhasesTraversed].Phase.moves[MovesTraversed]) );
                }
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = ScannedPhases[PhasesTraversed].Phase.moves[MovesTraversed] });
            }
            //Complete the current phase
            else
            {
                TriggeredOnPhaseComplete();
            }
        }

        public void TriggeredOnPhaseComplete()
        {
            ScannedPhases[PhasesTraversed].IsPlayed = true;

            //Check if next phase is available for transition
            if(++PhasesTraversed < Phases.Count)
            {
                ScannedPhases.Add(new PhaseHandler(Phases[PhasesTraversed]));
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = ScannedPhases[PhasesTraversed].Phase});
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = ScannedPhases[PhasesTraversed].Phase.moves[MovesTraversed = 0]});
            }
            //Loop the current set of moves
            else
            {
                if(!CurrentMove.looping && ScannedMoves.Count == 1)
                {
                    Debug.Log("NPC on idle");
                }
                else
                {
                    OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = ScannedPhases[PhasesTraversed = 0].Phase.moves[MovesTraversed = 0] });
                }
            }
        }

        public void SetState(State state)
        {
            this.State?.ExitState();
            this.State = state;
            this.State.EnterState();
        }

        protected void TriggerMove(Move move)
        {
            try
            {
                typeof(State).InvokeMember(move.GetType().Name, BindingFlags.InvokeMethod, null, State, null);
            }
            catch (MissingMethodException e)
            {
                Debug.Log($"Missing a state Method for: {e.Message}");
            }
        }

        public void CheckCondition()
        {
            if(Phases.Count > 0)
            {
                if (Health < (int)(MaxHealth * 0.5f) && !ScannedPhases[0].IsPlayed)
                {
                    TriggeredOnPhaseComplete();
                }
                else if (Health < (int)(MaxHealth * 0.25f) && !ScannedPhases[1].IsPlayed)
                {
                    TriggeredOnPhaseComplete();
                }
            }
        }

        protected override void Die()
        {
            if (DeathAnimation)
            {
                StartCoroutine(DeathClip());
            }
            else
            {
                if (DeathEffect) Instantiate(DeathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            IEnumerator DeathClip()
            {
                Animator.Play("Death");
                yield return new WaitForSeconds(DeathAnimation.length);
                if (DeathEffect) Instantiate(DeathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}