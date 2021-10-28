using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    public Animator animator;
    public KnightController knightController;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public float attackRange;
    private float attackDelay;
    private float timeAttacked;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        knightController = this.GetComponent<KnightController>();

        attackDelay = 1.017f;

        damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!knightController.enabled && Time.time >= timeAttacked + attackDelay)
        {
            knightController.enabled = true;
        }
    }

    public void Attack()
    {
        knightController.enabled = false;
        timeAttacked = Time.time;
        animator.SetTrigger("Attacking");
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in playersHit)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}