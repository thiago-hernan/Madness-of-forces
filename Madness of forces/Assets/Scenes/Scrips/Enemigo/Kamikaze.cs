using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class KamikazeEnemy2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public bool seekPlayerOnStart = true;

    [Header("Explosi�n / Da�o")]
    public int damage = 50;
    public GameObject explosionPrefab; // Part�culas/animaci�n de explosi�n (opcional)
    public float explosionDuration = 2f; // tiempo antes de destruir el prefab de explosi�n

    [Header("Autodestrucci�n")]
    public float lifeTime = 12f; // opcional: para destruir si no llega al jugador

    Rigidbody2D rb;
    Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (seekPlayerOnStart)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        if (lifeTime > 0f) Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            // intenta encontrar jugador cada cierto tiempo (robusto si el jugador aparece luego)
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
            else return;
        }

        Vector2 direction = (target.position - transform.position);
        rb.velocity = direction.normalized * speed;
        // opcional: rotar para mirar movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    // Recomendado: usar Collider2D con isTrigger = true para detecci�n de contacto.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicar da�o
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }

            Explode();
        }
        else
        {
            // Si quieres que colisione con otras capas y explote:
            // if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) Explode();
        }
    }

    // Alternativa si NO usas trigger (Collider2D.isTrigger = false):
    // void OnCollisionEnter2D(Collision2D collision) { if (collision.gameObject.CompareTag("Player")) { ... Explode(); } }

    void Explode()
    {
        // Instanciar FX
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, explosionDuration);
        }

        // Aqu� podr�as a�adir da�o por �rea usando Physics2D.OverlapCircleAll si quieres afectar a varios.
        Destroy(gameObject);
    }

    // M�todo p�blico para forzar la explosi�n desde otro script (por ejemplo, da�o por jugadores)
    public void ForceExplode()
    {
        Explode();
    }
}