using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Controller : Actor
    {
        [field: SerializeField] public LayerMask HittableLayers { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }

        public State State { get; private set; }

        private void Start()
        {
            SetState(new Idle(this));
        }

        private void Update()
        {
            State.Update();
            State.Transitions();
        }

        private void FixedUpdate()
        {
            State.FixedUpdate();
            MovementUpdate();
        }

        public void SetState(State state)
        {
            State?.ExitState();
            State = state;
            State.EnterState();
        }

        private void MovementUpdate()
        {
            if (!State.CanMove) return;
            Rigidbody2D.velocity = MovementSpeed * new Vector2(Inputs.Horizontal, Inputs.Vertical).normalized;
        }
    }
}