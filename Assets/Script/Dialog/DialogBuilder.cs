// using System.Collections.Generic;
// using UnityEngine;

// /// <summary>
// /// ✅ 用來建構一個 Dialog（ScriptableObject）的工具類別
// /// 支援以鏈式寫法逐行加入對話，最後產出一個 Dialog 實例
// /// </summary>
// public class DialogBuilder : MonoBehaviour
// {
//     // 🔸 暫存對話內容的文字列表
//     private List<string> lines = new List<string>();

//     /// <summary>
//     /// ✅ 加入一行對話文字
//     /// </summary>
//     /// <param name="line">單行對話內容</param>
//     /// <returns>回傳自己本身（支援鏈式呼叫）</returns>
//     public DialogBuilder AddLine(string line)
//     {
//         lines.Add(line);
//         return this;
//     }

//     /// <summary>
//     /// ✅ 建立一個 Dialog ScriptableObject 並填入目前的對話行
//     /// </summary>
//     /// <returns>產出的 Dialog 實例</returns>
//     public Dialog Build()
//     {
//         Dialog dialog = ScriptableObject.CreateInstance<Dialog>(); // 動態產生一個 ScriptableObject
//         dialog.SetLines(lines);                                     // 將累積的對話行設進去
//         return dialog;
//     }
// }
