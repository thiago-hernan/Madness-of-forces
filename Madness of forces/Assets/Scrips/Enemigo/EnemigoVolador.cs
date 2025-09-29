using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : MonoBehaviour, IDañable
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public LayerMask capaObstaculos;

    // NUEVO: Campo para el sonido de muerte en el Inspector
    public AudioClip sonidoMuerte;

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
    public int vida = 3;

    private Rigidbody2D rb;
    private bool haMuerto = false; // NUEVO: Bandera para controlar el estado de muerte

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Si rb es null, es porque falta el componente. Mostramos un error claro.
        if (rb == null)
        {
            Debug.LogError("¡Falta el componente Rigidbody2D en el enemigo volador!", this.gameObject);
            return; // Detenemos la ejecución para evitar más errores
        }

        rb.gravityScale = 0;
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // MODIFICADO: Si está muerto o no hay jugador, no hace nada.
        if (haMuerto || jugador == null) return;

        ManejarApuntado();
        ManejarDisparo();
    }

    void FixedUpdate()
    {
        // MODIFICADO: Si está muerto o no hay jugador, no hace nada.
        if (haMuerto || jugador == null) return;

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
        Vector2 vectorHaciaJugador = jugador.position - transform.position;
        float distancia = vectorHaciaJugador.magnitude;
        Vector2 direccionOrbita = new Vector2(-vectorHaciaJugador.y, vectorHaciaJugador.x).normalized;
        Vector2 direccionAjusteDistancia = vectorHaciaJugador.normalized * (distancia - radioOrbita);
        Vector2 direccionFinal = (direccionOrbita + direccionAjusteDistancia).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distanciaEvasion, capaObstaculos);
        Debug.DrawRay(transform.position, transform.up * distanciaEvasion, Color.red);

        if (hit.collider != null)
        {
            Vector2 direccionEscape = new Vector2(hit.normal.y, -hit.normal.x);
            direccionFinal += direccionEscape;
        }

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

    public void RecibirDaño(int cantidad)
    {
        if (haMuerto) return; // Si ya está muerto, no puede recibir más daño

        vida -= cantidad;
        Debug.Log("Enemigo volador recibió " + cantidad + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        haMuerto = true; // MODIFICADO: Activamos la bandera de muerte

        // NUEVO: Reproduce el sonido en la posición actual antes de destruir el objeto
        if (sonidoMuerte != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
        }

        Destroy(gameObject);
    }
}