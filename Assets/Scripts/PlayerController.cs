using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxSpeed;

    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 dir;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void OnMove(InputValue input)
    {
        dir = input.Get<Vector2>();
        animator.SetFloat("MoveSpeed", Mathf.Abs(dir.x));
        if(dir.x >= 0)
            spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }
    public void Move()
    {
        if(dir.x < 0 && rb.velocity.x > -maxSpeed)
            rb.AddForce(Vector2.right * dir.x * moveForce, ForceMode2D.Force);
        else if(dir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * dir.x * moveForce, ForceMode2D.Force);
    }
    private bool isGround;
    private void OnJump(InputValue input)
    {
        if(isGround)
            Jump();
    }
    public void Jump()
    {
        // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.velocity += Vector2.up * jumpForce;
    }

    private void GroundCheck()
    {
        // Physics2D.RaycastAll()   장해물 무시하고 레이저 거리 내의 모든 오브젝트를 배열로 받아옴
        // Physics2D.BoxCast()      대충 형태별로도 있다는 뜻
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            isGround = true;
            animator.SetBool("IsGrounded", true);
            Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);    
        }
        else
        {
            isGround = false;
            animator.SetBool("IsGrounded", false);
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.red);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = true;
        animator.SetBool("IsGrounded", true);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
        animator.SetBool("IsGrounded", false);
    }
    
}
