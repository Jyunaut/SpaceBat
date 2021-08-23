using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [System.Serializable]
    public struct ScreenShakeData
    {
        public Vector2 magnitude;
        [Range(0f,100f)] public float frequency;
        [Range(0f,2f)] public float duration;
        [Range(0f,20f)] public float damping;
        public bool shakeInOneAxis;
    }

    [System.Serializable]
    public struct TimeSlowData
    {
        [Range(0f,1f)] public float scale;
        [Range(0f,2f)] public float duration;
    }

    public static EffectsManager Instance { get; private set; }

    [SerializeField] Camera _camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SpawnEffect(GameObject effect, Vector2 position, Collider2D targetCol)
    {
        if (effect == null)
        {
            Debug.LogWarning("Effect caanot be found", this);
            return;
        }
        Vector2 spawnPosition = targetCol.bounds.Contains(position) ? position : targetCol.ClosestPoint(position);
        Instantiate(effect, position, Quaternion.identity);
    }
    public void SpawnEffect(GameObject effect, Vector2 position, Vector3 scale)
    {
        if (effect == null)
        {
            Debug.LogWarning("Effect cannot be found", this);
            return;
        }
        GameObject obj = Instantiate(effect, position, Quaternion.identity);
        obj.transform.localScale = scale;
    }

    private Coroutine _screenShake;
    public void ScreenShake(float xMagnitude, float yMagnitude, float frequency, float duration, float damping = 0f, bool shakeInOneAxis = false)
    {
        if (_screenShake != null)
        {
            StopCoroutine(_screenShake);
            _screenShake = null;
        }
        _screenShake = StartCoroutine(ScreenShakeBehaviour(xMagnitude, yMagnitude, frequency, duration, damping, shakeInOneAxis));

        IEnumerator ScreenShakeBehaviour(float xMagnitude, float yMagnitude, float frequency, float duration, float damping = 0f, bool shakeInOneAxis = false)
        {
            float time = 0;
            while (time < duration)
            {
                float x = xMagnitude * Mathf.Exp(-damping * time) * Mathf.Sin(frequency * time + (shakeInOneAxis ? Mathf.PI * time / duration : 0f));
                float y = yMagnitude * Mathf.Exp(-damping * time) * Mathf.Sin(frequency * time - (shakeInOneAxis ? Mathf.PI * time / duration : 0f));
                _camera.transform.localPosition = new Vector3(x, y, 0);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            _camera.transform.localPosition = Vector3.zero;
        }
    }
    public void ScreenShake(ScreenShakeData data)
    {
        if (_screenShake != null)
        {
            StopCoroutine(_screenShake);
            _screenShake = null;
        }
        _screenShake = StartCoroutine(ScreenShakeBehaviour(data));

        IEnumerator ScreenShakeBehaviour(ScreenShakeData data)
        {
            float time = 0;
            while (time < data.duration)
            {
                float x = data.magnitude.x * Mathf.Exp(-data.damping * time) * Mathf.Sin(data.frequency * time + (data.shakeInOneAxis ? Mathf.PI * time / data.duration : 0f));
                float y = data.magnitude.y * Mathf.Exp(-data.damping * time) * Mathf.Sin(data.frequency * time - (data.shakeInOneAxis ? Mathf.PI * time / data.duration : 0f));
                _camera.transform.localPosition = new Vector3(x, y, 0);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            _camera.transform.localPosition = Vector3.zero;
        }
    }

    private Coroutine _timeSlow;
    public void TimeSlow(float scale, float duration, bool smoothSlow = false)
    {
        if (_timeSlow != null)
        {
            StopCoroutine(_timeSlow);
            _timeSlow = null;
        }
        _timeSlow = StartCoroutine(TimeSlowBehaviour());

        IEnumerator TimeSlowBehaviour()
        {
            Time.timeScale = 1f - scale;
            if (smoothSlow) Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
            if (smoothSlow) Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
    public void TimeSlow(TimeSlowData data, bool smoothSlow = false)
    {
        if (_timeSlow != null)
        {
            StopCoroutine(_timeSlow);
            _timeSlow = null;
        }
        _timeSlow = StartCoroutine(TimeSlowBehaviour());

        IEnumerator TimeSlowBehaviour()
        {
            Time.timeScale = 1f - data.scale;
            if (smoothSlow) Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSecondsRealtime(data.scale);
            Time.timeScale = 1f;
            if (smoothSlow) Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }
}
