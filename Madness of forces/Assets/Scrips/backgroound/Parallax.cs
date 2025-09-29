using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        // Posici�n inicial del fondo
        startPos = transform.position.x;
        // Longitud del sprite del fondo
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Qu� tanto se ha movido la c�mara relativo a nuestra posici�n inicial
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        // Qu� tanto se ha movido la c�mara en total
        float dist = (cam.transform.position.x * parallaxEffect);

        // Movemos el fondo a la nueva posici�n
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // L�gica para que el fondo se repita
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}