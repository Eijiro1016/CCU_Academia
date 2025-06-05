using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    int currentLine = 0;
    Dialog dialog;
    bool isTyping = false;
    private Coroutine typingCoroutine = null;
    private bool isDialogFinished = false; // ? 對話結束旗標

    public IEnumerator ShowDialog(Dialog dialog)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        OnShowDialog?.Invoke();
        this.dialog = dialog;
        currentLine = 0;
        isDialogFinished = false;

        dialogBox.SetActive(true);
        typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));

        // ? 等待對話整體流程結束
        while (!isDialogFinished)
        {
            yield return null;
        }

        yield return null;
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = dialog.Lines[currentLine];
                isTyping = false;
                return;
            }

            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                typingCoroutine = null;
                OnHideDialog?.Invoke();
                isDialogFinished = true; // ? 標記對話完成
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }

    public bool IsDialogActive()
    {
        return dialogBox.activeSelf;
    }
}
