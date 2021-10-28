using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBreath : MonoBehaviour
{
    public PlayerController playerController;
    public SpriteRenderer sr;           // Fire Breath rendering control
    public Animator animator;           // Player animation control
    private float attackRate = 10f;     // Time between attacks
    private float nextAttack;           // Time of next attack
    private int damage = 10;             // Melee attack damage
    public Transform attackPoint;       // Where the attack range is centered
    private float attackRange = 0.5f;    // Range of the attack
    public LayerMask enemyLayers;       // What layer(s) the enemies are on

    private bool attacking;             // Is the player attacking?
    private float timeAttackStart;      // The time the attack started

    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null) { playerController = this.GetComponent<PlayerController>(); }

        attackRate = 10f;
        attackRange = 0.5f;

        // Grabbing player animator
        if (animator == null) { animator = this.GetComponent<Animator>(); }

        // Grabbing firebreath sprite renderer to toggle visibility
        if (sr == null) { sr = GameObject.Find("fireBreath").GetComponent<SpriteRenderer>(); }
        this.sr.enabled = false;
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Check if player can attack
        if (!animator.GetBool("Jumping") && Input.GetKeyDown(KeyCode.F) && (Time.time > nextAttack))
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
            attacking = false;
            this.sr.enabled = true;
        }
        // Hide the flames after the attack is finished
        if (this.sr.enabled && Time.time > (timeAttackStart + 0.9f))
        {
            this.sr.enabled = false;
            playerController.enabled = true;
        }
    }//end Update()

    // Attack control
    void Attack()
    {
        playerController.enabled = false;
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
        }
    }//end Attack

    // Allows the range to be seen
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }//end OnDrawGizmosSelected()
}
