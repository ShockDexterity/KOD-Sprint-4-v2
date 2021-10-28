using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;
    public int damage;

    public Vector3 initialPos;
    public int enemiesHit;

    private char id;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.position;
        enemiesHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        if (Mathf.Abs(transform.position.x) > 15 + Mathf.Abs(initialPos.x))
        {
            Destroy(this.gameObject);
        }
        if (id == 'f' && enemiesHit >= 3)
        {
            Destroy(this.gameObject);
        }
    }

    public void Fire(bool left, char id)
    {
        this.id = id;
        moveSpeed = (left) ? -3f : 3f;

        switch (this.id)
        {
            case 'f':
                damage = 10;
                break;

            case 'm':
                damage = 2;
                break;

            default:
                damage = 0;
                break;
        }

        if ((left && this.id == 'f') || (!left && this.id == 'm'))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (id == 'f')
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
                case "Player": break;

                case "NoEnemy": break;

                // It hit some other collider, so it can be destroyed
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
        else if (id == 'm')
        {
            switch (collision.gameObject.tag)
            {
                // Do nothing
                case "Enemy": break;

                case "NoEnemy": break;

                // It hit the player and deals damage
                case "Player":
                    collision.GetComponent<Player>().TakeDamage(damage);
                    Destroy(this.gameObject);
                    break;

                // It hit some other collider, so it can be destroyed
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}