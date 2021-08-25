using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [SerializeField] private Image _transitionScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        _transitionScreen.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
        _transitionScreen.gameObject.SetActive(false);
    }

    public static void LoadScene(int index)
    {
        Instance.StartCoroutine(Instance.SceneTransition(index));
    }

    private IEnumerator SceneTransition(int index)
    {
        _transitionScreen.gameObject.SetActive(true);

        const float duration = 0.5f;
        for (float i = 0; i < 1; i += Time.deltaTime / duration)
        {
            _transitionScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, i));
            yield return null;
        }

        AsyncOperation ao = SceneManager.LoadSceneAsync(index);

        while (!ao.isDone) yield return null;

        for (float i = 0; i < 1; i += Time.deltaTime / duration)
        {
            _transitionScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, i));
            yield return null;
        }

        _transitionScreen.gameObject.SetActive(false);
    }
}
