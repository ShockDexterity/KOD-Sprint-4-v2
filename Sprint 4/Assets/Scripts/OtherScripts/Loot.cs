using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private int value;      // Value of the coin

    // Start is called before the first frame update
    void Start()
    {
        // Are the first four letters 'coin'? If yes, then it has a value of one
        // otherwise it has a value of 5
        value = (this.gameObject.name.Substring(0, 4) == "coin") ? 1 : 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AcquireLoot(value);
                Destroy(this.gameObject);
                break;

            // The player didn't walk into it so do nothing
            default: break;
        }
    }
}
