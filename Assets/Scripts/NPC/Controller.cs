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

        public Phase phase;
        public Move currentMove;
        public int movesTraversed;
        public State state { get; set; }

        protected override void Awake()
        {
            base.Awake();
            state = new State(this);
        }

        private void Start()
        {
            OnMoveComplete += (object sender, OnMoveCompleteEventArgs eventArgs) =>
            {
               if(eventArgs.nextMove != null) TriggerMove(eventArgs.nextMove);
            };
            OnMoveComplete(this, new OnMoveCompleteEventArgs { nextMove = currentMove = phase.moves[movesTraversed = 0] });
        }
        
        private void Update()
        {
            state?.Update();
        }

        private void FixedUpdate()
        {
            state?.FixedUpdate();
        }

        public void TriggeredOnMoveComplete()
        {
            if(++movesTraversed != phase.moves.Length)
            {
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = currentMove = phase.moves[movesTraversed]});
            }
            else
            {
                Debug.Log("Looping phase");
                OnMoveComplete?.Invoke(this, new OnMoveCompleteEventArgs{nextMove = currentMove = phase.moves[movesTraversed = 0]});
            }
        }

        public void SetState(State state)
        {
            this.state?.ExitState();
            this.state = state;
            this.state.EnterState();
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