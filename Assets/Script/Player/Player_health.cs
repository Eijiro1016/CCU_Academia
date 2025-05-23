using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ? 控制玩家血量、死亡與（可選）受擊擊退邏輯
/// </summary>
public class Player_health : MonoBehaviour
{
    private float health = 0f;                      // 目前血量
    [SerializeField] private float maxHealth = 100f; // 最大血量（可在 Inspector 調整）

    //public Header("Knockback 設定")
    public float knockbackForce = 5f;               // 擊退時的力道大小（目前未使用）

    private Rigidbody2D rb;                         // 玩家剛體，用來取得位移與物理相關操作

    private void Start()
    {
        health = maxHealth;                         // 遊戲開始時初始化血量
        rb = GetComponent<Rigidbody2D>();           // 抓取 Rigidbody2D 組件
    }

    /// <summary>
    /// ? 更新血量數值（正值為回血，負值為扣血）
    /// 可選擇傳入擊退方向作為 knockback
    /// </summary>
    /// <param name="mod">血量變化量（正數加血，負數扣血）</param>
    /// <param name="knockbackDir">可選：受擊時的方向向量</param>
    public void UpdateHealth(float mod, Vector2? knockbackDir = null)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth; // 不超過最大值
        }
        else if (health <= 0f)
        {
            health = 0f;        // 死亡處理（此處暫時只印出訊息）
            Debug.Log("Player Respawn");
        }

        // ? 以下為擊退功能（目前註解）
        // 若啟用可讓玩家受到攻擊後向後推一段距離（需手動解開以下區塊）

        //// Knockback 手動位移（針對 Kinematic 玩家）
        //if (knockbackDir.HasValue)
        //{
        //    Vector3 displacement = (Vector3)(knockbackDir.Value.normalized * knockbackForce * 0.1f); // 調整縮放
        //    transform.position += displacement;
        //}
    }
}
