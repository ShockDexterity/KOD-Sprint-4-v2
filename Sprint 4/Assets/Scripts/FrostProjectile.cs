using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostProjectile : MonoBehaviour
{
    public float moveSpeed = 0f;        // Movespeed of projectile
    public int damage;                  // Damage of projectile

    public Vector3 initialPos;
    public int enemiesHit;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        this.damage = 1;
        enemiesHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        if (Mathf.Abs(transform.position.x) > 15 + Mathf.Abs(initialPos.x))
        {
            // Went out of bounds
            Destroy(this.gameObject);
        }
    }

    // What direction the projectile will move in
    public void Direction(bool dir)
    {
        moveSpeed = (dir) ? -3f : 3f;
        if (dir)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // Damage the enemy it hits
            case "Enemy":
                Mage mage = collision.gameObject.GetComponent<Mage>();
                Knight knight = collision.gameObject.GetComponent<Knight>();
                Spearmen spearmen = collision.gameObject.GetComponent<Spearmen>();
                if (mage != null) { mage.TakeDamage(damage); }
                else if (knight != null) { knight.TakeDamage(damage); }
                else if (spearmen != null) { spearmen.TakeDamage(damage); }
                enemiesHit++;
                break;

            // Do nothing
            case "Player":
                break;

            case "AttackPass":
                break;

            // It hit some other collider, so it can be destroyed
            default:
                Destroy(this.gameObject);
                break;
        }
    }
}
