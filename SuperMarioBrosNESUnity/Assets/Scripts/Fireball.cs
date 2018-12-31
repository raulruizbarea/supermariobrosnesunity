using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Mario
    Movement mario;
    Rigidbody2D rb;
    // Velocity of the fireball
    float velocity;
    // For sounds
    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("mario").GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();

        // If mario looks right the fireball goes right otherwise left
        if (mario.lookRight)
        {
            velocity = 8f;
        }
        else
        {
            velocity = -8f;
        }
        // Set the velocity
        rb.velocity = new Vector2(velocity, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If is not floor
        if (collision.collider.tag != "floor")
        {
            // If is not mario
            if(collision.collider.tag != "mario")
            {
                try {
                    // Destroy the fireball and count one less 
                    Destroy(gameObject);
                    mario.countBalls--;
                } catch (NullReferenceException ex)
                {
                    // Be sure the fireball is deteled
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

        // If its any block the sound for destroy
        if(collision.collider.tag == "block" || collision.collider.tag == "weakblock" || collision.collider.tag == "question")
        {
            SoundSystem.ss.PlayBump();
        }

        // If touchs koopa shell or goomba kick that enemy and if its goomba play animation of death otherwise just destroy the enemy
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
