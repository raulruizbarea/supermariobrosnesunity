using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlockCoins : MonoBehaviour
{
    // Animator for when no coins
    Animator animator;
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

    GameManager gm;

    int statusMario;
    public GameObject Mario;
    public int move;
    Vector2 initialPosition;

    GameObject Gift;
    public GameObject GiftInstance;
    public Gifts giftCode;
    // How many coins has
    int count;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Mario = GameObject.FindGameObjectWithTag("mario");
        initialPosition = gameObject.transform.position;
        hit = false;
        count = 0;
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

        if (hitHead.collider != null || hitLeft.collider != null || hitHead.collider != null)
        {
            try { 
                if (hitHead.collider.gameObject == Mario || hitLeft.collider.gameObject == Mario || hitRight.collider.gameObject == Mario
                && statusMario != 4)
                {
                    SoundSystem.ss.PlayBump();
                    // To 8 coins
                    if (!hit && count < 8)
                    {
                        StartCoroutine(Rebote());
                        StartCoroutine(NoHit());
                        GenerateCoin();
                        count++;
                    }
                }
            } catch (Exception e)
            {
                print("One hit delay");
            }
        }

        if(count == 8)
        {
            animator.SetBool("hit", true);
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

    void GenerateCoin()
    {
        SoundSystem.ss.PlayCoin();
        gm.UpdatePoints(200);
        gm.UpdateCoins();
        gm.UpdateDebug("Mario gain coin");
        Gift = (GameObject)Instantiate(GiftInstance, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
        giftCode = Gift.GetComponent<Gifts>();
        giftCode.gift = 0;
    }

    public IEnumerator NoHit()
    {
        hit = true;
        yield return new WaitForSeconds(0.5f);
        hit = false;
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
