using UnityEngine;

/// <summary>
/// ✅ 掛在敵人的 AttackZone（小範圍觸發區）
/// 當玩家進入並停留時，定期造成傷害與擊退效果
/// </summary>
public class AttackZone : MonoBehaviour
{
    private Enemy enemy; // 取得父物件的 Enemy 腳本，用來讀取攻擊力與攻速

    void Start()
    {
        enemy = GetComponentInParent<Enemy>(); // 在父物件中尋找 Enemy 腳本
    }

    /// <summary>
    /// 玩家持續待在攻擊區時會觸發（每一幀）
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 確保對象是玩家
        {
            var health = collision.GetComponent<Player_health>();
            if (health != null)
            {
                // 使用冷卻時間控制攻擊頻率
                if (enemy.attackSpeed <= enemy.canAttack)
                {
                    // 計算擊退方向（玩家位置 - 敵人位置）
                    Vector2 knockbackDir = (collision.transform.position - transform.position);
                    
                    // 傳送傷害與擊退方向給玩家
                    health.UpdateHealth(-enemy.attackDamage, knockbackDir);

                    // 重置攻擊計時器
                    enemy.canAttack = 0f;
                }
                else
                {
                    // 累積攻擊計時器（類似冷卻時間累加）
                    enemy.canAttack += Time.deltaTime;
                }
            }
        }
    }
}
