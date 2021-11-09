using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public GameObject player;
    private float playerX;
    private float playerY;
    private float archerX;
    private float archerY;

    public Rigidbody2D physics;
    public Animator animator;
    private bool facingLeft;
    private bool idle;

    private Vector2 vel;
    private float moveRate;
    private float moveCounter;
    private bool seesPlayer;
    private int dirX;

    private float attackRate;
    private float nextAttack;

    public GameObject attackPrefab;
    public Transform attackPoint;
    private bool inAttack;
    private float timeFired;
    private float attackDelay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        physics = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        vel = new Vector2(1f, 0);
        moveRate = 1f;
        moveCounter = 0f;
        seesPlayer = false;
        attackRate = 2f;
        inAttack = false;
        attackDelay = 1.167f;

        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 13);
        Physics2D.IgnoreLayerCollision(13, 13, true);
    }//end Start()

    // Update is called once per frame
    void Update()
    {
        if (!seesPlayer)
        {
            if (moveCounter > moveRate)
            {
                ChangeDirection();
                moveCounter = 0f;
            }
            physics.velocity = new Vector2(vel.x * dirX, physics.velocity.y);
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
                nextAttack = Time.time + attackRate;
                timeFired = Time.time;
                inAttack = true;
            }
            if (inAttack && Time.time > timeFired + attackDelay)
            {
                inAttack = false;
                GameObject arrow = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().Fire();
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

        archerX = this.transform.position.x;
        archerY = this.transform.position.y;

        if (Mathf.Abs(playerY - archerY) < 2f)
        {
            if (!facingLeft && playerX > archerX)
            {
                if (playerX - archerX <= 5f) { seesPlayer = true; }
                return;
            }
            else if (facingLeft && playerX < archerX)
            {
                if (playerX + 5f > archerX) { seesPlayer = true; }
                return;
            }
        }
    }//end FindPlayer()

    private void SeekPlayer()
    {
        if (!animator.GetBool("Idle")) { animator.SetBool("Idle", true); }
        playerX = player.transform.position.x;
        archerX = this.transform.position.x;

        if (playerX > archerX)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            this.facingLeft = false;
        }
        else if (playerX < archerX)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.facingLeft = true;
        }
    }//end SeekPlayer()
}