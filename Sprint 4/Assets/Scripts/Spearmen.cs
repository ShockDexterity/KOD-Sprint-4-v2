using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearmen : MonoBehaviour
{
    public GameObject player;           // The player
    private float playerX;              // Player x coord
    private float spearmenX;              // Spearmen x coord
    private float playerY;              // Player y coord
    private float spearmenY;              // Spearmen y coord

    private Rigidbody2D physics;        // Holds the Spearmen's rigidbody for movement
    public bool facingLeft;             // Is the mage facing left? based on initial spritesheet
    private int health = 8;             // Health of the Spearmen

    public float attackRate;            // Time between attacks
    private float nextAttack;           // Time of next attack
    private int damage;                 // Damage of attack
    public Transform attackPoint;       // Center of attack range
    public float attackRange;           // Range of attack
    public LayerMask playerLayer;       // The layer the player is on

    private Vector2 speed;              // How fast the mage can go
    private float moveRate = 1f;        // How long the mage will move
    private float moveCounter = 0f;     // How long the mage has moved
    public int dirX;                    // Direction of movement
    public bool seesPlayer;             // Did the mage see the player?

    public Animator animator;           // Spearmen animation control
    public bool idle;                   // Is the Spearmen idle?

    public BoxCollider2D boxCollider2D;
    public Vector3 lootSpawnPoint;
    private float centerOfSpearmen;
    public GameObject lootPrefab;

    private bool alive;
    public float timeOfDeath;
    public float deathDelay;
    public bool lootDropped;

    // ALMOST IDENTICAL TO KNIGHT.CS, CHECK THERE FOR COMMENTS

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        centerOfSpearmen = boxCollider2D.size.y / 2f;

        // Enemies won't collide
        Physics2D.IgnoreLayerCollision(10, 10);

        // Setting defaults
        this.idle = true;
        this.damage = 2;
        this.facingLeft = false;
        this.attackRate = 2f;
        this.attackRange = 0.7f;
        this.seesPlayer = false;
        speed = new Vector2(1.5f, 0f);
        alive = true;
        lootDropped = false;

        // Grabbing components
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.physics = this.GetComponent<Rigidbody2D>();
        this.attackPoint = GameObject.Find("spearAttackPoint").GetComponent<Transform>();
        this.animator = this.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "enemyDeath") { deathDelay = clip.length; }
        }
    }//end Start()

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            // If the Spearmen doesn't see the player
            if (!this.seesPlayer)
            {
                // Update movement
                if (this.moveCounter > this.moveRate)
                {
                    this.ChangeDirection();
                    this.moveCounter = 0f;
                }

                // Which way is the Spearmen facing?
                switch (this.dirX)
                {
                    case 1:
                        this.idle = false;
                        animator.SetBool("isIdle", this.idle);
                        this.transform.localScale = Vector3.one;
                        this.facingLeft = false;
                        break;

                    case -1:
                        this.idle = false;
                        animator.SetBool("isIdle", this.idle);
                        this.transform.localScale = new Vector3(-1, 1, 1);
                        this.facingLeft = true;
                        break;

                    case 0:
                        this.idle = true;
                        animator.SetBool("isIdle", this.idle);
                        break;
                }

                // Move the Spearmen
                this.physics.velocity = speed * dirX;

                moveCounter += Time.deltaTime;

                // Look for player
                FindPlayer();
            }
            else // After the Spearmen sees the player
            {

                // Look for player and move towards them
                playerX = player.transform.position.x;
                spearmenX = this.transform.position.x;
                if (playerX > spearmenX)
                {
                    this.transform.localScale = Vector3.one;
                    this.facingLeft = false;
                    dirX = 1;
                }
                else if (playerX < spearmenX)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    this.facingLeft = true;
                    dirX = -1;
                }
                this.physics.velocity = speed * dirX;

                if (Time.time > nextAttack)
                {
                    nextAttack = Time.time + attackRate;
                    Attack();
                }
            }
        }
        else
        {
            if (Time.time > timeOfDeath + deathDelay)
            {
                Destroy(this.gameObject);
            }
        }
    }//end Update()

    // Randomly chooses movement direction
    private void ChangeDirection()
    {
        // (inclusive, exclusive)
        this.dirX = Random.Range(-1, 2);
    }

    // Look for player location
    private void FindPlayer()
    {
        this.playerX = player.transform.position.x;
        this.playerY = player.transform.position.y;
        this.spearmenX = this.transform.position.x;
        this.spearmenY = this.transform.position.y;

        if (Mathf.Abs(this.playerY - this.spearmenY) < 2)
        {
            if (this.playerX > this.spearmenX && !this.facingLeft)
            {
                if (this.playerX - this.spearmenX <= 5f)
                {
                    this.seesPlayer = true;
                    // No longer idle
                    animator.SetBool("isIdle", false);
                    return;
                }//end if
            }//end if
            else if (this.playerX < this.spearmenX && this.facingLeft)
            {
                if (Mathf.Abs(this.playerX - this.spearmenX) <= 5f)
                {
                    this.seesPlayer = true;
                    // No longer idle
                    animator.SetBool("isIdle", false);
                    return;
                }//end if
            }//end else if
        }//end if
    }//end FindPlayer()

    // Spearmen can take damage and die
    public void TakeDamage(int outsideDamage)
    {
        this.health -= outsideDamage;
        if (this.health < 1)
        {
            if (!lootDropped) { DropLoot(); }
            timeOfDeath = Time.time;
            alive = false;
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            animator.SetTrigger("Dead");
        }
    }

    private void DropLoot()
    {
        lootDropped = true;
        float xPos = this.transform.position.x;
        float yPos = this.transform.position.y;
        float zPos = this.transform.position.z;
        CircleCollider2D lootCollider = lootPrefab.GetComponent<CircleCollider2D>();
        float lootCenter = lootCollider.radius;

        Vector3 coinSpawnPoint = new Vector3(xPos, yPos - centerOfSpearmen + lootCenter, zPos);

        Instantiate(lootPrefab, coinSpawnPoint, Quaternion.identity);
    }

    // Spearmen attack control
    void Attack()
    {
        this.animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitEnemies)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }
}
