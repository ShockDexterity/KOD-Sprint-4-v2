using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lectern : MonoBehaviour
{
    public Animator animator;
    public bool useable;
    public bool used;
    public float timeUsed;
    public float abilityDelay;

    // Start is called before the first frame update
    void Start()
    {
        useable = used = false;
        animator = this.gameObject.GetComponent<Animator>();
        abilityDelay = timeUsed = 0f;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "lecternUsed":
                    abilityDelay = clip.length;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!used && useable && Input.GetKeyDown(KeyCode.E))
        {
            timeUsed = Time.time;
            used = true;
            animator.SetTrigger("Used");
        }

        if (used && Time.time > (timeUsed + abilityDelay))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<playerFrostAttack>().GrantAbility();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // It hit the player and deals damage
            case "Player":
                useable = true;
                break;

            // The player didn't walk into it so do nothing
            default: break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        useable = false;
    }
}