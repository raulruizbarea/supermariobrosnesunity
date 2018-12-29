using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float velocityKoopa = 4f;
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
    GameObject mario;

    int statusMario;
    bool marioDeath;
    public bool noHit;
    public bool isKick;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mario = GameObject.FindGameObjectWithTag("mario");
    }

    private void Update()
    {
        statusMario = mario.GetComponent<Movement>().statusMario;
        marioDeath = mario.GetComponent<Movement>().marioDeath;
        isKick = mario.GetComponent<Movement>().noHit;

        startRaycastRight = raycast.transform.position;
        endRaycastRight = new Vector3(raycast.transform.position.x + raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag != "mario")
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                velocityKoopa *= -1;
            }

            if (hitRight.collider.tag == "mario" && Mathf.Abs(velocityKoopa) > 0 && !marioDeath && !noHit && !isKick)
            {
                if (statusMario != 0)
                {
                    StartCoroutine(NoHit());
                    mario.GetComponent<Movement>().marioHit = true;
                }
                else
                {
                    mario.GetComponent<Movement>().marioDeath = true;
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
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                velocityKoopa *= -1;
            }

            if (hitLeft.collider.tag == "mario" && Mathf.Abs(velocityKoopa) > 0 && !marioDeath && !noHit && !isKick)
            {
                if (statusMario != 0)
                {
                    StartCoroutine(NoHit());
                    mario.GetComponent<Movement>().marioHit = true;
                }
                else
                {
                    mario.GetComponent<Movement>().marioDeath = true;
                }
            }
        }

        startRaycastHead = raycastHead.transform.position;
        endRaycastHead = new Vector3(raycastHead.transform.position.x, raycastHead.transform.position.y + raycastLengthHead, 0);

        RaycastHit2D hitHead = Physics2D.Linecast(startRaycastHead, endRaycastHead);
        Debug.DrawLine(startRaycastHead, endRaycastHead, Color.red);
        if (hitHead.collider != null)
        {
            if (hitHead.collider.tag == "mario")
            {
                if (velocityKoopa > 0)
                {
                    StartCoroutine(NoHit());
                    rb.velocity = Vector2.zero;
                    velocityKoopa = 0;
                }
                else
                {
                    velocityKoopa = 4f;
                    rb.velocity = new Vector2(velocityKoopa, rb.velocity.y);
                }
            }
        }
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

    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocityKoopa, rb.velocity.y);
        animator.SetFloat("velocityX", Mathf.Abs(this.rb.velocity.x));
    }

    public IEnumerator NoHit()
    {
        noHit = true;
        yield return new WaitForSeconds(1f);
        noHit = false;
        StopCoroutine(NoHit());
    }
}
