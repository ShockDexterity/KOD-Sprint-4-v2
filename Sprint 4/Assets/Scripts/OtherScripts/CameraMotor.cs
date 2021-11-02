using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    private bool onScreen;

    // Start is called before the first frame update
    void Start()
    {
        onScreen = true;
        if (lookAt == null)
        {
            lookAt = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (onScreen)
        {
            float deltaX = lookAt.position.x - transform.position.x;
            float deltaY = lookAt.position.y - transform.position.y;

            transform.position += new Vector3(deltaX, deltaY + 1f, 0);
        }
    }

    public void PlayerFell()
    {
        onScreen = false;
    }
}
