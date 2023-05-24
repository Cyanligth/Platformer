using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected new Rigidbody2D rigidbody;
    // protected Animator animator;
    protected new Collider2D collider;
    protected new SpriteRenderer renderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }
    /*
    protected virtual void Die()
    {
        rigidbody.gravityScale = 1.0f;
        rigidbody.velocity = Vector2.up * 3;
        // animator.SetBool("Die", true);
        collider.enabled = false;

        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // TODO : 임시로 구현
            Die();
        }
    }
    */
}
