using UnityEngine;

/// <summary>
/// ✅ 控制敵人偵測範圍的腳本
/// 當玩家進入此範圍（例如大圈圈）時，敵人會開始追蹤玩家
/// 當玩家離開此範圍，敵人會停止追蹤
/// </summary>
public class DetectionZone : MonoBehaviour
{
    private Enemy enemy; // 引用父物件上的 Enemy 腳本

    void Start()
    {
        // 從父物件取得 Enemy 腳本（通常父物件就是整個敵人）
        enemy = GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// 玩家進入偵測範圍時觸發
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 設定追蹤目標為玩家
            // Debug.Log("🎯 玩家進入偵測圈，開始追蹤！");
            enemy.SetTarget(collision.transform);
        }
    }

    /// <summary>
    /// 玩家離開偵測範圍時觸發
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 清除追蹤目標，讓敵人停止追蹤
            // Debug.Log("🛑 玩家離開偵測圈，停止追蹤！");
            enemy.SetTarget(null);
        }
    }
}
