using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // 如果你使用 TextMeshPro

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Overworld"; // 目標場景名稱
    [SerializeField] private Slider loadingSlider;            // 進度條 UI
    [SerializeField] private TextMeshProUGUI tipText;         // 提示語句（可改為 Text）
    [SerializeField] private float fakeLoadTime = 0.5f;       // 進度條流暢性微調
    [SerializeField] private TextMeshProUGUI percentText; // 進度%數

    [SerializeField] private float minLoadTime = 15f;  // 最少顯示的秒數

    private readonly string[] tips = new string[]
    {
        "傳聞中，路邊遇到的鳳梨都是沒有畢業的學生轉生而來。",
        "小提示：按 F 與 NPC 對話。",
        "在校園內，遇到什麼都不要覺得驚訝。",
        "小提示：按 TAB 可以開啟手機介面。"
    };

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
        StartCoroutine(CycleTips());
    }

    IEnumerator LoadSceneAsync()
    {
        float startTime = Time.time;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // 第一階段：載入到 0.5
        while (asyncLoad.progress < 0.5f)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, asyncLoad.progress, fakeLoadTime);
            percentText.text = $"載入中... {(loadingSlider.value * 100f):0}%";
            yield return null;
        }

        // 第二階段：補進度條到 100%
        while (loadingSlider.value < 1f)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, 1f, fakeLoadTime);
            percentText.text = $"載入中... {(loadingSlider.value * 100f):0}%";
            yield return null;
        }

        // 新增：補足 minLoadTime 時間
        float elapsedTime = Time.time - startTime;
        float remainingTime = minLoadTime - elapsedTime;
        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 新增：顯示「已完成，請稍後...」1.5 秒
        percentText.text = " 已完成，請稍後...";
        yield return new WaitForSeconds(1.5f);

        asyncLoad.allowSceneActivation = true;
    }


    IEnumerator CycleTips()
    {
        int index = 0;
        while (true)
        {
            tipText.text = tips[index % tips.Length];
            index++;
            yield return new WaitForSeconds(3f); // 每 3 秒切換提示
        }
    }
}
