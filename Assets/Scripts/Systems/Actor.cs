using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Actor : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { if (value < 0) _maxHealth = 1; }
    }
    [SerializeField] private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value > _maxHealth ? _maxHealth : value;
            if (_health <= 0) Die();
        }
    }
    public bool IsAlive => Health > 0;
    public bool IsStaggered { get; private set; }

    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Dictionary<string, AnimationClip> Animations { get; private set; } = new Dictionary<string, AnimationClip>();

    private void OnValidate()
    {
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Animator.runtimeAnimatorController != null && Animator.runtimeAnimatorController.animationClips.Length > 0)
            foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                Animations.Add(clip.name, clip);
    }

    public virtual void TakeDamage(int amount)
    {
        Health -= Mathf.Clamp(amount, 0, MaxHealth);
    }

    private Coroutine _knockbackCoroutine;
    public virtual void SetStagger(float duration)
    {
        if (_knockbackCoroutine != null)
        {
            StopCoroutine(_knockbackCoroutine);
            _knockbackCoroutine = null;
        }
        _knockbackCoroutine = StartCoroutine(Delay(duration));

        IEnumerator Delay(float duration)
        {
            IsStaggered = true;
            yield return new WaitForSeconds(duration);
            IsStaggered = false;
        }
    }

    protected abstract void Die();
}