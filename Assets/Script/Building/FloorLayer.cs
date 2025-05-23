using UnityEngine;

/// <summary>
/// 控制單一樓層物件是否顯示的腳本（通常掛在一層樓層的 Tilemap 或區塊上）
/// 可與樓層切換控制器（如 FloorZone）搭配使用
/// </summary>
public class FloorLayer : MonoBehaviour
{
    // 是否在遊戲開始時預設顯示這一層（可在 Inspector 設定）
    public bool defaultVisible = false;

    private void Start()
    {
        // 根據設定，自動啟用或關閉此 GameObject
        gameObject.SetActive(defaultVisible);
    }

    /// <summary>
    /// 切換這個樓層是否顯示（可供外部樓層管理器呼叫）
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
