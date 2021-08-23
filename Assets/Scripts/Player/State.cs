using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace Player
{
    public class State : StateMachine
    {
        public interface IDoesDamage
        {
            int Damage { get; set; }
            Vector2 HitBoxOrigin { get; set; }
            Vector2 HitBoxSize { get; set; }
            void CastHitCollider();
        }

        public bool IsDoingAction { get => actionCoroutine != null; }
        public bool CanMove { get; set; } = true;
        public bool CanAttack { get; set; } = true;

        protected static Collider2D[] hits = new Collider2D[20];
        protected static Coroutine actionCoroutine;
        protected static Coroutine attackCoroutine;

        protected Controller Controller { get; set; }
        protected string AnimationName { get; set; }

        public State(Controller controller, string animationName = "")
        {
            Controller = controller;
            AnimationName = animationName;
        }
    }
    abstract class Action : State
    {
        public Action(Controller controller, string animationName = "") : base(controller, animationName) {}

        public override void EnterState()
        {
            Controller.Animator.Play(AnimationName, 0, 0f);
            StartAction(Controller.Animations[AnimationName].length);
        }

        public override void Transitions()
        {
            if (IsDoingAction)
                return;
            else if (Mathf.Abs(Inputs.Horizontal) <= 0.1f && Mathf.Abs(Inputs.Vertical) <= 0.1f)
                Controller.SetState(new Idle(Controller));
            else if (Mathf.Abs(Inputs.Horizontal) > 0.1f || Mathf.Abs(Inputs.Vertical) > 0.1f)
                Controller.SetState(new Moving(Controller));
        }

        private void StartAction(float duration)
        {
            if (actionCoroutine != null)
            {
                Controller.StopCoroutine(actionCoroutine);
                actionCoroutine = null;
            }
            actionCoroutine = Controller.StartCoroutine(DoAction(duration));

            IEnumerator DoAction(float duration)
            {
                float time = 0;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
                actionCoroutine = null;
            }
        }
    }
    abstract class Attack : Action, State.IDoesDamage
    {
        public delegate void HitTarget(IDamageable target);
        public event HitTarget OnHitTarget;

        public int HitFrame { get; set; }
        public int Damage { get; set; }
        public Vector2 HitBoxOrigin { get; set; }
        public Vector2 HitBoxSize { get; set; }

        public Attack(Controller controller, int hitFrame, int damage, Vector2 hitBoxOrigin, Vector2 hitBoxSize, string animationName = "") : base(controller, animationName)
        {
            HitFrame = hitFrame;
            Damage = damage;
            HitBoxOrigin = (Vector2)Controller.transform.position + hitBoxOrigin;
            HitBoxSize = hitBoxSize;
        }

        public override void EnterState()
        {
            base.EnterState();
            StartAttack();
        }

        private void StartAttack()
        {
            if (attackCoroutine != null)
            {
                Controller.StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            attackCoroutine = Controller.StartCoroutine(DoAttackAfterDelay(HitFrame.ToSeconds(Controller.Animations[AnimationName].frameRate)));

            IEnumerator DoAttackAfterDelay(float delayTime)
            {
                yield return new WaitForSeconds(delayTime);
                CastHitCollider();
            }
        }

        public void CastHitCollider()
        {
            int hitCount = Physics2D.OverlapBoxNonAlloc(HitBoxOrigin, HitBoxSize, 0f, hits, Controller.HittableLayers);
            Vector2[] points =
            {
                new Vector2(HitBoxOrigin.x - HitBoxSize.x / 2f, HitBoxOrigin.y + HitBoxSize.y / 2f),
                new Vector2(HitBoxOrigin.x + HitBoxSize.x / 2f, HitBoxOrigin.y + HitBoxSize.y / 2f),
                new Vector2(HitBoxOrigin.x - HitBoxSize.x / 2f, HitBoxOrigin.y - HitBoxSize.y / 2f),
                new Vector2(HitBoxOrigin.x + HitBoxSize.x / 2f, HitBoxOrigin.y - HitBoxSize.y / 2f)
            };
            Debug.DrawLine(points[0], points[1], Color.red, 1f); // top
            Debug.DrawLine(points[2], points[3], Color.red, 1f); // bottom
            Debug.DrawLine(points[0], points[2], Color.red, 1f); // left
            Debug.DrawLine(points[1], points[3], Color.red, 1f); // right
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].TryGetComponent(out IDamageable target))
                {
                    OnHitTarget?.Invoke(target);
                }
            }
        }
    }
    abstract class Charge : State
    {
        protected bool isCharged => _timer >= _chargeTime;
        
        private float _chargeTime;
        private float _timer;
        private Coroutine _chargeCoroutine;

        public Charge(Controller controller, string animationName = "",  float chargeTime = 0f) : base(controller, animationName)
        {
            _chargeTime = chargeTime;
        }

        public override void EnterState()
        {
            Controller.Animator.Play(AnimationName, 0, 0f);
            StartCharge();
        }

        private void StartCharge()
        {
            if (_chargeCoroutine != null)
            {
                Controller.StopCoroutine(_chargeCoroutine);
                _chargeCoroutine = null;
            }
            _chargeCoroutine = Controller.StartCoroutine(DoCharge());

            IEnumerator DoCharge()
            {
                while (_timer < _chargeTime)
                {
                    _timer += Time.deltaTime;
                    yield return null;
                }
                _chargeCoroutine = null;
            }
        }
    }
    class Idle : State
    {
        public Idle(Controller controller) : base(controller, "player_idle") {}

        public override void EnterState()
        {
            Controller.Animator.Play(AnimationName, 0, 0f);
        }

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge1(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing1(Controller));
        }
    }
    class Moving : State
    {
        public Moving(Controller controller) : base(controller, "player_idle") {}

        public override void EnterState()
        {
            Controller.Animator.Play(AnimationName, 0, 0f);
        }

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge1(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing1(Controller));
        }
    }
    class LightSwingCharge1 : Charge
    {
        public LightSwingCharge1(Controller controller) : base(controller, "player_lightswingcharge1", 0.25f) {}

        public override void Transitions()
        {
            if (Inputs.Attack && isCharged)
                Controller.SetState(new HeavySwing1(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing1(Controller));
        }
    }
    class LightSwing1 : Attack
    {
        public LightSwing1(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 1,
            hitBoxOrigin: new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_lightswing1") {}

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge2(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing2(Controller));
            base.Transitions();
        }
    }
    class HeavySwing1 : Attack
    {
        public HeavySwing1(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 6,
            hitBoxOrigin: new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_heavyswing1") {}

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge2(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing2(Controller));
            base.Transitions();
        }
    }
    class LightSwingCharge2 : Charge
    {
        public LightSwingCharge2(Controller controller) : base(controller, "player_lightswingcharge2", 0.25f) {}

        public override void Transitions()
        {
            if (Inputs.Attack && isCharged)
                Controller.SetState(new HeavySwing2(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing2(Controller));
        }
    }
    class LightSwing2 : Attack
    {
        public LightSwing2(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 2,
            hitBoxOrigin: new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_lightswing2") {}

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge3(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing3(Controller));
            base.Transitions();
        }
    }
    class HeavySwing2 : Attack
    {
        public HeavySwing2(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 6,
            new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_heavyswing2") {}

        public override void Transitions()
        {
            if (Inputs.AttackHold)
                Controller.SetState(new LightSwingCharge3(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing3(Controller));
            base.Transitions();
        }
    }
    class LightSwingCharge3 : Charge
    {
        public LightSwingCharge3(Controller controller) : base(controller, "player_lightswingcharge3", 0.25f) {}

        public override void Transitions()
        {
            if (Inputs.Attack && isCharged)
                Controller.SetState(new HeavySwing3(Controller));
            else if (Inputs.Attack)
                Controller.SetState(new LightSwing3(Controller));
        }
    }
    class LightSwing3 : Attack
    {
        public LightSwing3(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 3,
            hitBoxOrigin: new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_lightswing3") {}
    }
    class HeavySwing3 : Attack
    {
        public HeavySwing3(Controller controller) : base(
            controller,
            hitFrame: 1,
            damage: 6,
            hitBoxOrigin: new Vector2(0f, 0.2f),
            hitBoxSize: new Vector2(1f, 1f),
            animationName: "player_heavyswing3") {}
    }
}