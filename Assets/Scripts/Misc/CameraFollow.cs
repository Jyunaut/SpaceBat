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

    private void OnDrawGizmos()
    {
        Vector2 cameraOrigin = Camera.main.transform.position;
        Vector2 cameraExtents = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height,
                                            Camera.main.orthographicSize * Screen.width / Screen.height));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cameraOrigin + new Vector2(cameraExtents.x, cameraExtents.y), cameraOrigin + new Vector2(cameraExtents.x, cameraExtents.y + 40f));
        Gizmos.DrawLine(cameraOrigin + new Vector2(-cameraExtents.x, cameraExtents.y), cameraOrigin + new Vector2(-cameraExtents.x, cameraExtents.y + 40f));
    }
}
