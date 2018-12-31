using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{
    // If its hit
    public bool hit;
    // To check if its hit by mario
    public GameObject RaycastLeft;
    Vector3 startRaycastLeft;
    Vector3 endRaycastLeft;
    public GameObject RaycastCenter;
    Vector3 startRaycastCenter;
    Vector3 endRaycastCenter;
    public GameObject RaycastRight;
    Vector3 startRaycastRight;
    Vector3 endRaycastRight;
    // Know status mario to move or destroy it
    int statusMario;
    public GameObject Mario;
    public int move;
    // Initial position because had problems with physics
    Vector2 initialPosition;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        Mario = GameObject.FindGameObjectWithTag("mario");
        // Save old position
        initialPosition = gameObject.transform.position;
    }

    void Update()
    {
        statusMario = Mario.GetComponent<Movement>().statusMario;

        startRaycastRight = RaycastRight.transform.position;
        endRaycastRight = new Vector3(RaycastRight.transform.position.x, RaycastRight.transform.position.y - 0.1f, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        startRaycastLeft = RaycastLeft.transform.position;
        endRaycastLeft = new Vector3(RaycastLeft.transform.position.x, RaycastLeft.transform.position.y - 0.1f, 0);

        RaycastHit2D hitLeft = Physics2D.Linecast(startRaycastLeft, endRaycastLeft);
        Debug.DrawLine(startRaycastLeft, endRaycastLeft, Color.yellow);

        startRaycastCenter = RaycastCenter.transform.position;
        endRaycastCenter = new Vector3(RaycastCenter.transform.position.x, RaycastCenter.transform.position.y - 0.1f, 0);

        RaycastHit2D hitHead = Physics2D.Linecast(startRaycastCenter, endRaycastCenter);
        Debug.DrawLine(startRaycastCenter, endRaycastCenter, Color.grey);

        if (hitHead.collider != null)
        {
            if (hitHead.collider.gameObject == Mario && statusMario != 4)
            {
                if (statusMario == 0)
                {
                    gm.UpdateDebug("Block hit by Mario");
                    SoundSystem.ss.PlayBump();
                    StartCoroutine(Rebote());
                }
                else
                {
                    gm.UpdateDebug("Block broke by Mario");
                    SoundSystem.ss.PlayBlockBreak();
                    gm.UpdatePoints(100);
                    Destroy(gameObject);
                }
            }
        }

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.gameObject == Mario && statusMario != 4)
            {
                if (statusMario == 0)
                {
                    gm.UpdateDebug("Block hit by Mario");
                    SoundSystem.ss.PlayBump();
                    StartCoroutine(Rebote());
                }
                else
                {
                    gm.UpdateDebug("Block broke by Mario");
                    SoundSystem.ss.PlayBlockBreak();
                    gm.UpdatePoints(100);
                    Destroy(gameObject);
                }
            }
        }

        if (hitRight.collider != null)
        {
            if (hitRight.collider.gameObject == Mario && statusMario != 4)
            {
                if (statusMario == 0)
                {
                    gm.UpdateDebug("Block hit by Mario");
                    SoundSystem.ss.PlayBump();
                    StartCoroutine(Rebote());
                }
                else
                {
                    gm.UpdateDebug("Block broke by Mario");
                    SoundSystem.ss.PlayBlockBreak();
                    gm.UpdatePoints(100);
                    Destroy(gameObject);
                }
            }
        }

        if (move == 1)
        {
            transform.Translate(Vector3.up * 2 * Time.deltaTime);
        }

        if (move == -1)
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            gameObject.transform.position = initialPosition;
        }
    }

    public IEnumerator Rebote()
    {
        move = 1;
        yield return new WaitForSeconds(0.1f);
        move = -1;
        yield return new WaitForSeconds(0.1f);
        move = 0;
    }

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
