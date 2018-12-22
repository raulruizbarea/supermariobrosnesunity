using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "mario")
        {
            this.gameObject.layer = 8;
        }
        else
        {
            this.gameObject.layer = 13;
        }
    }
}
