using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    public bool hit;

    public GameObject RaycastLeft;
    Vector3 startRaycastLeft;
    Vector3 endRaycastLeft;

    public GameObject RaycastCenter;
    Vector3 startRaycastCenter;
    Vector3 endRaycastCenter;

    public GameObject RaycastRight;
    Vector3 startRaycastRight;
    Vector3 endRaycastRight;

    int statusMario;
    public GameObject Mario;
    public int rebote;
    Vector2 initialPosition;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Mario = GameObject.FindGameObjectWithTag("mario");
        initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
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
                    StartCoroutine(Rebote());
                }
                else
                {
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
                    StartCoroutine(Rebote());
                }
                else
                {
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
                    StartCoroutine(Rebote());
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        if (rebote == 1)
        {
            transform.Translate(Vector3.up * 2 * Time.deltaTime);
        }

        if (rebote == -1)
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            gameObject.transform.position = initialPosition;
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
