using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gifts : MonoBehaviour
{
    public int gift; //0 coin 1 mushroom 2 life up 3 flower
    public float velocityGift;
    public float direction;

    public Sprite[] sprites;
    public Sprite[] coinAnimation;
    public Sprite[] flowerAnimation;
    public int idxCoinAnimation = 0;
    public int idxFlowerAnimation = 0;
    public int lifeCoin = 0;
    SpriteRenderer sprite;

    Animator animator;
    Rigidbody2D rb;
    Collider2D colliderGift;

    public GameObject raycast;
    private Vector3 startRaycastLeft;
    private Vector3 endRaycastLeft;
    private Vector3 startRaycastRight;
    private Vector3 endRaycastRight;

    public float raycastLength;

    Movement mario;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        colliderGift = GetComponent<Collider2D>();
        mario = GameObject.FindGameObjectWithTag("mario").GetComponent<Movement>();

        // Set the name, sprite, and effect depending on what gift is coin mushroom lifeup or flower
        switch(gift)
        {
            case 0:
                gameObject.name = "coin";
                sprite.sprite = sprites[0];
                InvokeRepeating("AnimationCoin", 0.1f, 0.1f);
                Coin();
                break;
            case 1:
                gameObject.name = "mushroom";
                sprite.sprite = sprites[1];
                PowerUps();
                break;
            case 2:
                gameObject.name = "lifeup";
                sprite.sprite = sprites[2];
                PowerUps();
                break;
            case 3:
                gameObject.name = "flower";
                sprite.sprite = sprites[3];
                InvokeRepeating("AnimationFlower", 0.1f, 0.1f);
                PowerUps();
                break;
        }
    }

    void Coin()
    {
        colliderGift.isTrigger = true;
        // Apply velocity on y to make the effect of going up
        rb.velocity = new Vector2(0, 5f);
    }

    void AnimationCoin()
    {
        // Repeat the animation of moving the coin
        if (idxCoinAnimation < coinAnimation.Length -1 && lifeCoin > 8)
        {
            sprite.sprite = coinAnimation[idxCoinAnimation];
            idxCoinAnimation++;
        }
        else if (idxCoinAnimation == coinAnimation.Length - 1)
        {
            idxCoinAnimation = 0;
        }

        lifeCoin++;

        // Destroy coin after the time pass
        if(lifeCoin > 8)
        {
            Destroy(gameObject);
        }
    }

    void AnimationFlower()
    {
        // Same as coin
        if (idxFlowerAnimation < flowerAnimation.Length - 1)
        {
            sprite.sprite = flowerAnimation[idxFlowerAnimation];
            idxFlowerAnimation++;
        }
        else if (idxFlowerAnimation == flowerAnimation.Length - 1)
        {
            idxFlowerAnimation = 0;
        }
    }

    void PowerUps()
    {
        colliderGift.isTrigger = true;
        // Apply gravity to make stop
        rb.gravityScale = 0;
        // Velocity to stay nearby the block
        rb.velocity = new Vector2(0, 0.7f);
    }

    void Update()
    {
        // Raycast for change direction
        startRaycastRight = raycast.transform.position;
        endRaycastRight = new Vector3(raycast.transform.position.x + raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitRight = Physics2D.Linecast(startRaycastRight, endRaycastRight);
        Debug.DrawLine(startRaycastRight, endRaycastRight, Color.red);

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag == "block")
            {
                ChangeDirection();
            }
        }

        // Raycast for change direction
        startRaycastLeft = raycast.transform.position;
        endRaycastLeft = new Vector3(raycast.transform.position.x - raycastLength, raycast.transform.position.y, 0);

        RaycastHit2D hitLeft = Physics2D.Linecast(startRaycastLeft, endRaycastLeft);
        Debug.DrawLine(startRaycastLeft, endRaycastLeft, Color.red);


        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag == "block")
            {
                ChangeDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        // Velocity continuos
        rb.velocity = new Vector2(velocityGift, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If its mario
        if(collision.gameObject.tag == "mario")
        {
            // If its a mushroom
            if(gameObject.name == "mushroom")
            {
                // Destroy the mushroom
                Destroy(gameObject);
                // If its small mario now is SUPERMARIO!
                if (mario.GetComponent<Movement>().statusMario == 0)
                {
                    mario.UpdateStatus(1);
                }
            }
            else if (gameObject.name == "lifeup")
            {
                // Destroy the lifeup
                Destroy(gameObject);
            }
            else if (gameObject.name == "flower")
            {
                // Destroy the flower
                Destroy(gameObject);
                // If its super mario now is FIREMARIO!
                mario.UpdateStatus(2);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If its mushroom or lifeup
        if(gift == 1 || gift == 2)
        {
            // Random direction
            direction = Random.Range(0, 2);
            if (direction == 0)
            {
                velocityGift = 1f; ;
            }
            else
            {
                velocityGift = -1f;
            }
            rb.velocity = new Vector2(velocityGift, 0);
        }
        // Set gravity
        rb.gravityScale = 1;
        colliderGift.isTrigger = false;
    }

    void ChangeDirection()
    {
        // Inverse velocity
        velocityGift *= -1;
    }
}
