using System.Collections;
using UnityEngine;

public class NPCcontrol : MonoBehaviour, interactable
{
    [SerializeField] private Dialog dialogData; // 在 Inspector 中綁定 ScriptableObject

    public void Interact()
    {
        if (DialogManager.instance.IsDialogActive()) return; // 避免重複啟動對話
        StartCoroutine(DialogManager.instance.ShowDialog(dialogData));
    }
}
