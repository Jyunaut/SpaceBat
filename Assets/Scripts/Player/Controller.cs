using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Controller : Actor
    {
        [field: SerializeField] public LayerMask HittableLayers { get; private set; }
        [SerializeField] private float _moveSpeed;
        private float _speedMultiplier = 1f;
        public float MoveSpeed => _speedMultiplier * _moveSpeed;
        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier *= multiplier;
            _speedMultiplier = Mathf.Clamp(_speedMultiplier, 0, 2f);
        }
        public void ResetSpeedMultiplier() => _speedMultiplier = 1f;

        public State State { get; private set; }
        [SerializeField] private List<AttackData> _attackList;
        public Dictionary<string, AttackData> AttackDictionary { get; private set; } = new Dictionary<string, AttackData>();

        protected override void Awake()
        {
            base.Awake();
            foreach (AttackData data in _attackList)
                AttackDictionary.Add(data.name, data);
        }

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
            CheckInBounds();
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
            Rigidbody2D.velocity = MoveSpeed * new Vector2(Inputs.Horizontal, Inputs.Vertical).normalized;
        }

        private void CheckInBounds()
        {
            Vector2 cameraOrigin = Camera.main.transform.position;
            Vector2 cameraExtents = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height,
                                                Camera.main.orthographicSize * Screen.width / Screen.height);
            // Vector2 cameraExtents = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height,
            //                                     Camera.main.orthographicSize * Screen.height / Screen.width);
            if (transform.position.x > cameraOrigin.x + cameraExtents.x)
                transform.position = new Vector2(cameraOrigin.x + cameraExtents.x, transform.position.y);
            if (transform.position.x < cameraOrigin.x - cameraExtents.x)
                transform.position = new Vector2(cameraOrigin.x - cameraExtents.x, transform.position.y);
            if (transform.position.y > cameraOrigin.y + cameraExtents.y)
                transform.position = new Vector2(transform.position.x, cameraOrigin.y + cameraExtents.y);
            if (transform.position.y < cameraOrigin.y - cameraExtents.y)
                transform.position = new Vector2(transform.position.x, cameraOrigin.y - cameraExtents.y);
        }
    }
}