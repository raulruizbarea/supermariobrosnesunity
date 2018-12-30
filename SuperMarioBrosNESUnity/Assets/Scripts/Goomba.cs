using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float velocityX = 0.5f;
    public bool pisada = false;

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
    bool noHit;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mario = GameObject.FindGameObjectWithTag("mario");
    }

    void Update()
    {
        statusMario = mario.GetComponent<Movement>().statusMario;
        marioDeath = mario.GetComponent<Movement>().marioDeath;

        startRaycastRight = raycast.transform.position;
        endRaycastRight = new Vector3(raycast.transform.position.x + raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag != "mario")
            {
                if (hitRight.collider.tag == "shell" && Mathf.Abs(hitRight.collider.gameObject.GetComponent<Rigidbody2D>().velocity.x) > 0)
                {
                    gm.UpdateDebug("Goomba death by shell");
                    SoundSystem.ss.PlayKick();
                    StartCoroutine(DeathByShell());
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                    velocityX *= -1;
                }
            }

            if(hitRight.collider.tag == "mario" && !pisada && !marioDeath && !noHit)
            {
                if(statusMario != 0)
                {
                    gm.UpdateDebug("Goomba hits Mario");
                    StartCoroutine(NoHit());
                    mario.GetComponent<Movement>().marioHit = true;
                }
                else
                {
                    gm.UpdateDebug("Goomba kills Mario");
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
                if (hitLeft.collider.tag == "shell" && Mathf.Abs(hitLeft.collider.gameObject.GetComponent<Rigidbody2D>().velocity.x) > 0)
                {
                    gm.UpdateDebug("Goomba death by shell");
                    SoundSystem.ss.PlayKick();
                    StartCoroutine(DeathByShell());
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                    velocityX *= -1;
                }
            }

            if (hitLeft.collider.tag == "mario" && !pisada && !marioDeath && !noHit)
            {
                if (statusMario != 0)
                {
                    gm.UpdateDebug("Goomba hits Mario");
                    StartCoroutine(NoHit());
                    mario.GetComponent<Movement>().marioHit = true;
                }
                else
                {
                    gm.UpdateDebug("Goomba kills Mario");
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
            if (hitHead.collider.tag == "mario" && !pisada)
            {
                gm.UpdateDebug("Goomba killed by Mario");
                gm.UpdatePoints(100);
                SoundSystem.ss.PlayGoomba();
                pisada = true;
                StartCoroutine(Death());
            }
        }
    }

    public IEnumerator DeathByShell()
    {
        pisada = true;
        velocityX = 0;
        rb.velocity = new Vector2(0, 4f);
        transform.localScale = new Vector3(transform.localScale.x, -1f, 1f);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public IEnumerator Death()
    {

        pisada = true;
        animator.SetBool("pisada", true);
        GetComponent<CircleCollider2D>().radius = 0.1f;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!pisada) { 
            rb.velocity = new Vector2(velocityX, rb.velocity.y);
            animator.SetFloat("velocityX", Mathf.Abs(velocityX));
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

    public IEnumerator NoHit()
    {
        noHit = true;
        yield return new WaitForSeconds(1f);
        noHit = false;
        StopCoroutine(NoHit());
    }
}
