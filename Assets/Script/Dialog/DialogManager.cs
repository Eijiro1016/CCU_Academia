using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ✅ 管理對話框的顯示、逐字輸出與控制流程的系統
/// </summary>
public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;      // 對話框 UI 物件
    [SerializeField] Text dialogText;           // 顯示對話文字的 UI Text
    [SerializeField] int lettersPerSecond;      // 每秒顯示幾個字（打字速度）

    // 事件：對話開始 / 結束時通知外部（例如暫停遊戲）
    public event Action OnShowDialog;
    public event Action OnHideDialog;

    // 單例模式（方便其他腳本存取）
    public static DialogManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // 對話狀態紀錄
    int currentLine = 0;       // 當前顯示到第幾行
    Dialog dialog;             // 目前要顯示的 Dialog 資料
    bool isTyping = false;     // 是否正在逐字顯示文字中

    /// <summary>
    /// ✅ 開始顯示一整段對話（由 Dialog 提供）
    /// </summary>
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();       // 通知遊戲其他部分「對話開始」
        this.dialog = dialog;

        dialogBox.SetActive(true);    // 開啟對話框 UI
        StartCoroutine(TypeDialog(dialog.Lines[0])); // 顯示第一行
    }

    /// <summary>
    /// ✅ 對話中每次按下鍵盤（通常是 F）切到下一句
    /// </summary>
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                // 對話結束，關閉 UI 並通知外部
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
            }
        }
    }

    /// <summary>
    /// ✅ 逐字輸出單行對話內容
    /// </summary>
    public IEnumerator TypeDialog(string lines)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (var letter in lines.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }
}
