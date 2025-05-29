// Code written by tutmo (youtube.com/tutmo)
// For help, check out the tutorial - https://youtu.be/PNWK5o9l54w

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // ~~ 1. Controls All Player Movement
    // ~~ 2. Updates Animator to Play Idle & Walking Animations

    private float speed = 10f;
    private Rigidbody2D myRigidbody;
    private Vector3 input;
    private Animator animator;

    public LayerMask SolidObject_layer;     // 碰撞層（不能穿越）
    public LayerMask Interactable_layer;    // 可互動對象層（如 NPC）

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        input = Vector3.zero;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        HandleUpdate();
    }

    public void HandleUpdate()
    {
        if (input != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        // 按下 F 鍵執行互動
        if (Input.GetKeyDown(KeyCode.F))
        {
            interact();
        }
    }

    void interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        // 嘗試在面前找到可互動的物件
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, Interactable_layer);
        if (collider != null)
        {
            collider.GetComponent<interactable>()?.Interact();
        }
    }

    private void MoveCharacter()
    {
        myRigidbody.MovePosition(transform.position + input * speed * Time.deltaTime);
    }
}
