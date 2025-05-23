using UnityEngine;

/// <summary>
/// ✅ 控制敵人的基本邏輯：追蹤玩家、設定攻擊資訊
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("移動與攻擊設定")]
    public float speed = 10f;             // 敵人移動速度
    [SerializeField] public float attackDamage = 10f; // 攻擊時造成的傷害
    [SerializeField] public float attackSpeed = 1f;   // 攻擊間隔（秒）
    public float canAttack;               // 攻擊冷卻計時器（由 AttackZone 控制）

    private Transform target;            // 玩家位置參考（由 DetectionZone 設定）

    /// <summary>
    /// 每幀更新：若有追蹤目標則朝該方向移動
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            float step = speed * Time.deltaTime;
            // 將敵人位置移動到玩家方向（等速追蹤）
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    /// <summary>
    /// ✅ 由 DetectionZone 設定或清除追蹤目標
    /// </summary>
    public void SetTarget(Transform t)
    {
        target = t;
    }

    // 📝 以下是預備封裝 getter，可選擇是否啟用

    // public float GetAttackDamage()
    // {
    //     return attackDamage;
    // }

    // public float GetAttackSpeed()
    // {
    //     return attackSpeed;
    // }

    // public float GetCanAttack()
    // {
    //     return canAttack;
    // }
}
