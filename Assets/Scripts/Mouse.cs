using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundMask;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        if(!IsGround())
            Turn();
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -moveSpeed, rb.velocity.y);
    }

    private bool IsGround()
    {
        return Physics2D.Raycast(groundCheckPos.position, Vector2.down, 1f, groundMask);
    }
}
