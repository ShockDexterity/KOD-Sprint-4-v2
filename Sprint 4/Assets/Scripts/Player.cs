using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D physics;    // Allows for collisions with movement
    private float speed;            // Speed of player
    public float jumpForce;         // Force of player jump
    private bool jumping;           // Are they jumping?

    private int maxHealth = 50;     // max health
    private int health;             // current health

    private Animator animator;      // Animator of the player
    private bool idle;              // Is the player idle?
    public bool blocking;           // Is the player currently blocking?
    public bool facingLeft;

    private float timeJumped;       // When the player jumped
    private float jumpDelay;        // When the force will be applied
    private bool inJumpDelay;       // Are they waiting to jump?

    private bool alive;             // Is the player currently alive? I sure hope so
    private float timeOfDeath;      // When the player died

    public int totalLoot;
    public int coinCount;
    public int gemCount;

    // Allows other scripts to get the players direction
    public bool Direction()
    {
        return facingLeft;
    }

    // Start is called before the first frame update
    void Start()
    {
        physics = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                // Time jump force with animation
                case "playerJumpHold":
                    // I don't know why it has to be multiplied by 10, otherwise it's too short
                    jumpDelay = clip.length * 10f;
                    break;

                default: break;
            }
        }
        // Setting default values
        speed = 2.5f;
        jumpForce = 8f;
        jumping = false;
        idle = true;
        health = maxHealth;
        blocking = false;
        inJumpDelay = false;
        alive = true;
        timeOfDeath = 0;
        facingLeft = false;

        // The player is able to pass through what the enemies can't
        Physics2D.IgnoreLayerCollision(11, 12);
        //Physics2D.IgnoreLayerCollision(11, 10);
    }// end Start()

    // Update is called once per frame
    void Update()
    {
        if (timeOfDeath != 0 && Input.GetKeyDown(KeyCode.U) && Time.time > timeOfDeath + 10f)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        // Checking for pitfall trap
        // FIXME: UPDATE THIS FOR A COLLISION TRIGGER RATHER THAN A POSITION CHECK
        if (this.transform.position.y < -5f)
        {
            TakeDamage(1);
        }//end if

        if (alive)
        {
            if (!blocking)
            {
                // We don't want to double/triple/infinitely jump
                if (!jumping)
                {
                    // Multiple control options
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
                    {
                        timeJumped = Time.time;

                        idle = false;
                        jumping = true;
                        inJumpDelay = true;
                        animator.SetBool("isIdle", idle);
                        animator.SetBool("isJumping", jumping);
                    }//end if
                }//end if
                if (inJumpDelay && Time.time > (timeJumped + jumpDelay))
                {
                    // Player jumps on delay
                    physics.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    inJumpDelay = false;
                }
                // Player can move while in the air
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    // Don't want to change out of the jump animation
                    if (!jumping)
                    {
                        idle = false;
                        animator.SetBool("isIdle", this.idle);
                    }//end if

                    // Change player direction
                    this.transform.localScale = new Vector3(1, 1, 1);
                    facingLeft = true;

                    // Set new horizontal velocity while keeping vertical velocity
                    physics.velocity = new Vector2(speed, physics.velocity.y);
                    animator.SetFloat("yVel", physics.velocity.y);
                }//end if

                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    // Don't want to change out of the jump animation
                    if (!jumping)
                    {
                        idle = false;
                        animator.SetBool("isIdle", this.idle);
                    }//end if

                    // Change player direction
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    facingLeft = false;

                    // Set new horizontal velocity while keeping vertical velocity
                    physics.velocity = new Vector2(-speed, physics.velocity.y);
                    animator.SetFloat("yVel", physics.velocity.y);
                }//end else if

                else
                {
                    // Don't want to change from jumping animation
                    if (!jumping)
                    {
                        idle = true;
                        animator.SetBool("isIdle", this.idle);
                    }//end if
                }//end else
            }//end if blocking
        }//end if alive
        else if (timeOfDeath != 0f) //&& Time.time > timeOfDeath + 10f)
        {
            physics.velocity = Vector2.zero;
        }
    }//end Update()

    public void IsBlocking(bool b)
    {
        blocking = b;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // No longer jumping
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumping = false;
            animator.SetBool("isJumping", jumping);
        }//end if
    }//end OnCollisionEnter2D()

    // Player can take damage and die
    public void TakeDamage(int outsideDamage)
    {
        // Only take damage when not blocking
        if (!blocking)
        {
            health -= outsideDamage;
        }//end if
        else
        {
            Debug.Log("Damage blocked! Health is at " + health);
        }

        //FIXME: RESPAWN INSTEAD OF DESTROYING
        if (health < 1)
        {
            Die();
        }//end if
    }//end TakeDamage()

    public void AcquireLoot(int value)
    {
        if (value == 1)
        {
            coinCount += 1;
        }
        else
        {
            gemCount += 1;
        }
        totalLoot = coinCount + (5 * gemCount);

        Debug.Log("You acquired some loot! Your total loot is now: " + totalLoot);
    }

    public void Die()
    {
        physics.velocity = Vector2.zero;
        timeOfDeath = Time.time;

        // can't do anything anymore
        alive = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        animator.SetTrigger("Died");
    }
}//end Player