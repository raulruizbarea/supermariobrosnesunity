
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float velocityX = 0.08f;
    public float movementX;
    public float inputX;
    public bool lookRight;

    public float jumpForce = 350f;

    public Transform pie;
    public float radioPie = 0.1f;
    public LayerMask floor;
    public bool isFloor;
    public bool isCouch;
    public bool noHit;
    public int isDrift;
    public bool isRun;
    public bool isMoving;

    Animator animator;
    Rigidbody2D rb;
    public float falldown;
    public int right, left;

    public GameObject Shell;
    public GameObject Mario;

    public GameObject raycast;
    private Vector3 startRaycastLeft;
    private Vector3 endRaycastLeft;
    private Vector3 startRaycastRight;
    private Vector3 endRaycastRight;

    public float raycastLength;

    public RuntimeAnimatorController[] Marios;
    public int statusMario; //0 small 1 big 2 fire 3 hit
    bool changeStatus;
    public bool marioHit;

    Color alpha;
    Color marioAlpha;

    public GameObject[] allObjects;
    public GameObject[] koopas;
    public GameObject[] goombas;
    public GameObject[] shells;

    public bool marioDeath;
    public CircleCollider2D colliderDeath;

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = true;
    }

    void Update()
    {
        if (statusMario == 0)
        {
            animator.runtimeAnimatorController = Marios[0];
        }

        if (statusMario == 1)
        {
            animator.runtimeAnimatorController = Marios[1];
        }

        if (statusMario == 2)
        {
            animator.runtimeAnimatorController = Marios[2];
        }

        if(marioHit)
        {
            StartCoroutine(Hit());
            marioHit = false;
        }

        if(marioDeath && statusMario != 4)
        {
            StartCoroutine(Death());
        }

        allObjects = GameObject.FindObjectsOfType<GameObject>();
        koopas = GameObject.FindGameObjectsWithTag("koopa");
        goombas = GameObject.FindGameObjectsWithTag("goomba");
        shells = GameObject.FindGameObjectsWithTag("shell");

        startRaycastRight = raycast.transform.position;
        endRaycastRight = new Vector3(raycast.transform.position.x + raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        if (hitRight.collider != null)
        {
            if(hitRight.collider.tag == "shell" && lookRight)
            {
                Shell = hitRight.collider.gameObject;
                if (Shell.GetComponent<Shell>().velocityKoopa == 0) {
                    StartCoroutine(NoHit());
                    Shell.GetComponent<Shell>().velocityKoopa = 4f;
                }
            }
        }

        startRaycastLeft = raycast.transform.position;
        endRaycastLeft = new Vector3(raycast.transform.position.x - raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitLeft = Physics2D.Linecast(startRaycastLeft, endRaycastLeft);
        Debug.DrawLine(startRaycastLeft, endRaycastLeft, Color.red);


        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag == "shell" && !lookRight)
            {
                Shell = hitLeft.collider.gameObject;
                if (Shell.GetComponent<Shell>().velocityKoopa == 0)
                {
                    StartCoroutine(NoHit());
                    Shell.GetComponent<Shell>().velocityKoopa = -4f;
                }
            }
        }
    }

    void FixedUpdate () {
         inputX = Input.GetAxis("Horizontal");

         if (!isCouch && isMoving && !marioDeath) { 
            if (inputX > 0){
                movementX = transform.position.x + (inputX * velocityX);
                transform.position = new Vector3(movementX, transform.position.y, 0);
                transform.localScale = new Vector3(1, 1, 1);
                // animator.SetFloat("velocityX", inputX);
                lookRight = true;
            }

            if (inputX < 0)
            {
                movementX = transform.position.x + (inputX * velocityX);
                transform.position = new Vector3(movementX, transform.position.y, 0);
                transform.localScale = new Vector3(-1, 1, 1);
                //animator.SetFloat("velocityX", Mathf.Abs(inputX));
                lookRight = false;
            }
        }

        if (inputX != 0 && isFloor)
        {
            animator.SetFloat("velocityX", 1);
        }
        else
        {
            animator.SetFloat("velocityX", 0);
        }

        isFloor = Physics2D.OverlapCircle(pie.position, radioPie, floor);

        if (isFloor)
        {
            animator.SetBool("isFloor", true);

            //if (!isCouch && Input.GetKeyDown(KeyCode.Space))
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            }
        }
        else
        {
            animator.SetBool("isFloor", false);
        }

        if (isFloor && Input.GetKey(KeyCode.DownArrow) && statusMario != 0)
        {
            isCouch = true;
            animator.SetBool("isCouch", true);
        }
        else
        {
            isCouch = false;
            animator.SetBool("isCouch", false);
        }

        falldown = rb.velocity.y;

        if(falldown != 0 || falldown == 0)
        {
            animator.SetFloat("velocityY", falldown);
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(WaitingTime());
        }

        if (inputX > 0.5f)
        {
            right = 1;
        }

        if (right == 1)
        {
            isDrift = 1;
        }

        if (isDrift == 1 && Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isDrift", true);
            StartCoroutine(WaitingTime());
        }

        if (inputX < 0)
        {
            left = 1;
        }

        if (left == 1)
        {
            isDrift = -1;
        }

        if (isDrift == -1 && Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isDrift", true);
            StartCoroutine(WaitingTime());
        }

        if (inputX > 0 || inputX < 0)
        {
            if(Input.GetKey(KeyCode.Z))
            {
                isRun = true;
                animator.SetBool("isRun", true);
                velocityX = 0.14f;
            }
            else
            {
                isRun = false;
                velocityX = 0.08f;
                animator.SetBool("isRun", false);
            }
        }

        if (inputX == 0)
        {
            isRun = false;
            animator.SetBool("isRun", false);
        }

        if (changeStatus)
        {
            StartCoroutine(Pause());
            isMoving = false;
            changeStatus = false;
            //print(changeStatus);
        }
    }

    public void UpdateStatus(int newStatus)
    {
        changeStatus = true;
        statusMario = newStatus;
    }

    public IEnumerator Pause()
    {
        Vector2 velAntes = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = true;

        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "koopa")
            {
                for (int j = 0; j < koopas.Length; j++)
                {
                    koopas[j].GetComponent<Koopa>().Pause();
                }
            }
            if (allObjects[i].tag == "goomba")
            {
                for (int j = 0; j < goombas.Length; j++)
                {
                    goombas[j].GetComponent<Goomba>().Pause();
                }
            }

            if (allObjects[i].tag == "shell")
            {
                for (int j = 0; j < shells.Length; j++)
                {
                    shells[j].GetComponent<Shell>().Pause();
                }
            }
        }

        if(!marioDeath) {
            if (statusMario == 1)
            {
                animator.SetBool("superMario", false);
            }
            else if(statusMario == 2)
            {
                animator.SetBool("fireMario", false);
            }

            yield return new WaitForSeconds(1f);
            changeStatus = false;

            if (statusMario == 1)
            {
                animator.SetBool("superMario", true);
            }
            else if (statusMario == 2)
            {
                animator.SetBool("fireMario", true);
            }

            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].tag == "koopa")
                {
                    for (int j = 0; j < koopas.Length; j++)
                    {
                        koopas[j].GetComponent<Koopa>().Resume();
                    }
                }
                if (allObjects[i].tag == "goomba")
                {
                    for (int j = 0; j < goombas.Length; j++)
                    {
                        goombas[j].GetComponent<Goomba>().Resume();
                    }
                }

                if (allObjects[i].tag == "shell")
                {
                    for (int j = 0; j < shells.Length; j++)
                    {
                        shells[j].GetComponent<Shell>().Resume();
                    }
                }
            }
        }

        rb.velocity = velAntes;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //rb.isKinematic = false;
        isMoving = true;
    }

    public IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(0.3f);
        isDrift = 0;
        left = 0;
        right = 0;
        animator.SetBool("isDrift", false);
    }

    public IEnumerator NoHit()
    {
        noHit = true;
        yield return new WaitForSeconds(0.5f);
        noHit = false;
        StopCoroutine(NoHit());
    }

    public IEnumerator Hit()
    {
        if(statusMario != 0)
        {
            statusMario--;

            if (statusMario == 1)
            {
                animator.SetBool("superMario", false);
            }
            else if (statusMario == 2)
            {
                animator.SetBool("fireMario", false);
            }

            yield return new WaitForSeconds(0.1f);
            changeStatus = false;

            if (statusMario == 1)
            {
                animator.SetBool("superMario", true);
            }
            else if (statusMario == 2)
            {
                animator.SetBool("fireMario", true);
            }

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.1f);
                alpha = new Color(1f, 1f, 1f, 0);
                marioAlpha = Mario.GetComponent<SpriteRenderer>().color = alpha;
                yield return new WaitForSeconds(0.1f);
                alpha = new Color(1f, 1f, 1f, 1f);
                marioAlpha = Mario.GetComponent<SpriteRenderer>().color = alpha;
            }
        }

        StopCoroutine(Hit());
    }

    public IEnumerator Death()
    {
        statusMario = 4;
        animator.SetBool("death", true);

         for (int j = 0; j < koopas.Length; j++)
        {
            koopas[j].GetComponent<Koopa>().StartCoroutine(Pause());
        }

        for (int j = 0; j < goombas.Length; j++)
        {
            goombas[j].GetComponent<Goomba>().StartCoroutine(Pause());
        }

        for (int j = 0; j < shells.Length; j++)
        {
            shells[j].GetComponent<Shell>().StartCoroutine(Pause());
        }

        rb.isKinematic = true;
        colliderDeath.isTrigger = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 10f);
        rb.isKinematic = false;
        yield return new WaitForSeconds(0.5f);
    }
}
