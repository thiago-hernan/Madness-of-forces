using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public UIHealthDisplay healthDisplay;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (healthDisplay != null)
        {
            healthDisplay.SetupHearts(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        currentHealth -= damage;
        audioSource.PlayOneShot(hurtSound);
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
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void RecoverAllHealth()
    {
        currentHealth = maxHealth;
        healthDisplay.UpdateHearts(currentHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        healthDisplay.AddHeartContainer();
        healthDisplay.UpdateHearts(currentHealth);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float endTime = Time.time + invincibilityDuration;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto. GAME OVER.");
        GameSession.previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Game over");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}