using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFrostAttack : MonoBehaviour
{
    public GameObject frostAttack;
    public Animator animator;
    public Transform attackPoint;
    public float attackRate;
    private float nextAttack;

    private bool attacking;
    private float timeAttackStart;

    public bool granted;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { animator = this.GetComponent<Animator>(); }
        attackRate = 10f;

        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "Level_3":
                granted = true;
                break;

            default:
                granted = false;
                this.enabled = false;
                break;
        }

        Physics2D.IgnoreLayerCollision(11, 12);
        Physics2D.IgnoreLayerCollision(10, 12);
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("Jumping") && Input.GetKeyDown(KeyCode.G) && Time.time > nextAttack)
        {
            float t = Time.time;
            nextAttack = t + attackRate;
            timeAttackStart = t;
            attacking = true;
            animator.SetTrigger("FrostAttack");
        }

        if (attacking && Time.time > timeAttackStart + 0.4f)
        {
            attacking = false;
            Attack();
        }
    }

    private void Attack()
    {
        bool facingLeft = this.GetComponent<PlayerController>().facingLeft;
        GameObject proj = Instantiate(frostAttack, attackPoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Fire(facingLeft, 'f');
    }
}