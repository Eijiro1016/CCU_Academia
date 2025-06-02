using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]public class MyChatResponse {
    public List<string> responses;
}

[System.Serializable]public class MyChatHistory{
    public List<Conversation> dialogue;
}

[System.Serializable]public class Conversation {
    public string role;
    public string content;
}

[System.Serializable]public class ChatPayload{
    public int character;
    public string message;
}

public class OllamaAPI : MonoBehaviour{
    public string chatUrl = "http://127.0.0.1:5000/api/chat";
    public string historyUrl = "http://127.0.0.1:5000/api/history/";

    public void SendMessage(int npcID, string message, System.Action<string> onReply){
        StartCoroutine(SendChatCoroutine(npcID, message, onReply));
    }

    IEnumerator SendChatCoroutine(int npcID, string message, System.Action<string> onReply){
        ChatPayload payload = new ChatPayload { character = npcID, message = message };
        string json = JsonUtility.ToJson(payload);
        UnityWebRequest www = new UnityWebRequest(chatUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log(www.downloadHandler.text); // 查看回傳 JSON
            MyChatResponse res = JsonUtility.FromJson<MyChatResponse>(www.downloadHandler.text);

            if (res.responses != null && res.responses.Count > 0) {
                foreach (var msg in res.responses) {
                    onReply?.Invoke(msg);
                }
            } else {
                onReply?.Invoke("（伺服器回應為空）");
            }
        } else {
            onReply?.Invoke("（傳送失敗）");
        }
    }

    public void GetHistory(int npcID, System.Action<List<Conversation>> onHistory){
        StartCoroutine(GetHistoryCoroutine(npcID, onHistory));
    }

    IEnumerator GetHistoryCoroutine(int npcID, System.Action<List<Conversation>> onHistory){
        UnityWebRequest www = UnityWebRequest.Get(historyUrl + npcID);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success){
            Debug.Log($"[GET HISTORY] 回傳 JSON: {www.downloadHandler.text}");
            MyChatHistory history = JsonUtility.FromJson<MyChatHistory>(www.downloadHandler.text);
            onHistory?.Invoke(history.dialogue);
        } else {
            Debug.LogWarning($"[GET HISTORY] 失敗: {www.error}");
            onHistory?.Invoke(new List<Conversation>());
        }
    }
}
