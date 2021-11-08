using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    public bool canEnter;
    private Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        canEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            switch (scene.name)
            {
                case "Level_1":
                    SceneManager.LoadScene("Level_2");
                    break;

                case "Level_2":
                    SceneManager.LoadScene("Level_3");
                    break;

                case "Level_3":
                    SceneManager.LoadScene("YouWin");
                    break;
            }
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
