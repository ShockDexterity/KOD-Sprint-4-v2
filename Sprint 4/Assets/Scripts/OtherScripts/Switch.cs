using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Sprite off, on;
    public bool toggleable;
    public bool toggled;

    // Start is called before the first frame update
    void Start()
    {
        toggleable = false;
        toggled = false;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = off;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleable && !toggled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = on;
                toggled = true;
                GameObject.FindGameObjectWithTag("Gate").GetComponent<GateControl>().Open();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // It hit the player and deals damage
            case "Player":
                toggleable = true;
                break;

            // The player didn't walk into it so do nothing
            default: break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        toggleable = false;
    }
}
