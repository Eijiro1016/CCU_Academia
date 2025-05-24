//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// ✅ 玩家控制器：負責移動、互動、收集物品與受傷後反應
///// </summary>
//public class player_control : MonoBehaviour
//{
//    public float moveSpeed = 3f; // 玩家移動速度

//    private bool isMoving;       // 玩家是否正在移動
//    private Vector2 input;       // 玩家輸入方向
//    private Animator animator;   // 控制角色動畫
//    public int cherry = 0;       // 玩家收集到的櫻桃數

//    public float knockbackForce = 5f; // 被敵人打到時的擊退力量
//    private Rigidbody2D rb;           // 玩家剛體，用於物理推移
//    private bool isHurt;              // 玩家是否處於受傷狀態（例如被打退）

//    public LayerMask SolidObject_layer;     // 碰撞層（不能穿越）
//    public LayerMask Interactable_layer;    // 可互動對象層（如 NPC）

//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody2D>();

//        if (animator == null)
//        {
//            Debug.LogError("Animator component not found on this object!");
//        }
//    }

//    /// <summary>
//    /// ✅ 每幀更新：處理玩家輸入與移動邏輯
//    /// </summary>
//    public void HandleUpdate()
//    {
//        if (!isMoving)
//        {
//            // 取得輸入方向（只有四方向，沒有對角線）
//            input.x = Input.GetAxisRaw("Horizontal");
//            input.y = Input.GetAxisRaw("Vertical");

//            if (input != Vector2.zero)
//            {
//                // 記錄方向供動畫使用
//                animator.SetFloat("moveX", input.x);
//                animator.SetFloat("moveY", input.y);

//                // 移動方向正規化（避免斜向變快）
//                Vector3 direction = new Vector3(input.x, input.y).normalized;
//                Vector3 targetPos = transform.position + direction * moveSpeed * Time.deltaTime;

//                // 判斷是否可走
//                if (isWalkable(targetPos))
//                {
//                    StartCoroutine(Move(targetPos));
//                }
//            }
//        }

//        if (!isHurt)
//        {
//            animator.SetBool("is_moving", isMoving);
//        }

//        // 按下 F 鍵執行互動
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            interact();
//        }
//    }

//    /// <summary>
//    /// ✅ 與面前的物件互動（如 NPC）
//    /// </summary>
//    void interact()
//    {
//        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
//        var interactPos = transform.position + facingDir;

//        // 嘗試在面前找到可互動的物件
//        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, Interactable_layer);
//        if (collider != null)
//        {
//            collider.GetComponent<interactable>()?.Interact();
//        }
//    }

//    /// <summary>
//    /// ✅ 使用協程平滑移動
//    /// </summary>
//    IEnumerator Move(Vector3 targetPos)
//    {
//        isMoving = true;

//        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
//            yield return null;
//        }

//        transform.position = targetPos;
//        isMoving = false;

//        animator.SetBool("is_moving", false);
//    }

//    /// <summary>
//    /// ✅ 判斷目標位置是否可通行
//    /// </summary>
//    private bool isWalkable(Vector3 targetPos)
//    {
//        bool hit = Physics2D.OverlapCircle(targetPos, 0.2f, SolidObject_layer) != null;
//        return !hit;
//    }

//    /// <summary>
//    /// ✅ 玩家碰到收集物（如櫻桃）時觸發
//    /// </summary>
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.tag == "collection")
//        {
//            Destroy(collision.gameObject);
//            cherry += 1;
//        }
//    }

//    /// <summary>
//    /// ✅ 玩家被敵人碰撞時受擊反應（推開）
//    /// </summary>
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.tag == "Enemy")
//        {
//            // 根據玩家與敵人位置計算擊退方向
//            if (transform.position.x < collision.transform.position.x)
//            {
//                rb.linearVelocity = new Vector2(-knockbackForce, rb.linearVelocity.y);
//            }
//            else
//            {
//                rb.linearVelocity = new Vector2(knockbackForce, rb.linearVelocity.y);
//            }

//            isHurt = true;
//        }
//    }

//    // 📝 備用版本（直接摧毀敵人）
//    //private void OnCollisionEnter2D(Collision2D collision)
//    //{
//    //    if (collision.gameObject.tag == "Enemy")
//    //    {
//    //        Destroy(collision.gameObject);
//    //    }
//    //}
//}
