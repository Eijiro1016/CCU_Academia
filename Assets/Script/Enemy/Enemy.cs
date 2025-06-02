using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("移動與攻擊設定")]
    public float speed = 10f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public float attackSpeed = 1f;
    public float canAttack;

    private Transform target;
    private SpriteRenderer spriteRenderer;

    [Header("四方向圖")]
    public Sprite spriteDown;   // cars_0
    public Sprite spriteLeft;   // cars_0_1
    public Sprite spriteRight;  // cars_1
    public Sprite spriteUp;     // cars_4

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);

            // ✅ 根據方向切換 sprite
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // 左右
                spriteRenderer.sprite = direction.x > 0 ? spriteRight : spriteLeft;
            }
            else
            {
                // 上下
                spriteRenderer.sprite = direction.y > 0 ? spriteUp : spriteDown;
            }
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
