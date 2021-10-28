using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public bool canEnter;

    // Start is called before the first frame update
    void Start()
    {
        canEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("YouWin");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                canEnter = true;
                break;

            // Do nothing
            default: break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canEnter = false;
    }
}
