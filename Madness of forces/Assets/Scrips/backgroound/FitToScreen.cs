using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se asegura de que el sprite llene la pantalla de la cámara.
// Es ideal para fondos.
[RequireComponent(typeof(SpriteRenderer))]
public class FitToScreen : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Obtenemos el tamaño del sprite en unidades de Unity
        float spriteAncho = spriteRenderer.sprite.bounds.size.x;
        float spriteAlto = spriteRenderer.sprite.bounds.size.y;

        // Obtenemos el alto y ancho de la pantalla en unidades de Unity
        float altoPantalla = Camera.main.orthographicSize * 2.0f;
        float anchoPantalla = altoPantalla * Screen.width / Screen.height;

        // Calculamos la escala necesaria para cubrir la pantalla
        float escalaX = anchoPantalla / spriteAncho;
        float escalaY = altoPantalla / spriteAlto;

        // Usamos la escala más grande para asegurarnos de que cubra todo (evita franjas)
        float escalaFinal = Mathf.Max(escalaX, escalaY);

        // Aplicamos la escala al objeto
        transform.localScale = new Vector3(escalaFinal, escalaFinal, 1);
    }
}