using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemiga : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float lifeTime = 3f; // Tiempo en segundos antes de autodestruirse

    void Start()
    {
        // 1. Asegura que no tenga gravedad
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // 2. Se impulsa hacia adelante en l�nea recta
        rb.velocity = transform.up * speed;

        // 3. Se destruye autom�ticamente despu�s de 'lifeTime' segundos
        Destroy(gameObject, lifeTime);
    }

    // 4. Se destruye al chocar con algo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con el jugador, le hace da�o
        if (collision.gameObject.CompareTag("Player"))
        {
            // Usamos el '?' para evitar errores si el objeto no tiene el script
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        // Se destruye al chocar con cualquier cosa (excepto otros enemigos si quieres)
        // if (!collision.gameObject.CompareTag("Enemigo"))
        // {
        Destroy(gameObject);
        // }
    }
}