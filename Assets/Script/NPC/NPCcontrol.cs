using System.Collections;
using UnityEngine;

/// <summary>
/// ✅ 控制 NPC 與玩家互動的行為：
/// 1. 顯示開場白
/// 2. 呼叫 Ollama AI 回覆對話
/// </summary>
public class NPCcontrol : MonoBehaviour, interactable
{
    [SerializeField] string characterName = "莉亞";  // 傳給 Ollama 用的角色名，對應 JSON 中的角色設定

    [TextArea]
    public string defaultUserInput = "最近村子裡有什麼事？";  // 預設玩家會問的話（可自定）

    [TextArea]
    public string openingLine = "你好，歡迎來到村子！"; // 每次對話的開場白

    private OllamaDialog ollama; // 與本地語言模型通訊的腳本實例

    private void Awake()
    {
        // 在 NPC 身上動態加上 OllamaDialog 腳本
        ollama = gameObject.AddComponent<OllamaDialog>();
        ollama.characterName = characterName; // 指定角色身份
    }

    /// <summary>
    /// ✅ 玩家按下互動鍵時觸發（由 interactable 系統呼叫）
    /// </summary>
    public void Interact()
    {
        // 1️⃣ 顯示固定的開場白
        Dialog dialog = new DialogBuilder().AddLine(openingLine).Build();
        Debug.Log("👋 開場白：" + openingLine);

        // 啟動對話協程
        StartCoroutine(ShowOpeningAndThenAsk(dialog));
    }

    /// <summary>
    /// ✅ 協程流程：開場白結束 → 再顯示 AI 回覆
    /// </summary>
    private IEnumerator ShowOpeningAndThenAsk(Dialog opening)
    {
        // 顯示開場白
        yield return DialogManager.instance.ShowDialog(opening);

        // 稍作停頓
        yield return new WaitForSeconds(0.5f);

        // 呼叫 Ollama 取得 AI 回應
        ollama.GetResponse(defaultUserInput, (reply) =>
        {
            // 顯示 AI 回覆
            Dialog dialog = new DialogBuilder().AddLine(reply).Build();
            Debug.Log("🧠 AI 回覆：" + reply);
            StartCoroutine(DialogManager.instance.ShowDialog(dialog));
        });
    }
}
