using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Follow Mario
    public Transform follow;
    // How is going to follow the camera
    public float suavity;
    // Velocity
    private float velocityX = 0;

    // Limits and camera position
    public float leftLimit;
    public float rightLimit;
    private float newPositionX;
    private float oldPositionX;
    
    void Start()
    {
        // Old position 0
        oldPositionX = 0;
    }

    void Update()
    {
        // Check camera position with objective (mario)
        newPositionX = Mathf.SmoothDamp(transform.position.x, follow.position.x, ref velocityX, suavity);

        // If the new position is bigger than the old one - to avoid going back camera
        if (newPositionX > oldPositionX)
        {
            // The position is fixed in the limits
            if (newPositionX > rightLimit || newPositionX < leftLimit)
            {
                newPositionX = transform.position.x;
            }
            // I save the old position to compare it with the new one
            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
            oldPositionX = newPositionX;
        }
    }
}
