using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public GameObject player;               // The player
    private float playerX;                  // Player x coord
    private float mageX;                    // Mage x coord
    private float playerY;                  // Player x coord
    private float mageY;                    // Mage x coord

    private Rigidbody2D physics;            // Holds the mage's rigidbody for movement
    public bool facingLeft = true;          // Is the mage facing left? based on initial spritesheet
    public int health = 4;                  // Health of the mage

    public GameObject attackPrefab;         // Holds the prefab for the mage's attack
    public float attackRate;                // Time between attacks
    private float nextAttack;               // Time of next attack

    private Vector2 speed;                  // How fast the mage can go
    private float moveRate = 1f;            // How long the mage will move
    private float moveCounter = 0f;         // How long the mage has moved
    public int dirX;                        // Direction of movement
    public bool seesPlayer = false;         // Did the mage see the player?

    // Animation booleans
    public Animator anim;                   // Mage animation control
    public bool idle;
    public bool attacking;

    public BoxCollider2D boxCollider2D;
    public Vector3 lootSpawnPoint;
    private float centerOfMage;
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
        centerOfMage = boxCollider2D.size.y / 2f;

        Physics2D.IgnoreLayerCollision(11, 11);

        player = GameObject.FindGameObjectWithTag("Player");

        this.idle = true;
        this.attacking = false;

        attackRate = 2f;
        this.nextAttack = 0f;

        // grabbing the rigidbody component for movement
        this.physics = this.gameObject.GetComponent<Rigidbody2D>();
        this.anim = this.gameObject.GetComponent<Animator>();
        this.speed = new Vector2(1.25f, 0f);
        alive = true;
        lootDropped = false;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "enemyDeath") { deathDelay = clip.length; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (!this.seesPlayer)
            {
                if (this.moveCounter > this.moveRate)
                {
                    this.ChangeDirection();
                    this.moveCounter = 0f;
                }

                switch (this.dirX)
                {
                    case 1:
                        this.idle = false;
                        anim.SetBool("isIdle", this.idle);
                        this.transform.localScale = new Vector3(-1, 1, 1);
                        this.facingLeft = false;
                        break;

                    case -1:
                        this.idle = false;
                        anim.SetBool("isIdle", this.idle);
                        this.transform.localScale = Vector3.one;
                        this.facingLeft = true;
                        break;

                    case 0:
                        this.idle = true;
                        anim.SetBool("isIdle", this.idle);
                        break;
                }

                if (Mathf.Abs(this.physics.velocity.x) < speed.x)
                {
                    this.physics.AddForce(speed * dirX);
                }
                else
                {
                    this.physics.velocity = speed * dirX;
                }

                moveCounter += Time.deltaTime;

                FindPlayer();
            }
            else
            {
                this.idle = true;
                anim.SetBool("isIdle", this.idle);

                try
                {
                    playerX = player.transform.position.x;
                    this.mageX = this.transform.position.x;

                    if (playerX > mageX)
                    {
                        this.transform.localScale = new Vector3(-1, 1, 1);
                        this.facingLeft = false;
                    }
                    else if (playerX < mageX)
                    {
                        this.transform.localScale = Vector3.one;
                        this.facingLeft = true;
                    }
                }
                catch { }

                if (Time.time > nextAttack)
                {
                    this.attacking = true;
                    this.anim.SetBool("isAttacking", this.attacking);
                    this.nextAttack = Time.time + this.attackRate;

                    float xOffset = (!facingLeft) ? 0.3f : -0.3f;

                    GameObject projectile = Instantiate(attackPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + .25f, 0), Quaternion.identity);

                    projectile.GetComponent<MageProjectile>().Direction(facingLeft);
                }
                else
                {
                    this.attacking = false;
                    this.anim.SetBool("isAttacking", this.attacking);
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
    }

    // Randomly generates the direction of movement
    private void ChangeDirection()
    {
        this.dirX = Random.Range(-1, 2);
    }

    // Looks for the player
    private void FindPlayer()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        mageX = this.transform.position.x;
        mageY = this.transform.position.y;

        if (Mathf.Abs(playerY - mageY) < 2)
        {
            if (playerX > mageX && !this.facingLeft)
            {
                if (playerX - mageX <= 5f)
                {
                    seesPlayer = true;
                    return;
                }
            }
            else if (playerX < mageX && facingLeft)
            {
                if (Mathf.Abs(playerX - mageX) <= 5f)
                {
                    seesPlayer = true;
                    return;
                }
            }
        }
    }

    // Allows the mage to take damage and die
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
            anim.SetTrigger("Dead");
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

        Vector3 coinSpawnPoint = new Vector3(xPos, yPos - centerOfMage + lootCenter, zPos);

        Instantiate(lootPrefab, coinSpawnPoint, Quaternion.identity);
    }
}
