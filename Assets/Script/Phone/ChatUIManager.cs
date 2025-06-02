using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ChatUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject contactPanel;
    public GameObject chatPanel;

    [Header("Chat Components")]
    public TMP_Text npcNameText;
    public TMP_InputField inputField;
    public Button backButton;
    public Button sendButton;

    [Header("Bubble UI")]
    public Transform contentTransform;
    public ScrollRect scrollRect;
    public GameObject userBubblePrefab;
    public GameObject npcBubblePrefab;

    [Header("API")]
    public OllamaAPI ollama;

    private int currentNpcID = -1;
    private string currentNpcName = "";

    void Start(){
        contactPanel.SetActive(true);
        chatPanel.SetActive(false);
        backButton.onClick.AddListener(BackToContacts);
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    public void OpenChat(int npcID, string npcName){
        currentNpcID = npcID;
        currentNpcName = npcName;
        npcNameText.text = npcName;

        contactPanel.SetActive(false);
        chatPanel.SetActive(true);

        LoadHistory();
    }

    private void LoadHistory(){
        Debug.Log($"[LoadHistory] 呼叫 GetHistory for ID = {currentNpcID}");
        ClearChat();
        ollama.GetHistory(currentNpcID, (historyList) => {
            foreach (var msg in historyList) {
                bool isUser = msg.role == "user";
                AppendChatLine((isUser ? "你：" : currentNpcName + "：") + msg.content, isUser);
            }
        });
    }

    public void OnSendButtonClicked() {
        StartCoroutine(DelayedSubmit());
    }

    private System.Collections.IEnumerator DelayedSubmit() {
        //yield return null; // 等一幀

        string input = inputField.text.Trim();

        if (string.IsNullOrEmpty(input)) yield break;

        inputField.text = "";
        AppendChatLine($"你：{input}", true);

        ollama.SendMessage(currentNpcID, input, (reply) => {
            AppendChatLine($"{currentNpcName}：{reply}", false);
        });

        inputField.ActivateInputField(); // 讓輸入框再次聚焦
    }


    private void AppendChatLine(string line, bool isUser){
        Debug.Log($"AppendChatLine: {line} (isUser: {isUser})");
        
        var prefab = isUser ? userBubblePrefab : npcBubblePrefab;
        var go = Instantiate(prefab, contentTransform);
        go.GetComponentInChildren<TextMeshProUGUI>().text = line;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void ClearChat(){
        foreach (Transform child in contentTransform){
            Destroy(child.gameObject);
        }
    }

    private void BackToContacts(){
        contactPanel.SetActive(true);
        chatPanel.SetActive(false);
    }
}
