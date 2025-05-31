using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ✅ 玩家手機 UI 控制器
/// - 可按 Tab 開啟手機
/// - 可輸入訊息給 AI
/// - 顯示 NPC 的回應
/// </summary>
public class Map : MonoBehaviour
{
    public GameObject mapPanel;                 // 手機 UI 面板本體

    private bool isPhoneVisible = false;          // 手機目前是否開啟


    private void Update()
    {
        // 按下 M 鍵切換手機開關
        if (Input.GetKeyDown(KeyCode.M))
        {
            isPhoneVisible = !isPhoneVisible;
            mapPanel.SetActive(isPhoneVisible);
        }
    }

}
