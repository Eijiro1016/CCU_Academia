using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]public class MyChatResponse{
    public string response;
}

[System.Serializable]public class MyChatHistory{
    public List<string> history;
}

public class OllamaAPI : MonoBehaviour{
    public string chatUrl = "http://127.0.0.1:5000/api/chat";
    public string historyUrl = "http://127.0.0.1:5000/api/history/";

    public void SendMessage(int npcID, string message, System.Action<string> onReply){
        StartCoroutine(SendChatCoroutine(npcID, message, onReply));
    }

    IEnumerator SendChatCoroutine(int npcID, string message, System.Action<string> onReply){
        var payload = new { character = npcID, message = message };
        string json = JsonUtility.ToJson(payload);
        UnityWebRequest www = new UnityWebRequest(chatUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success){
            MyChatResponse res = JsonUtility.FromJson<MyChatResponse>(www.downloadHandler.text);
            onReply?.Invoke(res.response);
        }
        else{
            onReply?.Invoke("（傳送失敗）");
        }
    }

    public void GetHistory(int npcID, System.Action<List<string>> onHistory){
        StartCoroutine(GetHistoryCoroutine(npcID, onHistory));
    }

    IEnumerator GetHistoryCoroutine(int npcID, System.Action<List<string>> onHistory){
        UnityWebRequest www = UnityWebRequest.Get(historyUrl + npcID);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success){
            MyChatHistory history = JsonUtility.FromJson<MyChatHistory>(www.downloadHandler.text);
            onHistory?.Invoke(history.history);
        }
        else{
            onHistory?.Invoke(new List<string> { "（無法讀取歷史紀錄）" });
        }
    }
}
