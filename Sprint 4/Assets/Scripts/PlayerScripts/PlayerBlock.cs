using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public Animator animator;
    public bool blocking;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { animator = this.GetComponent<Animator>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            blocking = true;
            animator.SetBool("Blocking", blocking);

            this.GetComponent<PlayerController>().enabled = false;
            this.GetComponent<PlayerMelee>().enabled = false;
            this.GetComponent<PlayerFireBreath>().enabled = false;
            this.GetComponent<PlayerFrostAttack>().enabled = false;
        }
        else
        {
            blocking = false;
            animator.SetBool("Blocking", blocking);

            this.GetComponent<PlayerController>().enabled = true;
            this.GetComponent<PlayerMelee>().enabled = true;
            this.GetComponent<PlayerFireBreath>().enabled = true;
            this.GetComponent<PlayerFrostAttack>().enabled = true;
        }
    }
}
