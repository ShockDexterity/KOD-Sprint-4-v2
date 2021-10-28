using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmenAttack : MonoBehaviour
{
    public Animator animator;
    public SpearmenController spearmenController;
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
        spearmenController = this.GetComponent<SpearmenController>();

        attackDelay = 0.817f;

        damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spearmenController.enabled && Time.time >= timeAttacked + attackDelay)
        {
            spearmenController.enabled = true;
        }
    }

    public void Attack()
    {
        spearmenController.enabled = false;
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
