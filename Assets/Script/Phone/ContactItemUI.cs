using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactItemUI : MonoBehaviour {
    [Header("UI 元件")]
    public TMP_Text nameText;
    public TMP_Text styleText;
    public Image styleBackground;
    public TMP_Text summaryText;

    // 設定 NPC 顯示資料與樣式顏色
    public void Setup(string npcName, string npcStyle, Color styleColor, string npcTitle) {
        nameText.text = npcName;
        styleText.text = npcStyle;
        styleBackground.color = styleColor;
        summaryText.text = npcTitle;
    }
}
