using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : MonoBehaviour, IDañable
{
    // Enum para definir los estados del enemigo
    private enum State { Attacking, Evading }
    private State currentState;

    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public LayerMask capaObstaculos;

    [Header("Estadísticas de Combate")]
    public int vida = 3;
    public int sugarDropAmount = 7; // Variable de azúcar que faltaba
    public float tiempoEntreDisparos = 1.5f;
    public AudioClip sonidoMuerte;

    [Header("Estadísticas de Vuelo")]
    public float velocidadMovimiento = 5f;
    public float velocidadRotacion = 10f;
    public float radioOrbita = 7f;

    [Header("Estadísticas de Evasión")]
    public float distanciaEvasion = 3f;
    public float duracionEvasion = 0.5f; // Tiempo que pasará esquivando
    private float evadeTimer;

    // Componentes y variables privadas
    private Rigidbody2D rb;
    private bool haMuerto = false;
    private float cronometroDisparo;
    private Vector2 moveDirection; // Dirección de movimiento calculada en Update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Búsqueda de jugador más robusta
        if (jugador == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                jugador = playerObject.transform;
            }
            else
            {
                // Si aún no encuentra al jugador, se desactiva para no dar errores
                Debug.LogError("Enemigo Volador no pudo encontrar al Jugador. Desactivando.", this.gameObject);
                this.enabled = false;
                return;
            }
        }

        // El enemigo siempre empieza atacando
        currentState = State.Attacking;
    }

    void Update()
    {
        if (haMuerto || jugador == null) return;

        // El cerebro del enemigo: decide qué hacer según su estado actual
        switch (currentState)
        {
            case State.Attacking:
                HandleAttackingState();
                break;
            case State.Evading:
                HandleEvadingState();
                break;
        }
    }

    void FixedUpdate()
    {
        if (haMuerto || jugador == null) return;

        // FixedUpdate solo se encarga de aplicar el movimiento físico
        rb.velocity = moveDirection.normalized * velocidadMovimiento;
    }

    // Lógica cuando está en modo ATAQUE
    private void HandleAttackingState()
    {
        // 1. Apuntar al jugador
        ApuntarAlJugador();

        // 2. Comprobar si hay obstáculos
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distanciaEvasion, capaObstaculos);
        if (hit.collider != null)
        {
            // Si hay un obstáculo, cambia al modo ESQUIVAR
            currentState = State.Evading;
            evadeTimer = duracionEvasion;
            return;
        }

        // 3. Calcular movimiento de órbita
        Vector2 dirHaciaJugador = jugador.position - transform.position;
        float distancia = dirHaciaJugador.magnitude;

        // Vector para moverse en círculos (perpendicular al jugador)
        Vector2 dirOrbita = new Vector2(-dirHaciaJugador.y, dirHaciaJugador.x).normalized;
        // Vector para ajustar la distancia y mantenerse en el radio de órbita
        Vector2 dirAjusteDistancia = dirHaciaJugador.normalized * (distancia - radioOrbita);

        // Combina ambas fuerzas para un movimiento fluido y lo guarda para FixedUpdate
        moveDirection = (dirOrbita + dirAjusteDistancia);

        // 4. Disparar
        ManejarDisparo();
    }

    // Lógica cuando está en modo ESQUIVAR
    private void HandleEvadingState()
    {
        // Mientras esquiva, simplemente se mueve hacia atrás para alejarse del obstáculo
        moveDirection = -transform.up;

        // Reduce el temporizador de evasión
        evadeTimer -= Time.deltaTime;
        if (evadeTimer <= 0)
        {
            // Cuando se acaba el tiempo, vuelve al modo ATAQUE
            currentState = State.Attacking;
        }
    }

    private void ApuntarAlJugador()
    {
        Vector2 direccion = jugador.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotacionObjetivo = Quaternion.AngleAxis(angulo, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);
    }

    private void ManejarDisparo()
    {
        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0)
        {
            // El enemigo ahora solo tiene que crear la bala. La bala hará el resto.
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
            cronometroDisparo = tiempoEntreDisparos;
        }
    }

    public void RecibirDaño(int cantidad)
    {
        if (haMuerto) return;
        vida -= cantidad;
        if (vida <= 0)
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