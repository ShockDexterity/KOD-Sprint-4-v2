using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAttack : MonoBehaviour
{
    public SpriteRenderer sr;           // Fire Breath rendering control
    public Animator animator;           // Player animation control
    public float attackRate = 10f;     // Time between attacks
    private float nextAttack;           // Time of next attack
    private int damage = 10;             // Melee attack damage
    public Transform attackPoint;       // Where the attack range is centered
    public float attackRange = 0.5f;    // Range of the attack
    public LayerMask enemyLayers;       // What layer(s) the enemies are on

    private bool attacking;             // Is the player attacking?
    private float timeAttackStart;      // The time the attack started

    // Start is called before the first frame update
    void Start()
    {
        attackRate = 10f;

        // Grabbing player animator
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        // Grabbing firebreath sprite renderer to toggle visibility
        sr = GameObject.Find("fireBreath").GetComponent<SpriteRenderer>();
        this.sr.enabled = false;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Check if player can attack
        if (!animator.GetBool("isJumping") && Input.GetKeyDown(KeyCode.F) && (Time.time > nextAttack))
        {
            nextAttack = Time.time + attackRate;
            timeAttackStart = Time.time;
            attacking = true;
            // Set fire attack animation
            this.animator.SetTrigger("FireAttack");
        }

        // Make the flames visible
        if (attacking && Time.time > (timeAttackStart + 0.4f))
        {
            Attack();
            this.sr.enabled = true;
        }
        // Hide the flames after the attack is finished
        if (this.sr.enabled && Time.time > (timeAttackStart + 0.9f))
        {
            this.sr.enabled = false;
            attacking = false;
        }
    }//end Update()

    // Attack control
    void Attack()
    {
        // Gathering all enemies we hit, if any
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Checking who we hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Mage mage = enemy.GetComponent<Mage>();
            Knight knight = enemy.GetComponent<Knight>();
            Spearmen spearmen = enemy.GetComponent<Spearmen>();

            if (mage != null)
            {
                mage.TakeDamage(damage);
            }
            else if (knight != null)
            {
                knight.TakeDamage(damage);
            }
            else if (spearmen != null)
            {
                spearmen.TakeDamage(damage);
            }
        }//end foreach
    }//end Attack

    // Allows the range to be seen
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }//end OnDrawGizmosSelected()
}//end PlayerFireAttack