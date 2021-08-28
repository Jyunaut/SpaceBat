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
        public event EventHandler<OnMoveCompleteEventArgs> OnMoveComplete;
        public class OnMoveCompleteEventArgs : EventArgs { public Move nextMove { get; set; } }

        public event EventHandler<OnPhaseCompleteEventArgs> OnPhaseComplete;
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
            public int Position { get; set; }

            public MoveHandler(Move move, int position, bool isPlayed = true)
            {
                Move = move;
                IsPlayed = isPlayed;
                Position = position;
            }
        }

        [field: SerializeField] public List<Phase> Phases{ get; private set; }
        [field: SerializeField] public List<PhaseHandler> CurrentPhase { get; private set; }
        [field: SerializeField] public List<MoveHandler> LoopMoveOnce { get; private set; }
        
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
            CurrentPhase = new List<PhaseHandler> { new PhaseHandler(Phases[PhasesTraversed = 0]) };
            LoopMoveOnce = new List<MoveHandler> { new MoveHandler(Phases[PhasesTraversed = 0].moves[MovesTraversed = 0], MovesTraversed, false) };
        }

        private void Start()
        {
            SpawnPosition = transform.position;
            OnPhaseComplete += (object sender, OnPhaseCompleteEventArgs eventArgs) =>
            {
                // TODO: Might have to remove this.Since I am not using it
                // if(eventArgs.nextPhase != null) TriggerPhase(eventArgs.nextPhase);
            };
            OnMoveComplete += (object sender, OnMoveCompleteEventArgs eventArgs) =>
            {
                if(eventArgs.nextMove != null)
                {
                    TriggerMove(eventArgs.nextMove);
                }
            };
            OnPhaseComplete(this, new OnPhaseCompleteEventArgs { nextPhase = CurrentPhase[PhasesTraversed].Phase });
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed = 0] });
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
            if(++MovesTraversed < CurrentPhase[PhasesTraversed].Phase.moves.Count)
            {
                if(CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed].looping)
                    OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed]});
            }
            else
            {
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed = 0]});
            }
        }

        public void TriggeredOnPhaseComplete()
        {
            MovesTraversed = 0;
            CurrentPhase[PhasesTraversed].IsPlayed = true;
            if(++PhasesTraversed < Phases.Count)
            {
                CurrentPhase.Add(new PhaseHandler(Phases[PhasesTraversed]));
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = CurrentPhase[PhasesTraversed].Phase});
                // OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed = 0]});
            }
            else
            {
                Debug.Log("There are no additional phases in the list");
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = CurrentPhase[PhasesTraversed].Phase = Phases[PhasesTraversed = 0]});
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed = 0]});
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
                if (Health < (int)(MaxHealth * 0.5f) && !CurrentPhase[0].IsPlayed)
                {
                    TriggeredOnPhaseComplete();
                }
                else if (Health < (int)(MaxHealth * 0.25f) && !CurrentPhase[1].IsPlayed)
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