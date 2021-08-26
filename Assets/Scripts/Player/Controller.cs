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

        private void OnEnable()
        {
            GlobalEvents.OnPlayerSpawn += EnableControls;
            GlobalEvents.OnPlayerDead += DisableControls;
        }

        private void OnDisable()
        {
            GlobalEvents.OnPlayerSpawn -= EnableControls;
            GlobalEvents.OnPlayerDead -= DisableControls;
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
        }

        public void SetState(State state)
        {
            State?.ExitState();
            State = state;
            State.EnterState();
        }

        private void MovementUpdate()
        {
            Rigidbody2D.velocity = MoveSpeed * new Vector2(Inputs.Horizontal, Inputs.Vertical).normalized;
        }

        private void EnableControls()
        {
            State.CanMove = true;
            State.CanAttack = true;
        }
        private void DisableControls()
        {
            State.CanMove = false;
            State.CanAttack = false;
        }
    }
}