using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : MonoBehaviour, IDañable
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public LayerMask capaObstaculos; // ¡Importante! Define qué es un obstáculo.

    [Header("Comportamiento de Vuelo")]
    public float radioOrbita = 7f;
    public float velocidadMovimiento = 5f;
    public float velocidadRotacion = 10f;

    [Header("Comportamiento de Combate")]
    public float tiempoEntreDisparos = 1.5f;
    private float cronometroDisparo;

    [Header("Evasión")]
    public float distanciaEvasion = 3f;

    [Header("Vida")]
    public int vida = 3; // 👈 vida del enemigo volador

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Los enemigos voladores no deben tener gravedad.
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        ManejarApuntado();
        ManejarDisparo();
    }

    void FixedUpdate()
    {
        if (jugador == null) return;

        ManejarMovimiento();
    }

    void ManejarApuntado()
    {
        Vector2 direccionHaciaJugador = jugador.position - transform.position;
        float angulo = Mathf.Atan2(direccionHaciaJugador.y, direccionHaciaJugador.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotacionObjetivo = Quaternion.AngleAxis(angulo, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);
    }

    void ManejarMovimiento()
    {
        // --- Cálculo de la órbita ---
        Vector2 vectorHaciaJugador = jugador.position - transform.position;
        float distancia = vectorHaciaJugador.magnitude;

        // Dirección para moverse en círculos (perpendicular al jugador)
        Vector2 direccionOrbita = new Vector2(-vectorHaciaJugador.y, vectorHaciaJugador.x).normalized;

        // Dirección para ajustar la distancia (acercarse o alejarse)
        Vector2 direccionAjusteDistancia = vectorHaciaJugador.normalized * (distancia - radioOrbita);

        // Combinamos ambas direcciones para obtener el movimiento deseado
        Vector2 direccionFinal = (direccionOrbita + direccionAjusteDistancia).normalized;

        // --- Evasión de obstáculos ---
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distanciaEvasion, capaObstaculos);
        Debug.DrawRay(transform.position, transform.up * distanciaEvasion, Color.red); // Para ver el rayo en la escena

        if (hit.collider != null)
        {
            // Si detecta un obstáculo, gira para evitarlo.
            Vector2 direccionEscape = new Vector2(hit.normal.y, -hit.normal.x);
            direccionFinal += direccionEscape;
        }

        // Aplicamos el movimiento
        rb.velocity = direccionFinal.normalized * velocidadMovimiento;
    }

    void ManejarDisparo()
    {
        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0)
        {
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
            cronometroDisparo = tiempoEntreDisparos;
        }
    }

    // 👇 Implementación de la interfaz IDañable
    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("Enemigo volador recibió " + cantidad + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        // Acá podés poner animación de muerte, partículas, etc.
        Destroy(gameObject);
    }
}
