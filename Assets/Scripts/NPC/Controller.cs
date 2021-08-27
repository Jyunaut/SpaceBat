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

        [field: SerializeField] public List<Phase> Phases{ get; private set; }
        [field: SerializeField] public List<PhaseHandler> CurrentPhase { get; private set; }
        [field: SerializeField] public Move CurrentMove { get; private set; }
        [field: SerializeField] public int MovesTraversed { get; private set; }
        [field: SerializeField] public int PhasesTraversed { get; private set; }
        [field: SerializeField] public State State { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            State = new State(this);
            CurrentPhase = new List<PhaseHandler> { new PhaseHandler(Phases[PhasesTraversed = 0]) };
        }

        private void Start()
        {
            OnPhaseComplete += (object sender, OnPhaseCompleteEventArgs eventArgs) =>
            {
                // TODO: Might have to remove this.Since I am not using it
                // if(eventArgs.nextPhase != null) TriggerPhase(eventArgs.nextPhase);
            };
            OnMoveComplete += (object sender, OnMoveCompleteEventArgs eventArgs) =>
            {
               if(eventArgs.nextMove != null) TriggerMove(eventArgs.nextMove);
            };
            OnPhaseComplete(this, new OnPhaseCompleteEventArgs { nextPhase = CurrentPhase[PhasesTraversed].Phase });
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = CurrentPhase[PhasesTraversed].Phase.moves[MovesTraversed = 0] });
        }
        
        private void Update()
        {
            Debug.Log(CurrentPhase[PhasesTraversed].Phase.ToString());
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
            UpdateMoveTraversal();
            if(MovesTraversed < CurrentPhase[PhasesTraversed].Phase.moves.Count)
            {
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

        private void UpdateMoveTraversal()
        {
            if(!CurrentMove.looping)
            {
                CurrentPhase[PhasesTraversed].Phase.moves.RemoveAt(MovesTraversed); // TODO: Need to find a way to prevent 1 time use moves from getting deleted in the phase prefab
            }
            else
            {
                MovesTraversed++;
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
    }
}