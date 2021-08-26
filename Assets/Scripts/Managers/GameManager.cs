using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform _map;
    [SerializeField] private float _scrollSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        GlobalEvents.OnReachedEndOfLevel += StopScrollingMap;
    }
    private void OnDisable()
    {
        GlobalEvents.OnReachedEndOfLevel -= StopScrollingMap;
    }

    private void Start()
    {
        _scrollMap = StartCoroutine(ScrollMap());
    }

    private Coroutine _scrollMap;
    private IEnumerator ScrollMap()
    {
        if (_map == null) yield break;
        while (true)
        {
            _map.position = new Vector2(_map.position.x, _map.position.y - _scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private void StopScrollingMap() => StopCoroutine(_scrollMap);
}
