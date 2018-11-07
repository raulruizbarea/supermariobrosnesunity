using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour {
    public float velocityX = 0.1f;
    public float movementX;
    public float inputX;

    public float jumpForce = 350f;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
         inputX = Input.GetAxis("Horizontal");

        if (inputX > 0){
            movementX = transform.position.x + (inputX * velocityX);
            transform.position = new Vector3(movementX, transform.position.y, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (inputX < 0)
        {
            movementX = transform.position.x + (inputX * velocityX);
            transform.position = new Vector3(movementX, transform.position.y, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }
    }
}
