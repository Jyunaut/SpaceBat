using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MoveLibrary;

namespace NPC
{
    public class Controller : Actor
    {
        public event EventHandler<OnMoveCompleteEventArgs> OnMoveComplete;
        public class OnMoveCompleteEventArgs : EventArgs { public Move nextMove { get; set; } }

        public event EventHandler<OnPhaseCompleteEventArgs> OnPhaseComplete;
        public class OnPhaseCompleteEventArgs : EventArgs { public Phase nextPhase { get; set; } }

        public class PhaseHandler
        {
            public Phase Phase { get; set; }
            public bool isPlayed { get; set; }
        }

        [field: SerializeField] public List<Phase> Phases{ get; private set; }
        [field: SerializeField] public PhaseHandler CurrentPhase { get; private set; }
        [field: SerializeField] public Move CurrentMove { get; private set; }
        [field: SerializeField] public int MovesTraversed { get; private set; }
        [field: SerializeField] public int PhasesTraversed { get; private set; }
        [field: SerializeField] public State State { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            State = new State(this);
            CurrentPhase = new PhaseHandler();
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
            OnPhaseComplete(this, new OnPhaseCompleteEventArgs { nextPhase = CurrentPhase.Phase = Phases[PhasesTraversed = 0] });
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed = 0] });
        }
        
        private void Update()
        {
            State?.Update();
            State?.Transitions();
        }

        private void FixedUpdate()
        {
            State?.FixedUpdate();
        }

        public void TriggeredOnMoveComplete()
        {
            UpdateMoveTraversal();
            if(MovesTraversed < CurrentPhase.Phase.moves.Count)
            {
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed]});
            }
            else
            {
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed = 0]});
            }
        }

        public void TriggeredOnPhaseComplete()
        {
            CurrentPhase.isPlayed = true;
            if(++PhasesTraversed < Phases.Count)
            {
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = CurrentPhase.Phase = Phases[PhasesTraversed]});
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed = 0]});
            }
            else
            {
                Debug.Log("There are no additional phases in the list");
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = CurrentPhase.Phase = Phases[PhasesTraversed = 0]});
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = CurrentMove = CurrentPhase.Phase.moves[MovesTraversed = 0]});
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
                typeof(State).InvokeMember(move.name, BindingFlags.InvokeMethod, null, State, null);
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
                CurrentPhase.Phase.moves.RemoveAt(MovesTraversed); // TODO: Need to find a way to prevent 1 time use moves from getting deleted in the phase prefab
            }
            else
            {
                MovesTraversed++;
            }
        }

        public void CheckCondition()
        {
            if (Health < (int)(MaxHealth * 0.5f) && !CurrentPhase.isPlayed)
            {
                TriggeredOnPhaseComplete();
            }
        }
    }
}