using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 0.6f;

    bool isFading;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(Fade(1f, 0f));
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ★ 关键：在这里重置
        isFading = false;

        if (fadeImage == null) return;

        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f));
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"[Fader] LoadScene({sceneName}) isFading={isFading}");

        if (isFading)
        {
            Debug.LogWarning("[Fader] 被 isFading 阻断");
            return;
        }

        StartCoroutine(LoadRoutine(sceneName));
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        isFading = true;

        yield return Fade(0f, 1f);

        SceneManager.LoadScene(sceneName);

        // 注意：这里不需要重置 isFading，由 OnSceneLoaded 负责
    }

    public IEnumerator Fade(float from, float to)
    {
        if (fadeImage == null) yield break;

        float dur = Mathf.Max(0.01f, fadeDuration);
        float t = 0f;

        Color c = fadeImage.color;
        c.a = from;
        fadeImage.color = c;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;

            c.a = Mathf.Lerp(from, to, t / dur);
            fadeImage.color = c;

            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}