using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ? 表示一段 NPC 或劇情對話資料的 ScriptableObject
/// 可以在 Unity 專案中建立多個 Dialog 資料，用於儲存文字對話內容
/// </summary>
[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    // ? 儲存多行對話文字（每一行代表一段逐句顯示）
    [SerializeField] List<string> lines = new List<string>();

    // ? 公開唯讀屬性，讓其他類別可存取這些對話
    public List<string> Lines => lines;

    /// <summary>
    /// ? 允許外部一次設定整組對話內容
    /// </summary>
    public void SetLines(List<string> newLines)
    {
        lines = newLines;
    }
}
