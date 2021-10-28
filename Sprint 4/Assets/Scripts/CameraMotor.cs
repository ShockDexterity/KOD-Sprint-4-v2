using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;

    private void Start()
    {
        // if we forget to set it ahead of time
        if (lookAt == null)
        {
            lookAt = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void LateUpdate()
    {
        // telling the camera to look at the player
        float deltaX = lookAt.position.x - transform.position.x;
        float deltaY = lookAt.position.y - transform.position.y;

        transform.position += new Vector3(deltaX, deltaY + 1f, 0);
    }
}
