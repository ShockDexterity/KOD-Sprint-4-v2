using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningstarAttack : MonoBehaviour
{
    public Animator animator;
    public MorningstarController morningstarController;
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
        morningstarController = this.GetComponent<MorningstarController>();

        attackDelay = 1.273f;

        damage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!morningstarController.enabled && Time.time >= timeAttacked + attackDelay)
        {
            morningstarController.enabled = true;
        }
    }

    public void Attack()
    {
        morningstarController.enabled = false;
        timeAttacked = Time.time;
        animator.SetTrigger("Attacking");
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        if (playersHit.Length > 0)
        {
            playersHit[0].GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}