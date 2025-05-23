using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

#region 對應 JSON 結構用的資料類別

// ✅ 傳送給 API 的請求資料結構
[System.Serializable]
public class ChatRequest
{
    public string character; // 角色名稱（用來判斷是誰在對話）
    public string message;   // 玩家輸入的訊息
}

// ✅ 從 API 接收到的回應資料結構
[System.Serializable]
public class ChatResponse
{
    public string response; // GPT 或 AI 回覆的訊息
}

#endregion

/// <summary>
/// ✅ 負責與本地端 Ollama 語言模型伺服器通訊的腳本
/// 可提供 NPC 使用 AI 對話
/// </summary>
public class OllamaDialog : MonoBehaviour
{
    [Header("設定")]
    public string characterName = "莉亞"; // 傳送給後端的角色名，代表哪位 NPC
    public string apiUrl = "http://127.0.0.1:5000/api/chat"; // 調用本地 Flask API 的網址

    /// <summary>
    /// 呼叫方法，發送訊息並接收回應
    /// </summary>
    /// <param name="userInput">玩家輸入的訊息</param>
    /// <param name="onReply">回調函式，用來處理收到的回應文字</param>
    public void GetResponse(string userInput, System.Action<string> onReply)
    {
        StartCoroutine(SendChat(characterName, userInput, onReply));
    }

    /// <summary>
    /// 發送 HTTP POST 請求到 Flask API
    /// </summary>
    IEnumerator SendChat(string character, string message, System.Action<string> onReply)
    {
        // 將角色與訊息組成 JSON 格式
        ChatRequest requestData = new ChatRequest
        {
            character = character,
            message = message
        };

        // 將物件轉換成 JSON 字串，再轉成 byte[]
        string json = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        // 建立 POST 請求
        UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw); // 上傳資料
        www.downloadHandler = new DownloadHandlerBuffer(); // 接收回應
        www.SetRequestHeader("Content-Type", "application/json"); // 告訴伺服器是 JSON

        // 等待回應
        yield return www.SendWebRequest();

        // 處理回應結果
        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseJson = www.downloadHandler.text;
            ChatResponse res = JsonUtility.FromJson<ChatResponse>(responseJson); // 將回應轉為物件

            Debug.Log("🧠 NPC 回應：" + res.response);
            onReply?.Invoke(res.response); // 傳給呼叫者的 callback
        }
        else
        {
            Debug.LogError("❌ 傳送失敗：" + www.error + "\n" + www.downloadHandler.text);
            onReply?.Invoke("（發送失敗）");
        }
    }
}
