using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Awake()
    {
        // 避免重複建立
        if (FindObjectsOfType<SceneTransition>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // 切場景也保留
    }

    void Start()
    {
        // Scene 加載完後淡入
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOutAndSwitch(sceneIndex));
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0);
    }

    IEnumerator FadeOutAndSwitch(int sceneIndex)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1);
        SceneManager.LoadScene(sceneIndex);
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Debug.Log($"[SceneTransition] SetAlpha to {alpha}");
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}
