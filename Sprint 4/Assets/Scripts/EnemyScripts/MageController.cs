using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    public GameObject player;
    private float playerX;
    private float playerY;
    private float mageX;
    private float mageY;

    public Rigidbody2D physics;
    public Animator animator;
    private bool facingLeft;
    private bool idle;

    private Vector2 vel;
    private float moveRate;
    private float moveCounter;
    private bool seesPlayer;
    private int dirX;

    public float attackRate;
    private float nextAttack;
    public GameObject attackPrefab;
    public Transform attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Grabbing needed components
        player = GameObject.FindGameObjectWithTag("Player");
        physics = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();

        vel = new Vector2(1f, 0);
        moveRate = 1f;
        moveCounter = 0f;
        seesPlayer = false;
        attackRate = 2f;

        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 13);
        Physics2D.IgnoreLayerCollision(13, 13, true);
    }//end Start()

    // Update is called once per frame
    void Update()
    {
        // While the mage doesn't see the player
        if (!seesPlayer)
        {
            // Update movement
            if (moveCounter > moveRate)
            {
                ChangeDirection();
                moveCounter = 0f;
            }

            physics.velocity = vel * dirX;

            moveCounter += Time.deltaTime;

            FindPlayer();
        }
        else
        {
            if (!idle) { idle = true; }
            SeekPlayer();

            if (Time.time > nextAttack)
            {
                animator.SetTrigger("Attacking");
                this.nextAttack = Time.time + this.attackRate;

                GameObject projectile = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);

                projectile.GetComponent<Projectile>().Fire(facingLeft, 'm');
            }
        }
    }//end Update()

    private void ChangeDirection()
    {
        // int [-1,2)
        dirX = Random.Range(-1, 2);

        switch (dirX)
        {
            case -1:
                idle = false;
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;
                break;

            case 0:
                idle = true;
                break;

            case 1:
                idle = false;
                transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;
                break;
        }
        animator.SetBool("Idle", idle);
    }//end ChangeDirection()

    private void FindPlayer()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;

        mageX = this.transform.position.x;
        mageY = this.transform.position.y;

        if (Mathf.Abs(playerY - mageY) < 2f)
        {
            if (!facingLeft && playerX > mageX)
            {
                if (playerX - mageX <= 5f) { seesPlayer = true; }
                return;
            }
            else if (facingLeft && playerX < mageX)
            {
                if (playerX + 5f > mageX) { seesPlayer = true; }
                return;
            }
        }
    }//end FindPlayer()

    private void SeekPlayer()
    {
        if (!animator.GetBool("Idle")) { animator.SetBool("Idle", idle); }
        playerX = player.transform.position.x;
        mageX = this.transform.position.x;

        if (playerX > mageX)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            this.facingLeft = false;
        }
        else if (playerX < mageX)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.facingLeft = true;
        }
    }//end SeekPlayer()
}