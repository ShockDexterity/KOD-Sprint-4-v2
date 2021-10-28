using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFrostAttack : MonoBehaviour
{
    public GameObject frostAttack;
    public Animator animator;           // Player animation control
    public float attackRate;            // Time between attacks
    private float nextAttack;           // Time of next attack
    public Transform attackPoint;       // Where the attack range is centered

    private bool attacking;             // Is the player attacking?
    private float timeAttackStart;      // The time the attack started

    public bool useable;                // Can the player even use the frost attack?

    // Start is called before the first frame update
    void Start()
    {
        useable = false;
        attackRate = 10f;
        // Grabbing player animator
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        if (useable)
        {
            // Check if player can attack
            if (!animator.GetBool("isJumping") && Input.GetKeyDown(KeyCode.G) && (Time.time > nextAttack))
            {
                nextAttack = Time.time + attackRate;
                timeAttackStart = Time.time;
                attacking = true;
                // Set frost attack animation
                this.animator.SetTrigger("FrostAttack");
            }

            // Launch frost projectile
            if (attacking && Time.time > (timeAttackStart + 0.4f))
            {
                Attack();
                attacking = false;
            }
        }
    }//end Update()

    // Attack control
    private void Attack()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject proj = Instantiate(frostAttack, attackPoint.position, Quaternion.identity);
        proj.GetComponent<FrostProjectile>().Direction(player.Direction());
    }

    public void GrantAbility()
    {
        useable = true;
    }
}