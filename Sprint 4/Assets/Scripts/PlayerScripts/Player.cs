using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Animator animator;
    public HealthController healthController;

    private int maxHealth = 16;
    public int health;

    public bool alive;
    private float timeOfDeath;

    private int totalLoot;
    private int coinCount;
    private int gemCount;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { animator = this.GetComponent<Animator>(); }

        health = maxHealth;
        alive = true;

        timeOfDeath = 0f;
        totalLoot = coinCount = gemCount = 0;
        healthController = GameObject.Find("healthIcons").GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeOfDeath != 0f && Input.GetKeyDown(KeyCode.U) && Time.time >= (timeOfDeath + 5f))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void AcquireLoot(int value)
    {
        if (value == 1) { coinCount += 1; }
        else { gemCount += 1; }

        totalLoot += value;

        Debug.Log("You acquired some loot! Your total loot is now: " + totalLoot);
    }

    public void Heal(int pot)
    {
        if ((health + pot) > maxHealth) { health = maxHealth; }
        else { health += pot; }
    }

    public void TakeDamage(int incomingDamage)
    {
        if (!this.GetComponent<PlayerBlock>().blocking)
        {
            health -= incomingDamage;
            healthController.UpdateHealth((health >= 0) ? health : 0);
            Debug.Log(health);
            Debug.Log("ouch");
        }

        if (health < 1)
        {
            this.Die();
        }
    }

    public void Die()
    {
        timeOfDeath = Time.time;
        alive = false;

        this.GetComponent<PlayerBlock>().enabled = false;

        this.gameObject.GetComponent<PlayerController>().enabled = false;

        this.GetComponent<PlayerMelee>().enabled = false;

        this.GetComponent<PlayerFireBreath>().enabled = false;

        this.GetComponent<PlayerFrostAttack>().enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        animator.SetTrigger("Died");
    }
}
