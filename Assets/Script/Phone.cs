using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ✅ 玩家手機 UI 控制器
/// - 可按 Tab 開啟手機
/// - 可輸入訊息給 AI
/// - 顯示 NPC 的回應
/// </summary>
public class Phone : MonoBehaviour
{
    public GameObject phonePanel;                 // 手機 UI 面板本體
    public TMP_InputField playerInput;            // 玩家輸入框（TextMeshPro）
    public TMP_Text npcResponseText;              // 顯示 AI 回覆的文字欄位
    public OllamaDialog ollamaDialog;             // 與本地 AI 模型通訊的腳本

    private bool isPhoneVisible = false;          // 手機目前是否開啟

    private void Start()
    {
        // 綁定輸入框的送出事件（按 Enter 時觸發）
        playerInput.onSubmit.AddListener(HandleSubmit);
    }

    private void Update()
    {
        // 按下 Tab 鍵切換手機開關
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPhoneVisible = !isPhoneVisible;
            phonePanel.SetActive(isPhoneVisible);

            if (isPhoneVisible)
            {
                // 每次開啟時清空輸入並聚焦
                playerInput.text = "";
                playerInput.ActivateInputField();
            }
        }
    }

    /// <summary>
    /// ✅ 當按下 Enter 時由 TMP 輸入框觸發
    /// </summary>
    private void HandleSubmit(string text)
    {
        SendMessageToNPC();
    }

    /// <summary>
    /// ✅ 傳送訊息給 Ollama 並顯示 AI 回應
    /// </summary>
    void SendMessageToNPC()
    {
        string message = playerInput.text.Trim();

        // 避免送出空白訊息
        if (string.IsNullOrEmpty(message)) return;

        npcResponseText.text = "（等待回應中...）"; // 顯示等待提示

        // 呼叫 Ollama API 並顯示回覆
        ollamaDialog.GetResponse(message, response =>
        {
            npcResponseText.text = response;
        });

        // 清空輸入並重新聚焦
        playerInput.text = "";
        playerInput.ActivateInputField();
    }
}
