// ✅ 整合 player_control 的功能版本
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float knockbackForce = 5f;
    public int cherry = 0;

    private bool isMoving;
    private bool isHurt;
    private Vector2 input;
    private Animator animator;
    private Rigidbody2D rb;

    public LayerMask SolidObject_layer;
    public LayerMask Interactable_layer;

    private void Awake()
    {
    
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) Debug.LogError("Animator component not found on this object!");
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // 僅允許單一方向輸入（上下左右）
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 direction = new Vector3(input.x, input.y).normalized;
                Vector3 targetPos = transform.position + direction * moveSpeed * Time.deltaTime;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        if (!isHurt)
        {
            animator.SetBool("is_moving", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, Interactable_layer);
        if (collider != null)
        {
            collider.GetComponent<interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        if ((targetPos - transform.position).sqrMagnitude < 0.001f)
        {
            isMoving = false;
            yield break;
        }

        Debug.Log("開始移動至: " + targetPos);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        animator.SetBool("is_moving", false);
        Debug.Log("移動結束，位置：" + transform.position);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.2f, SolidObject_layer);
        if (hit != null)
        {
            Debug.Log("被擋住的是：" + hit.name);
        }
        return hit == null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collection")
        {
            Destroy(collision.gameObject);
            cherry += 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float knockbackDirection = (transform.position.x < collision.transform.position.x) ? -1f : 1f;
            rb.linearVelocity = new Vector2(knockbackDirection * knockbackForce, rb.linearVelocity.y);
            isHurt = true;
        }
    }
}
