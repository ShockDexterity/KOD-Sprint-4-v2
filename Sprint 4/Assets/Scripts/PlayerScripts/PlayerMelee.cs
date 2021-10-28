using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Animator animator;
    public PlayerController playerController;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float attackRate = 0.6f;
    private float nextAttack;
    private float attackRange = 0.5f;
    private int damage;

    private float timeAttacked;
    private float attackDelay;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { animator = this.GetComponent<Animator>(); }
        if (playerController == null) { playerController = this.GetComponent<PlayerController>(); }

        damage = 2;

        attackDelay = 0.5f;
    }//end Start()

    // Update is called once per frame
    void Update()
    {
        // Check if user can attack
        if (!animator.GetBool("Jumping") && Input.GetMouseButtonDown(0) && (Time.time > nextAttack))
        {
            nextAttack = Time.time + attackRate;
            Attack();
            timeAttacked = Time.time;
        }
        if (!playerController.enabled && Time.time > (timeAttacked + attackDelay))
        {
            playerController.enabled = true;
        }
    }//end Update()

    // Attack control
    void Attack()
    {
        playerController.enabled = false;

        // Set melee animation
        animator.SetTrigger("MeleeAttack");

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
    }//end Attack()

    // Allows the range to be seen
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    // }//end OnDrawGizmosSelected()
}