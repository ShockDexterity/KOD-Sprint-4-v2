using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform r_player;
    public Transform r_init_arrow;
    public Rigidbody2D physics;
    private int damage;

    private float timeFired;

    private float dt;

    private float v_x;
    private float dx;

    private float dti_y;
    private float dtf_y;
    private float vi_y;
    private float dy;
    private float dy_i;

    // Start is called before the first frame update
    void Start()
    {
        physics = this.GetComponent<Rigidbody2D>();

        damage = 1;

        v_x = 2f;

        dx = r_player.position.x - r_init_arrow.position.x;
        dy = r_player.position.y - r_init_arrow.position.y;

        dtf_y = Mathf.Sqrt(1f / 4.9f);

        dy_i = 1f + dy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeFired + dt)
        {
            Destroy(this.gameObject);
        }
    }

    public void Fire(bool left)
    {
        timeFired = Time.time;
        dt = dx / v_x;
        dti_y = dt - dtf_y;

        v_x *= (left) ? -1 : 1;

        if (dy < 0f)
        {
            vi_y = (dy / dt) - (4.9f * dt);
        }
        else
        {
            vi_y = (2 * dy_i) / (dti_y);
        }
        Vector2 vel = new Vector2(v_x, vi_y);

        physics.AddForce(vel, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                Destroy(this.gameObject);
                break;

            default: break;
        }
    }
}
