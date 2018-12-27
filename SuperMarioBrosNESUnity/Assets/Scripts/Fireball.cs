using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Movement mario;
    Rigidbody2D rb;

    float velocity;

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario").GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();

        if (mario.lookRight)
        {
            velocity = 4.5f;
        }
        else
        {
            velocity = -4.5f;
        }

        rb.velocity = new Vector2(velocity, 0);
    }

}
