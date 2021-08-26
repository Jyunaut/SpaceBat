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

        public List<Phase> phases;

        public Phase currentPhase;
        public Move currentMove;
        public int movesTraversed;
        public int phasesTraversed;
        public State state { get; set; }

        protected override void Awake()
        {
            base.Awake();
            state = new State(this);
        }

        private void Start()
        {
            OnPhaseComplete += (object sender, OnPhaseCompleteEventArgs eventArgs) =>
            {
               if(eventArgs.nextPhase != null) TriggerPhase(eventArgs.nextPhase);
            };
            OnPhaseComplete(this, new OnPhaseCompleteEventArgs { nextPhase = currentPhase = phases[phasesTraversed = 0] });

            OnMoveComplete += (object sender, OnMoveCompleteEventArgs eventArgs) =>
            {
               if(eventArgs.nextMove != null) TriggerMove(eventArgs.nextMove);
            };
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = currentMove = currentPhase.moves[movesTraversed = 0] });
        }
        
        private void Update()
        {
            state?.Update();
            state?.Transitions();
        }

        private void FixedUpdate()
        {
            state?.FixedUpdate();
        }

        public void TriggeredOnMoveComplete()
        {
            // if(!currentMove.looping) currentPhase.moves.RemoveAt(movesTraversed);
            if(++movesTraversed < currentPhase.moves.Count)
            {
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = currentMove = currentPhase.moves[movesTraversed]});
            }
            else
            {
                TriggeredOnPhaseComplete();
            }
        }

        public void TriggeredOnPhaseComplete()
        {
            if(++phasesTraversed < phases.Count)
            {
                movesTraversed = 0;
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = currentPhase = phases[phasesTraversed]});
            }
            else
            {
                OnPhaseComplete?.Invoke(this, new OnPhaseCompleteEventArgs{nextPhase = currentPhase = phases[phasesTraversed = 0]});
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = currentMove = currentPhase.moves[movesTraversed = 0]});
            }
        }

        public void SetState(State state)
        {
            this.state?.ExitState();
            this.state = state;
            this.state.EnterState();
        }

        protected void TriggerPhase(Phase phase)
        {
            currentPhase = phase;
        }

        protected void TriggerMove(Move move)
        {
            try
            {
                typeof(State).InvokeMember(move.name, BindingFlags.InvokeMethod, null, state, null);
            }
            catch (MissingMethodException e)
            {
                Debug.Log($"Missing a state Method for: {e.Message}");
            }
        }
    }
}