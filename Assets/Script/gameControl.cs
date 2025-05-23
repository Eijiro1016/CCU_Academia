using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ✅ 遊戲狀態列舉類型
/// </summary>
public enum GameState
{
    FreeRoam,  // 玩家自由移動狀態
    Dialog,    // 正在對話中（無法控制角色）
    Battle     // 進入戰鬥（預留）
}

/// <summary>
/// ✅ 遊戲總控制器，管理整體狀態與輸入切換
/// </summary>
public class gameControl : MonoBehaviour
{
    [SerializeField] player_control player_Control; // 參考玩家控制腳本

    GameState state; // 目前的遊戲狀態（預設為 FreeRoam）

    private void Start()
    {
        // 載入主選單畫面（Additive 方式不會覆蓋現有場景）
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

        // 當 DialogManager 發出「對話開始」事件時，切換遊戲狀態為 Dialog
        DialogManager.instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        // 當對話結束時（DialogManager 通知），回復為自由移動狀態
        DialogManager.instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        // 根據不同狀態進行不同輸入處理
        if (state == GameState.FreeRoam)
        {
            // 玩家可以自由移動與互動
            player_Control.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            // 對話狀態時，交由 DialogManager 控制對話流程
            DialogManager.instance.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            // ⚠️ 尚未實作戰鬥狀態
        }
    }
}
