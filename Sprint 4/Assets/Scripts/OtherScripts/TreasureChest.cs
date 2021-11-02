using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private Animator animator;          // Chest animation control
    private float timeLooted;           // When the chest was opened
    private float lootDelay;            // How long the player has to wait to loot it
    private float timeLootable;         // When the player can loot the chest
    private bool opened;                // Has the chest been opened?
    private bool looted;                // Has the chest been looted?
    private bool lootable;              // Can the player loot the chest?

    public GameObject[] possibleLoot;   // All possible loot options
    private GameObject loot;            // What the chest will drop
    private BoxCollider2D chestBox;
    private BoxCollider2D lootBox;
    private float centerOfChest;
    private float centerOfLoot;
    private Vector3 lootSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        loot = possibleLoot[Random.Range(0, 4)];
        chestBox = this.GetComponent<BoxCollider2D>();
        lootBox = loot.GetComponent<BoxCollider2D>();

        centerOfChest = chestBox.size.y / 2f;
        centerOfLoot = lootBox.size.y / 2f;
        lootSpawnPoint = new Vector3(this.transform.position.x, this.transform.position.y - centerOfChest + centerOfLoot, this.transform.position.z);

        animator = this.GetComponent<Animator>();

        lootDelay = 1.017f;

        lootable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!opened && lootable && !looted && Input.GetKeyDown(KeyCode.E))
        {
            opened = true;
            animator.SetTrigger("Opened");
            timeLootable = Time.time + lootDelay;
        }

        if (lootable && !looted && (Time.time > timeLootable) && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Looted");
            looted = true;
            Instantiate(loot, lootSpawnPoint, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // It hit the player and deals damage
            case "Player":
                lootable = true;
                break;

            // The player didn't walk into it so do nothing
            default: break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        lootable = false;
    }
}
