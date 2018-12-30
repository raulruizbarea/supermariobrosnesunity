using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public GameObject Mario;
    public GameObject Shell;
    public float velocityX = 0f;
    public bool lookRight = false;
    public bool pisada = false;

    private bool oneTime;

    public GameObject raycast;
    public GameObject raycastHead;
    private Vector3 startRaycastLeft;
    private Vector3 endRaycastLeft;
    private Vector3 startRaycastRight;
    private Vector3 endRaycastRight;
    private Vector3 startRaycastHead;
    private Vector3 endRaycastHead;

    public float raycastLength;
    public float raycastLengthHead;

    Animator animator;
    Rigidbody2D rb;

    int statusMario;
    bool marioDeath;

    bool noHit;

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
        oneTime = false;
    }

    private void Update()
    {
        statusMario = Mario.GetComponent<Movement>().statusMario;
        marioDeath = Mario.GetComponent<Movement>().marioDeath;

        startRaycastRight = raycast.transform.position;
        endRaycastRight = new Vector3(raycast.transform.position.x + raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag != "mario")
            {
                //print(hitRight.collider.tag);
                lookRight = !lookRight;
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                velocityX *= -1;
            }

            if (hitRight.collider.tag == "mario" && !marioDeath && !noHit)
            {
                if (statusMario != 0)
                {
                    StartCoroutine(NoHit());
                    Mario.GetComponent<Movement>().marioHit = true;
                }
                else
                {
                    Mario.GetComponent<Movement>().marioDeath = true;
                }
            }
        }

        startRaycastLeft = raycast.transform.position;
        endRaycastLeft = new Vector3(raycast.transform.position.x - raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitLeft = Physics2D.Linecast(startRaycastLeft, endRaycastLeft);
        Debug.DrawLine(startRaycastLeft, endRaycastLeft, Color.red);


        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag != "mario")
            {
                lookRight = !lookRight;
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                velocityX *= -1;
            }

            if (hitLeft.collider.tag == "mario" && !marioDeath && !noHit)
            {
                if (statusMario != 0)
                {
                    StartCoroutine(NoHit());
                    Mario.GetComponent<Movement>().marioHit = true;
                    //gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                }
                else
                {
                    Mario.GetComponent<Movement>().marioDeath = true;
                }
            }
        }

        startRaycastHead = raycastHead.transform.position;
        endRaycastHead = new Vector3(raycastHead.transform.position.x, raycastHead.transform.position.y + raycastLengthHead, 0);

        RaycastHit2D hitHead = Physics2D.Linecast(startRaycastHead, endRaycastHead);
        Debug.DrawLine(startRaycastHead, endRaycastHead, Color.red);
        if (hitHead.collider != null)
        {
            if (hitHead.collider.gameObject == Mario)
            {
                gm.UpdatePoints(100);
                animator.SetBool("pisada", pisada);
                Instantiate(Shell, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oneTime && (this.transform.position.x - Mario.transform.position.x) < 8)
        {
            velocityX = -1f;
            oneTime = true;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);
        animator.SetFloat("velocityX", Mathf.Abs(velocityX));
    }

    public void Pause()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("pause", true);
    }

    public void Resume()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("pause", false);
    }

    public IEnumerator NoHit()
    {
        noHit = true;
        yield return new WaitForSeconds(1f);
        noHit = false;
        StopCoroutine(NoHit());
    }
}
