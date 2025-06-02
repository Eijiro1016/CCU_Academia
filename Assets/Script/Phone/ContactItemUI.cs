using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactItemUI : MonoBehaviour {
    [Header("UI 元件")]
    public TMP_Text nameText;
    public TMP_Text styleText;
    public Image styleBackground;
    public TMP_Text summaryText;
    public Button contactButton;

    public ChatUIManager chatUIManager;  // 指派用

    public void Setup(int npcID, string npcName, string npcStyle, Color styleColor, string npcTitle) {
        nameText.text = npcName;
        styleText.text = npcStyle;
        styleBackground.color = styleColor;
        summaryText.text = npcTitle;

        GetComponent<Button>().onClick.AddListener(() => {
            chatUIManager.OpenChat(npcID, npcName);
        });

        contactButton.onClick.AddListener(() => {
            chatUIManager.OpenChat(npcID, npcName);
        });
    }
}
