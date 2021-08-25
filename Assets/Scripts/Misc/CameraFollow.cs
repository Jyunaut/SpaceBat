using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _smoothing;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector2 _deadzone;

    private void FixedUpdate()
    {
        if (_followTarget != null)
        {
            if (_followTarget.position.x > transform.position.x + _deadzone.x)
                transform.position = Vector3.Lerp((Vector2)transform.position, new Vector3(_followTarget.position.x - _deadzone.x, transform.position.y) + _offset, _smoothing);
            if (_followTarget.position.x < transform.position.x - _deadzone.x)
                transform.position = Vector3.Lerp((Vector2)transform.position, new Vector3(_followTarget.position.x + _deadzone.x, transform.position.y) + _offset, _smoothing);
        }
    }
}
