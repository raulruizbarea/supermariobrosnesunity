
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {
    // Velocity
    public float velocityX = 5f;
    // Left or right
    public float inputX;
    public float newPosX;
    // Where is mario looking
    public bool lookRight;
    // Jump force
    public float jumpForce = 800f;
    // Feet
    public Transform pie;
    // How big is the feet
    public float radioPie = 0.1f;
    // If is touching floor
    public LayerMask floor;
    public bool isFloor;
    // If its couching
    public bool isCouch;
    // Waiting time for no hit mario if he was hit
    public bool noHit;
    // Check drift
    public int isDrift;
    // Running with Z
    public bool isRun;
    public bool isMoving;
    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D bc;
    // Falldown
    public float falldown;
    public int right, left;
    // Shell
    public GameObject Shell;
    public GameObject Mario;
    // Raycast touching objects
    public GameObject raycast;
    private Vector3 startRaycastLeft;
    private Vector3 endRaycastLeft;
    private Vector3 startRaycastRight;
    private Vector3 endRaycastRight;
    public float raycastLength;
    public RuntimeAnimatorController[] Marios;
    public int statusMario; //0 small 1 supermario 2 firemario 3 hit
    bool changeStatus;
    public bool marioHit;
    // When hit change alphas
    Color alpha;
    Color marioAlpha;
    // To do pause after touch mushrooms or flower
    public GameObject[] allObjects;
    public GameObject[] koopas;
    public GameObject[] goombas;
    public GameObject[] shells;
    // Death of mario
    public bool marioDeath;
    public BoxCollider2D colliderDeath;
    // Fireball
    public bool isThrow = true;
    public int countBalls=0;
    public GameObject fireball;
    public GameObject[] fireballs;
    // Flag
    public bool isFlag = false;

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        isMoving = true;
    }

    void Update()
    {
        // If mario small then animator and change the boxcollider small
        if (statusMario == 0)
        {
            animator.runtimeAnimatorController = Marios[0];
            bc.size = new Vector2(0.65f, 0.8f);
            bc.offset = new Vector2(0, 0.4f);
        }
        // If supermario then animator and change the boxcollider big
        if (statusMario == 1)
        {
            animator.runtimeAnimatorController = Marios[1];
            if (!isCouch) {
                bc.size = new Vector2(0.8f, 1.5f);
                bc.offset = new Vector2(0, 0.8f);
            }
        }
        // If fire mario  then animator and change the boxcollider big
        if (statusMario == 2)
        {
            animator.runtimeAnimatorController = Marios[2];
            if(!isCouch) {
                bc.size = new Vector2(0.8f, 1.5f);
                bc.offset = new Vector2(0, 0.8f);
            }
        }
        // If mario was hit start Hit effect 
        if(marioHit)
        {
            gm.UpdateDebug("Mario hit");
            StartCoroutine(Hit());
            marioHit = false;
        }
        // If mario death start Death effect
        if(marioDeath && statusMario != 4)
        {
            SoundSystem.ss.StopMusic();
            SoundSystem.ss.PlayDeath();
            StartCoroutine(Death());
        }
        // Set al objects for pause
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
                    gm.UpdateDebug("Mario can't be hit");
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
                    gm.UpdateDebug("Mario can't be hit");
                    StartCoroutine(NoHit());
                    Shell.GetComponent<Shell>().velocityKoopa = -4f;
                }
            }
        }
    }

    void FixedUpdate () {
         inputX = Input.GetAxis("Horizontal");
         // Mario move if its not couch, death and not flag
         if (!isCouch && isMoving && !marioDeath && !isFlag) {
            transform.position += new Vector3(inputX * velocityX * Time.deltaTime, 0, 0);
            newPosX = Mathf.Clamp(rb.transform.position.x, Camera.main.transform.position.x - 7.5f, Camera.main.transform.position.x + 7.5f);//-7.6f, 195f);
            transform.position = new Vector3(newPosX, rb.transform.position.y, 0);

            if (rb.transform.position.y < -7)
            {
                gm.UpdateDebug("Mario death by hole");
                marioDeath = true;
            }

            if (inputX > 0){
                 transform.localScale = new Vector3(1, 1, 1);
                lookRight = true;
            }

            if (inputX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                lookRight = false;
            }
        }
         // If its moving and floor set animation
        if (inputX != 0 && isFloor)
        {
            animator.SetFloat("velocityX", 1);
        }
        else
        {
            animator.SetFloat("velocityX", 0);
        }

        // If is touching floor animation and if press space jump
        isFloor = Physics2D.OverlapCircle(pie.position, radioPie, floor);
        if (isFloor)
        {
            animator.SetBool("isFloor", true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gm.UpdateDebug("Mario jump");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                SoundSystem.ss.PlayJump();
            }
        }
        else
        {
            animator.SetBool("isFloor", false);
        }
        //If press down arrow couch, and make collider smaller
        if (isFloor && Input.GetKey(KeyCode.DownArrow) && statusMario != 0)
        {
            bc.size = new Vector2(0.8f, 1.2f);
            bc.offset = new Vector2(0, 0.6f);
            isCouch = true;
            animator.SetBool("isCouch", true);
        }
        else
        {
            isCouch = false;
            animator.SetBool("isCouch", false);
        }
        // Falldown animation
        falldown = rb.velocity.y;

        if(falldown != 0 || falldown == 0)
        {
            animator.SetFloat("velocityY", falldown);
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            // Avoid rare effects left and arrow same time
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
                velocityX = 8f;
            }
            else
            {
                isRun = false;
                velocityX = 5f;
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
        }
        // If its fire mario can throw 2 fireballs
        if(statusMario == 2)
        {
            fireballs = GameObject.FindGameObjectsWithTag("fireball");
            if(fireballs.Length == 0)
            {
                countBalls = 0;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (countBalls < 2 && fireballs.Length < 2)
                {
                    gm.UpdateDebug("Mario throw fireball");
                    SoundSystem.ss.PlayFireball();
                    ThrowBall();
                }
            }
        }
    }
    // Change mario status
    public void UpdateStatus(int newStatus)
    {
        changeStatus = true;
        statusMario = newStatus;
    }

    public IEnumerator Pause()
    {
        // Check last velocity
        Vector2 velAntes = rb.velocity;
        // Stop mario
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // Freeze enemies
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
        // If mario is not death powerUP!
        if(!marioDeath) {
            SoundSystem.ss.PlayPowerUp();
            if (statusMario == 1)
            {
                gm.UpdateDebug("SUPER MARIO!");
                animator.SetBool("superMario", false);
            }
            else if(statusMario == 2)
            {
                gm.UpdateDebug("FIRE MARIO!");
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
        // Continue lat status
        rb.velocity = velAntes;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isMoving = true;
    }

    public IEnumerator WaitingTime()
    {
        // Wait for no double keys press
        yield return new WaitForSeconds(0.3f);
        isDrift = 0;
        left = 0;
        right = 0;
        animator.SetBool("isDrift", false);
    }

    public IEnumerator NoHit()
    {
        // Mario cant be hit if it was hit
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
        yield return new WaitForSeconds(2f);
        TitleManager.lifes--;
        if (TitleManager.lifes == 0)
        {
            SceneManager.LoadScene("gameover");
        }
        else
        {
            SceneManager.LoadScene("level");
        }
    }

    public void ThrowBall()
    {
        animator.SetTrigger("throw");
        if (lookRight)
        {
            GameObject fire = Instantiate(fireball, new Vector2(transform.position.x + 0.5f, transform.position.y + 1f), Quaternion.identity);
            try
            {
                Destroy(fire, 2);
            } catch (Exception e)
            {
                print("Fireball destroyed");
            }
        }
        else
        {
            GameObject fire = Instantiate(fireball, new Vector2(transform.position.x - 0.5f, transform.position.y + 1f), Quaternion.identity);
            try
            {
                Destroy(fire, 2);
            }
            catch (Exception e)
            {
                print("Fireball destroyed");
            }
        }

        countBalls++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "flag")
        {
            animator.SetBool("isFlag", true);
            gm.UpdatePoints(400);
            isFlag = true;
            collision.transform.SendMessage("AnimateFlag");
            StartCoroutine(Redirect());
        }
    }

    public IEnumerator Redirect()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("gameover");
    }
}
