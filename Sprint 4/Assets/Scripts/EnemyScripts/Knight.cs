using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public GameObject coinPrefab;
    public Animator animator;
    public BoxCollider2D boxCollider2D;
    public Vector3 coinSpawnPoint;
    private float centerOfKnight;
    private int health;
    private bool alive;
    public float timeOfDeath;
    public float deathDelay;
    public bool lootDropped;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        centerOfKnight = boxCollider2D.size.y / 2f;

        deathDelay = 0.889f;

        health = 8;
        alive = true;
        lootDropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
        {
            this.GetComponent<KnightController>().enabled = false;
        }
        if (!alive && Time.time > timeOfDeath + deathDelay)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int incomingDamage)
    {
        if (alive) { health -= incomingDamage; }

        if (health < 1)
        {
            alive = false;
            timeOfDeath = Time.time;
            if (!lootDropped) { DropLoot(); }
            //this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //this.GetComponent<BoxCollider2D>().isTrigger = true;
            animator.SetTrigger("Dead");
        }
    }

    private void DropLoot()
    {
        lootDropped = true;
        CircleCollider2D coinCollider = coinPrefab.GetComponent<CircleCollider2D>();
        float coinCenter = coinCollider.radius;

        float xPos = this.transform.position.x;
        float yPos = this.transform.position.y - centerOfKnight + coinCenter;
        float zPos = this.transform.position.z;

        coinSpawnPoint = new Vector3(xPos, yPos, zPos);

        Instantiate(coinPrefab, coinSpawnPoint, Quaternion.identity);
    }
}