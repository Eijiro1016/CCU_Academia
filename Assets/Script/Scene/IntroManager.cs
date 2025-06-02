using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour {
    public GameObject introPanel;        // 劇情 Panel
    public GameObject instructionPanel;  // 操作說明 Panel
    public float introTime = 5f;
    public float initialDelay = 2f;       // 進場延遲（秒）

    private bool canSkipIntro = false;
    // private bool showingInstruction = false;

    void Start() {
        introPanel.SetActive(false);
        instructionPanel.SetActive(false);
        StartCoroutine(ShowIntroAfterDelay());
    }

    void Update() {
        if (introPanel.activeSelf && canSkipIntro && Input.anyKeyDown) {
            SkipToInstruction();
        } else if (instructionPanel.activeSelf && Input.anyKeyDown) {
            StartGame(); // 進入遊戲主邏輯
        }
    }

    IEnumerator ShowIntroAfterDelay() {
        yield return new WaitForSeconds(initialDelay);    // 等幾秒再出現
        introPanel.SetActive(true);
        canSkipIntro = true; // 允許跳過劇情
    }

    // IEnumerator ShowInstructionAfterIntro() {
    //     yield return new WaitForSeconds(introTime);
    //     if(Input.anyKeyDown) {
    //         SkipToInstruction();
    //     }
        
    // }

    void SkipToInstruction() {
        introPanel.SetActive(false);
        instructionPanel.SetActive(true);
        // showingInstruction = true;
    }

    void StartGame() {
        // 如果你有 GameManager 就從這裡啟動主角控制器或場景流程
        instructionPanel.SetActive(false);
        Debug.Log("? 遊戲開始！");
    }
}
