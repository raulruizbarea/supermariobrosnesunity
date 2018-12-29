using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variable para seguir a Mario
    public Transform follow;
    //Variable para como se movera la camara
    public float suavity;
    //Variable velocidad en X
    private float velocityX = 0;

    //Limites y posiciones de la camara
    public float leftLimit;
    public float rightLimit;
    private float newPositionX;
    private float oldPositionX;

    // Use this for initialization
    void Start()
    {
        //Inicializo la posicion anterior a 0
        oldPositionX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Miro la nueva posicion de la camara con el objetivo
        newPositionX = Mathf.SmoothDamp(transform.position.x, follow.position.x, ref velocityX, suavity);

        //Si la nueva posicion es mayor que la anterior (nos sirve para que la camara solo avance a la derecha)
        if (newPositionX > oldPositionX)
        {
            //Fuerzo la posicion para que quede entre los limites indicados manualmente
            if (newPositionX > rightLimit || newPositionX < leftLimit)
            {
                newPositionX = transform.position.x;
            }
            //Guardo la posicion anterior para compararla con la nueva
            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
            oldPositionX = newPositionX;
        }
    }
}
