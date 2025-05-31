using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ContactListManager : MonoBehaviour {
    [Header("設定")]
    public GameObject buttonPrefab;
    public Transform contentParent;
    public TextAsset jsonFile;

    private Dictionary<string, Color> styleColors;

    // JSON 對應資料結構
    [System.Serializable]
    public class Dialogue {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class Character {
        public int ID;
        public string name;
        public string style;
        public List<Dialogue> dialogue;
    }

    [System.Serializable]
    public class NPCList {
        public List<Character> characters;
    }

    void Start() {
        InitStyleColors();

        NPCList npcList = JsonConvert.DeserializeObject<NPCList>(jsonFile.text);

        foreach (var npc in npcList.characters) {
            GameObject newItem = Instantiate(buttonPrefab, contentParent);
            ContactItemUI ui = newItem.GetComponent<ContactItemUI>();

             // 安全取得標題（第2句對話）
            string title = (npc.dialogue != null && npc.dialogue.Count > 1) 
                ? npc.dialogue[1].content
                : "（尚無標題）";

            // 預先處理顏色
            Color color = styleColors.TryGetValue(npc.style, out Color c) ? c : Color.gray;

            // 延遲一幀再更新 UI，避免 Graphic Rebuild 錯誤
            StartCoroutine(DelayedSetup(ui, npc.name, npc.style, color, title));
        }
    }

    private void InitStyleColors() {
        styleColors = new Dictionary<string, Color> {
            { "直率",  new Color(0.25f, 0.41f, 0.88f) },
            { "情感",  new Color(1.00f, 0.75f, 0.80f) },
            { "理性",  new Color(0.68f, 0.85f, 0.90f) },
            { "幽默",  new Color(0.75f, 0.75f, 0.75f) },
            { "支持",  new Color(0.60f, 0.80f, 0.60f) },
            { "批判",  new Color(1.00f, 0.47f, 0.47f) },
            { "觀察",  new Color(1.00f, 0.64f, 0.00f) },
            { "建議",  new Color(0.00f, 0.39f, 0.00f) },
            { "好奇",  new Color(1.00f, 1.00f, 0.60f) }
        };
    }

    private IEnumerator DelayedSetup(ContactItemUI ui, string name, string style, Color color, string rawTitle) {
        yield return null; // 等一幀，避免在 UI 初始化中更新 TMP

        // string parsed = EmojiReplacer.Replace(rawTitle);
        ui.Setup(name, style, color,rawTitle);

        Debug.Log("Title" + rawTitle);
    }
}