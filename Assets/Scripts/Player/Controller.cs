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

        [SerializeField] private List<AttackData> _attackList;
        public State State { get; private set; }
        public CircleCollider2D CircleCollider2d { get; private set; }
        public Dictionary<string, AttackData> AttackDictionary { get; private set; } = new Dictionary<string, AttackData>();

        protected override void Awake()
        {
            base.Awake();
            CircleCollider2d = GetComponent<CircleCollider2D>();
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

        private void LateUpdate()
        {
            _topCollision = false;
            _botCollision = false;
        }

        private ContactPoint2D[] _obstacleContacts = new ContactPoint2D[1];
        private ContactPoint2D[] _cameraContacts = new ContactPoint2D[1];
        private bool _topCollision, _botCollision;
        private void OnCollisionStay2D(Collision2D col)
        {
            const float tolerance = 0.9f; // 10% tolerance on the height and bottom of the player circle collider
            if (LayerMask.GetMask("Obstacle") == (LayerMask.GetMask("Obstacle") | (1 << col.gameObject.layer)))
            {
                col.GetContacts(_obstacleContacts);
                bool pass = false;
                for (int i = 0; i < _obstacleContacts.Length; i++)
                {
                    if (_obstacleContacts[i].point.y >= transform.position.y + CircleCollider2d.bounds.extents.y * tolerance
                        && Mathf.Abs(GameManager.Instance.ScrollSpeed) >= 0.01f)
                    {
                        _topCollision = true;
                        pass = true;
                        break;
                    }
                }
                if (!pass) _topCollision = false;
            }
            if (LayerMask.GetMask("Camera") == (LayerMask.GetMask("Camera") | (1 << col.gameObject.layer)))
            {
                col.GetContacts(_cameraContacts);
                bool pass = false;
                for (int i = 0; i < _cameraContacts.Length; i++)
                {
                    if (_cameraContacts[i].point.y <= transform.position.y - CircleCollider2d.bounds.extents.y * tolerance)
                    {
                        _botCollision = true;
                        pass = true;
                        break;
                    }
                }
                if (!pass) _botCollision = false;
            }
            // Check if player is crushed
            if (_topCollision && _botCollision)
                GlobalEvents.PlayerDies();
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