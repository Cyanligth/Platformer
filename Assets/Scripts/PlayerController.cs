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

    private void OnJump(InputValue input)
    {
        Jump();
    }
    public void Jump()
    {
        // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.velocity += Vector2.up * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("IsGrounded", true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsGrounded", false);
    }
}
