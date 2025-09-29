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
        // Posición inicial del fondo
        startPos = transform.position.x;
        // Longitud del sprite del fondo
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Qué tanto se ha movido la cámara relativo a nuestra posición inicial
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        // Qué tanto se ha movido la cámara en total
        float dist = (cam.transform.position.x * parallaxEffect);

        // Movemos el fondo a la nueva posición
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // Lógica para que el fondo se repita
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