using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public GameObject Mario;
    GameObject Gift;
    public GameObject GiftInstance;
    public Gifts giftCode;

    public bool hit;
    public bool haveGift;

    public GameObject RaycastLeft;
     Vector3 startRaycastLeft;
     Vector3 endRaycastLeft;

    public GameObject RaycastCenter;
     Vector3 startRaycastCenter;
     Vector3 endRaycastCenter;

    public GameObject RaycastRight;
     Vector3 startRaycastRight;
     Vector3 endRaycastRight;

    public int rebote;
    public int numeroAleatorio;

    Animator animator;
    Rigidbody2D rb;

    int statusMario;
    Vector2 initialPosition;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Mario = GameObject.FindGameObjectWithTag("mario");
        hit = false;
        haveGift = true;
        initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        statusMario = Mario.GetComponent<Movement>().statusMario;

        if (statusMario == 4)
        {
            animator.enabled = false;
        }

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

        if(hitHead.collider != null)
        {
            if(hitHead.collider.gameObject == Mario && statusMario != 4)
            {
                SoundSystem.ss.PlayBump();
                if (!hit)
                {
                    StartCoroutine(Rebote());
                    GenerateGift();
                }
            }
        }

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.gameObject == Mario && statusMario != 4)
            {
                SoundSystem.ss.PlayBump();
                if (!hit)
                {
                    StartCoroutine(Rebote());
                    GenerateGift();
                }
            }
        }

        if (hitRight.collider != null)
        {
            SoundSystem.ss.PlayBump();
            if (hitRight.collider.gameObject == Mario && statusMario != 4)
            {
                if (!hit)
                {
                    StartCoroutine(Rebote());
                    GenerateGift();
                }
            }
        }


        if(rebote == 1)
        {
            transform.Translate(Vector3.up * 2 * Time.deltaTime);
            haveGift = false;
        }

        if (rebote == -1)
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            haveGift = false; 
            gameObject.transform.position = initialPosition;
        }
    }

    void GenerateGift()
    {
        //TODO CAMBIAR
        //numeroAleatorio = Random.Range(0, 4);
       //numeroAleatorio = Random.Range(3, 4);
       //numeroAleatorio = Random.Range(1, 2);
        if(numeroAleatorio == 0)
        {
            SoundSystem.ss.PlayCoin();
            gm.UpdatePoints(200);
            gm.UpdateCoins();
        } else if (numeroAleatorio == 3 && statusMario == 0)
        {
            numeroAleatorio = 1;
        }

        if (numeroAleatorio != 0)
        {
            SoundSystem.ss.PlayMushroom();
        }

        if (haveGift) { 
            Gift = (GameObject)Instantiate(GiftInstance, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
            hit = true;
            animator.SetBool("hit", hit);
            giftCode = Gift.GetComponent<Gifts> ();
            giftCode.gift = numeroAleatorio;
        }
    }

    public IEnumerator Rebote()
    {
        rebote = 1;
        yield return new WaitForSeconds(0.1f);
        rebote = -1;
        yield return new WaitForSeconds(0.1f);
        rebote = 0;
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
