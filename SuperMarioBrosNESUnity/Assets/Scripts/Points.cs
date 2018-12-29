using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "lifeup")
        {
            SoundSystem.ss.PlayLifeUp();
            gm.UpdateLifes();
        }

        if (collision.gameObject.name == "mushroom")
        {
            gm.UpdatePoints(1000);

        }

        if ( collision.gameObject.name == "flower")
        {
            gm.UpdatePoints(1000);
        }
    }
}
