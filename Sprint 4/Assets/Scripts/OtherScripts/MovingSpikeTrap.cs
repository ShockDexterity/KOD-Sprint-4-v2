using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikeTrap : MonoBehaviour
{
    private float lastTrigger;
    private int damage;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); }
        lastTrigger = -2f;
        damage = 4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time >= lastTrigger + 2f)
        {
            lastTrigger = Time.time;
            collision.GetComponent<Player>().TakeDamage(damage);
            Debug.Log(";)");
        }
    }
}
