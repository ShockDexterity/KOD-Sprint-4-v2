using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 player;
    private Vector3 initialPosition;
    private int damage;

    private float speed;

    private float timeFired;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        damage = 1;

        speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeFired + 2f) { Destroy(this.gameObject); }
        this.transform.position = Vector2.MoveTowards(this.transform.position, player, speed * Time.deltaTime);
        // if (Time.time > timeFired + 0.1f)
        // {
        //     if (Mathf.Abs(this.transform.position.x - player.x) < 0.001f)
        //     {
        //         if (Mathf.Abs(this.transform.position.y - player.y) < 0.01f)
        //         {
        //             Destroy(this.gameObject);
        //         }
        //     }
        // }
    }

    public void Fire()
    {
        timeFired = Time.time;
        initialPosition = this.transform.position;
        if (player.x > initialPosition.x) { transform.localScale = new Vector3(-1, 1, 1); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "AttackAllowed": break;
            case "NoEnemy": break;
            case "Enemy": break;

            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(damage); }
                Destroy(this.gameObject);
                break;

            default:
                Destroy(this.gameObject);
                break;
        }
    }
}
