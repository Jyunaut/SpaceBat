using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Controller : Actor
    {
        public State State { get; private set; }

        public void SetState(State state)
        {
            State?.ExitState();
            State = state;
            State.EnterState();
        }
    }
}