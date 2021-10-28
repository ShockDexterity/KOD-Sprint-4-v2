using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public Player player;       // The player script
    public Animator animator;   // The player's animator
    public bool blocking;       // Is the player blocking?

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!blocking) { blocking = true; }
            animator.SetBool("isBlocking", blocking);
        }
        else
        {
            if (blocking) { blocking = false; }
            animator.SetBool("isBlocking", blocking);
        }
        player.IsBlocking(blocking);
    }
}
