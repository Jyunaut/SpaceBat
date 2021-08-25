using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace NPC
{
    public class State : StateMachine
    {
        public Controller Controller { get; set; }

        public State(Controller controller)
        {
            Controller = controller;
        }

        public void Wait() => Controller.SetState(new Wait(Controller));
        public void Shoot() => Controller.SetState(new Shoot(Controller));

    }
    class Wait : State
    {
        public Wait(Controller controller) : base(controller) { }

        public override void EnterState()
        {
            Debug.Log("Wait");
        }
    }
    class Shoot : State
    {
        public Shoot(Controller controller) : base(controller) { }

        public override void EnterState()
        {
            Debug.Log("Shoot");
        }
    }
}
