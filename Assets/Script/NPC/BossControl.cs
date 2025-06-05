using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossControl : MonoBehaviour, interactable {
    [SerializeField] private Dialog dialogData;

    public void Interact() {
        if (DialogManager.instance.IsDialogActive()) return;
        StartCoroutine(HandleFinalDialog());
    }

    private IEnumerator HandleFinalDialog() {
        yield return DialogManager.instance.ShowDialog(dialogData);
        // 等對話完全結束後，切換場景
        yield return new WaitForSeconds(1f); // 加一點過場緩衝
        SceneManager.LoadScene("EndingScene");
    }
}
