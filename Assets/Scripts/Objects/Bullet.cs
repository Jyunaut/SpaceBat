using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _canHit;
    [field: SerializeField] public int Damage { get; private set; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_canHit == (_canHit | (1 << col.gameObject.layer)))
        {
            if (col.TryGetComponent(out Actor target))
            {
                target.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}