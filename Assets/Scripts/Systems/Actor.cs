using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Actor : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Dictionary<string, AnimationClip> Animations { get; private set; } = new Dictionary<string, AnimationClip>();

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

    }
}