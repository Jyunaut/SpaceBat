using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace NPC
{
    public class Controller : Actor
    {
        public State state { get; set; }
        [SerializeField] public List<Move> moves = new List<Move>();

        protected override void Awake()
        {
            // base.Awake();
            state = new State(this);
        }

        public void Start()
        {
            // Debug.Log(moves[0].name);
            // Debug.Log(moves[1].name);
            RunState(moves[0]);
            RunState(moves[1]);
        }

        public void SetState(State state)
        {
            this.state?.ExitState();
            this.state = state;
            this.state.EnterState();
        }

        public void RunState(Move move)
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