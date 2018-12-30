using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Movement mario;
    Rigidbody2D rb;

    float velocity;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario").GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();

        if (mario.lookRight)
        {
            velocity = 8f;
        }
        else
        {
            velocity = -8f;
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
                    if(mario != null) { 
                        if (mario.countBalls > 0)
                        {
                            mario.countBalls--;
                        }
                    }
                    print("No fireballs");
                }
            }
        }

        if(collision.collider.tag == "block" || collision.collider.tag == "weakblock" || collision.collider.tag == "question")
        {
            SoundSystem.ss.PlayBump();
        }

        if (collision.collider.tag == "koopa" || collision.collider.tag == "shell" || collision.collider.tag == "goomba")
        {
            SoundSystem.ss.PlayKick();
            if (collision.collider.tag == "goomba")
            {
                gm.UpdateDebug("Fireball kills Goomba");
                StartCoroutine(collision.collider.gameObject.GetComponent<Goomba>().DeathByShell());
            } else { 
                Destroy(collision.gameObject);
            }
        }
    }
}
