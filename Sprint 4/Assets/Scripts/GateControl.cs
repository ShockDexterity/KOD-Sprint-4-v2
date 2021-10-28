using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    public Animator animator;
    public float timeOpened;
    public float enterDelay;
    public bool opened;
    public bool canEnter;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();

        enterDelay = 0f;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "gateOpening":
                    enterDelay += clip.length;
                    break;

                case "gateOpening1":
                    enterDelay += clip.length;
                    break;

                default: break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (opened) { animator.SetTrigger("Opened"); }

        if (opened && Time.time > timeOpened + enterDelay)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (opened && Time.time > (timeOpened + enterDelay))
        {
            Destroy(this.gameObject);
        }
    }

    public void Open()
    {
        opened = true;
        timeOpened = Time.time;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     switch (collision.gameObject.tag)
    //     {
    //         case "Player":
    //             if (opened) { canEnter = true; }
    //             break;

    //         // Do nothing
    //         default: break;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     canEnter = false;
    // }
}
