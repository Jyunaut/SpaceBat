using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Actor : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public BoxCollider2D BoxCollider2D { get; private set; }
    public Dictionary<string, AnimationClip> Animations { get; set; } = new Dictionary<string, AnimationClip>();

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        if (Animator.runtimeAnimatorController.animationClips.Length > 0)
            foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                Animations.Add(clip.name, clip);
    }
}