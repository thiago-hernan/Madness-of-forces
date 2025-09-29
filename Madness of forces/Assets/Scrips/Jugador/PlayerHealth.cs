using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Invencibilidad")]
    public float invincibilityDuration = 2f;
    private bool isInvincible = false;

    [Header("Feedback Visual y Sonido")]
    public AudioClip hurtSound;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [Header("Referencias UI")]
    public UIHealthDisplay healthDisplay; // Referencia al script de la UI

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Inicializa la UI con los corazones llenos
        if (healthDisplay != null)
        {
            healthDisplay.SetupHearts(maxHealth);
        }
    }

    // Esta funci�n ser� llamada por los enemigos
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Si es invencible, no hace nada

        currentHealth -= damage;
        audioSource.PlayOneShot(hurtSound);

        // Actualiza la UI
        if (healthDisplay != null)
        {
            healthDisplay.UpdateHearts(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Si a�n tiene vida, activa la invencibilidad
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Bucle de parpadeo
        float endTime = Time.time + invincibilityDuration;
        while (Time.time < endTime)
        {
            // Alterna la visibilidad del sprite
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f); // Velocidad del parpadeo
        }

        // Al terminar, asegura que el jugador sea visible y ya no sea invencible
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto. GAME OVER.");

        // ----> A�ADE ESTA L�NEA AQU� <----
        // Guardamos el nombre de la escena actual en nuestra variable est�tica.
        GameSession.previousSceneName = SceneManager.GetActiveScene().name;

        // Carga la escena de Game Over
        SceneManager.LoadScene("Game over");
    }

    // Detecta la colisi�n con los enemigos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto con el que choc� tiene la etiqueta "Enemigo"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // Pierde 1 de vida
        }
    }
}