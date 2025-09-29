using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoKamikaze : MonoBehaviour, IDañable
{
    // Variables que puedes ajustar desde el Inspector de Unity
    public Transform jugador;      // Para saber a quién seguir.
    public float velocidad = 4f;   // Qué tan rápido se mueve.
    public int vida = 2;           // Los golpes que aguanta antes de destruirse.
    public AudioClip sonidoMuerte;

    private Rigidbody2D rb;

    // Start se ejecuta una sola vez al principio
    void Start()
    {
        // Obtenemos el componente Rigidbody2D para poder moverlo con físicas
        rb = GetComponent<Rigidbody2D>();

        // Busca automáticamente al jugador por su "Tag" si no lo asignaste manualmente
        if (jugador == null)
        {
            GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
            if (jugadorGO != null)
            {
                jugador = jugadorGO.transform;
            }
            else
            {
                Debug.LogError("¡No se encontró ningún objeto con el tag 'Player' en la escena!");
                // Desactivamos el enemigo si no hay jugador para evitar errores
                this.enabled = false;
            }
        }
    }

    // FixedUpdate es ideal para cálculos de física como el movimiento
    void FixedUpdate()
    {
        if (jugador == null) return; // Si no hay jugador, no hace nada.

        // --- Lógica de Movimiento y Rotación ---

        // 1. Calcular la dirección hacia el jugador
        // Restamos la posición del jugador menos nuestra posición para obtener el vector que nos une
        Vector2 direccion = (Vector2)jugador.position - rb.position;
        direccion.Normalize(); // Normalizamos para que la velocidad sea constante

        // 2. Mover el enemigo hacia esa dirección
        // Aplicamos una velocidad constante al Rigidbody
        rb.velocity = direccion * velocidad;

        // 3. Calcular el ángulo para apuntar
        // Usamos Atan2 para obtener el ángulo en radianes y lo convertimos a grados
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 4. Aplicar la rotación
        // Le restamos 90 grados porque en Unity, los sprites "apuntan hacia arriba" (eje Y) por defecto.
        // Esto alinea la "punta" de la cápsula con la dirección de movimiento.
        rb.rotation = angulo - 90f;
    }

    // --- Lógica de Vida y Muerte ---

    // Esta es una función PÚBLICA, lo que significa que otros scripts (como el de la bala) pueden llamarla.
    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad; // Restamos la vida

        if (vida <= 0)
        {
            Morir(); // Si la vida llega a 0 o menos, llamamos a la función de morir
        }
    }

    void Morir()
    {
        if (sonidoMuerte != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
        }

        // Ahora sí, destruye el objeto del enemigo
        Destroy(gameObject);
    }
}