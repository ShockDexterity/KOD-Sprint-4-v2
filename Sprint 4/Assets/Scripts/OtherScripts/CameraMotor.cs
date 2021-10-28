using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        if (lookAt == null)
        {
            lookAt = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float deltaX = lookAt.position.x - transform.position.x;
        float deltaY = lookAt.position.y - transform.position.y;

        transform.position += new Vector3(deltaX, deltaY + 1f, 0);
    }
}
