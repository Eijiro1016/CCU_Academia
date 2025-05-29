using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ✅ 控制 NPC 與玩家互動的行為（不使用 AI）：
/// 1. 顯示開場白
/// 2. 顯示預設對話內容
/// </summary>
public class NPCcontrol : MonoBehaviour, interactable
{
    [TextArea]
    public string openingLine = "你好，歡迎來到村子！"; // 每次對話的開場白

    [TextArea(3, 10)]
    public List<string> dialogLines; // 全部的對話內容（可在 Inspector 自訂）

    /// <summary>
    /// 玩家按下互動鍵時觸發（由 interactable 系統呼叫）
    /// </summary>
    public void Interact()
    {
        StartCoroutine(StartDialogSequence());
    }

    /// <summary>
    /// 依序顯示開場白和完整對話
    /// </summary>
    private IEnumerator StartDialogSequence()
    {
        // 顯示開場白
        Dialog opening = new DialogBuilder().AddLine(openingLine).Build();
        yield return DialogManager.instance.ShowDialog(opening);

        yield return new WaitForSeconds(0.5f);

        // 顯示主要對話內容
        DialogBuilder builder = new DialogBuilder();
        foreach (string line in dialogLines)
        {
            builder.AddLine(line);
        }

        Dialog fullDialog = builder.Build();
        yield return DialogManager.instance.ShowDialog(fullDialog);
    }
}
