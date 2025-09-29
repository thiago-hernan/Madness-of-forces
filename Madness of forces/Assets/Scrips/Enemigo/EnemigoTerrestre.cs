using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoTerrestre : MonoBehaviour, IDañable
{
    // --- Variables Configurables ---
    [Header("Estadísticas")]
    public float velocidad = 3f;
    public float fuerzaSalto = 7f;
    public int vidaMaxima = 3; // NUEVO: La vida máxima, configurable por enemigo.

    [Header("Componentes")]
    public Transform gafas;
    public Transform jugador;
    private bool haMuerto = false;
    // --- Variables Internas ---
    private Rigidbody2D rb;
    private bool mirandoDerecha = true;
    private int vidaActual; // NUEVO: La vida actual que tiene el enemigo.
    public AudioClip sonidoMuerte;
    public int sugarDropAmount = 5;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }

        vidaActual = vidaMaxima; // NUEVO: Al empezar, la vida actual es igual a la máxima.
    }

    void FixedUpdate()
    {
        if (jugador == null) return;

        // 1. Decidir la dirección del movimiento
        float direccionHorizontal = jugador.position.x - transform.position.x;

        // 2. Moverse hacia el jugador
        rb.velocity = new Vector2(Mathf.Sign(direccionHorizontal) * velocidad, rb.velocity.y);

        // 3. Girar las gafas si es necesario
        if (direccionHorizontal > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (direccionHorizontal < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        gafas.localScale = new Vector3(gafas.localScale.x * -1, gafas.localScale.y, gafas.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].normal.x) > 0.9)
        {
            if (rb.velocity.y > -0.1f && rb.velocity.y < 0.1f)
            {
                rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
            }
        }
    }

    // --- NUEVO: Sistema de Vida y Muerte ---

    // Esta es la función que tu bala buscará y llamará.
    public void RecibirDaño(int cantidad)
    {
        if (haMuerto) return;
        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        haMuerto = true;

        if (sonidoMuerte != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
        }
        CurrencyManager.instance.AddSugar(sugarDropAmount);

        Destroy(gameObject);
    }
}