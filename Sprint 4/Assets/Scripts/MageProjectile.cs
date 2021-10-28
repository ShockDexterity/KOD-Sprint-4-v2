using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    public float moveSpeed = 0f;        // Movespeed of projectile
    public int damage;                  // Damage of projectile

    public Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        this.damage = 1;
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
        moveSpeed = (dir) ? 3f : -3f;
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
            // Do nothing
            case "Enemy": break;

            case "NoEnemy": break;

            // It hit the player and deals damage
            case "Player":
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
                Destroy(this.gameObject);
                break;

            // It hit some other collider, so it can be destroyed
            default:
                Destroy(this.gameObject);
                break;
        }
    }
}