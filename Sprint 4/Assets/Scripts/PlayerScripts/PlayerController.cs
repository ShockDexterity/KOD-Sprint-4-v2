using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D physics;
    private float speed;
    private Vector2 jumpForce;
    private bool jumping;
    private float timeJumped;
    private float jumpDelay;
    private bool inJumpDelay;
    private int dirX;

    private Animator animator;
    private bool idle;
    public bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        physics = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        jumpDelay = 0.444f;

        speed = 2.5f;
        jumpForce = new Vector2(0, 8f);
        jumping = false;
        idle = true;
        inJumpDelay = false;
        facingLeft = false;

        Physics2D.IgnoreLayerCollision(11, 12);
        Physics2D.IgnoreLayerCollision(10, 12);
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumping)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                timeJumped = Time.time;
                idle = false;
                jumping = true;
                inJumpDelay = true;
                animator.SetBool("Jumping", jumping);
                this.GetComponent<PlayerBlock>().enabled = false;
            }
        }
        if (inJumpDelay && Time.time > (timeJumped + jumpDelay))
        {
            physics.AddForce(jumpForce, ForceMode2D.Impulse);
            inJumpDelay = false;
        }
        if (inJumpDelay) { physics.velocity = Vector2.zero; }
        else
        {
            dirX = (int)Input.GetAxisRaw("Horizontal");

            switch (dirX)
            {
                case 1:
                    facingLeft = false;
                    idle = false;
                    this.transform.localScale = new Vector3(1, 1, 1);
                    break;

                case 0:
                    idle = true;
                    break;

                case -1:
                    facingLeft = true;
                    idle = false;
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
            if (!jumping) { animator.SetBool("Idle", idle); }

            physics.velocity = new Vector2(speed * dirX, physics.velocity.y);
            animator.SetFloat("yVel", physics.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // No longer jumping
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumping = false;
            animator.SetBool("Jumping", jumping);
            this.GetComponent<PlayerBlock>().enabled = true;
        }//end if
    }//end OnCollisionEnter2D()
}