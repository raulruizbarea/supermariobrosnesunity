using System;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "floor")
        {
            if(collision.collider.tag != "mario")
            {
                try { 
                    Destroy(gameObject);
                    mario.countBalls--;
                } catch (NullReferenceException ex)
                {
                    if(mario.countBalls > 0)
                    {
                        mario.countBalls--;
                    }
                    print("No fireballs");
                }
            }
        }

        if (collision.collider.tag == "koopa" || collision.collider.tag == "shell" || collision.collider.tag == "goomba")
        {
            Destroy(collision.gameObject);
        }
    }
}
