using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class GPTDialog : MonoBehaviour
{
    // ✅ GPT API 金鑰（請替換為你自己的 key，正式版建議不要硬編碼）
    public string apiKey = "sk-xxxxxxx"; 

    // ✅ NPC 的初始角色設定提示，用來決定 GPT 回應的風格
    public string npcPrompt = "你是村莊裡一位和善的老人，會回答旅人的問題。";

    /// <summary>
    /// 發送對話給 GPT，並透過 callback 回傳回應內容
    /// </summary>
    /// <param name="userMessage">玩家輸入的訊息</param>
    /// <param name="callback">接收 GPT 回覆的函式</param>
    public IEnumerator GetResponseFromGPT(string userMessage, System.Action<string> callback)
    {
        string url = "https://api.openai.com/v1/chat/completions"; // GPT chat API 的網址

        // 將訊息組成 JSON 字串，包含 system（NPC設定）與 user（玩家輸入）
        string jsonBody = @"{
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [
                {""role"": ""system"", ""content"": """ + npcPrompt + @"""},
                {""role"": ""user"", ""content"": """ + userMessage + @"""}
            ]
        }";

        // 建立 POST 請求
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody); // 編碼 JSON 成為 byte[]
        request.uploadHandler = new UploadHandlerRaw(bodyRaw); // 載入上傳資料
        request.downloadHandler = new DownloadHandlerBuffer(); // 準備接收下載資料
        request.SetRequestHeader("Content-Type", "application/json"); // 告訴伺服器內容格式
        request.SetRequestHeader("Authorization", "Bearer " + apiKey); // 加入 API 金鑰

        yield return request.SendWebRequest(); // 等待回應

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text; // 回傳的 JSON 文字

            try
            {
                // 嘗試解析 GPT 回傳的 JSON 資料
                GPTResponse response = JsonUtility.FromJson<GPTResponse>(result);
                string reply = response.choices[0].message.content; // 取第一筆回應內容
                Debug.Log("✅ GPT 真實回應內容：" + reply);
                callback?.Invoke(reply); // 呼叫 callback 回傳結果
            }
            catch
            {
                // JSON 格式錯誤時的處理
                Debug.LogError("❌ 無法解析 GPT 回應，原始文字如下：\n" + result);
                callback?.Invoke("⚠️ 無法解析 GPT 回應。");
            }
        }
        else
        {
            // 連線或伺服器錯誤時的處理
            callback?.Invoke("⚠️ 請求失敗：" + request.error + "\n" + request.downloadHandler.text);
        }
    }
}

// ✅ 用來對應 GPT API 回傳 JSON 的資料結構
[System.Serializable]
public class GPTResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}
