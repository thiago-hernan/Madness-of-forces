using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ¡Importante para poder usar TextMeshPro!

public class ShopTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject shopPanel;
    public TextMeshProUGUI interactionPrompt; // El cartel de "Pulsa H..."

    // Variable para saber si el jugador está dentro de la zona
    private bool playerIsInside = false;

    void Start()
    {
        // Nos aseguramos de que el cartel esté oculto al empezar
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    // Se ejecuta cuando el jugador entra en el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInside = true;
            interactionPrompt.gameObject.SetActive(true); // Muestra el cartel
        }
    }

    // Se ejecuta cuando el jugador sale del trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInside = false;
            interactionPrompt.gameObject.SetActive(false); // Oculta el cartel
        }
    }

    // Se ejecuta en cada fotograma
    void Update()
    {
        // Si el jugador está dentro Y presiona la tecla 'H'...
        if (playerIsInside && Input.GetKeyDown(KeyCode.H))
        {
            OpenShop();
        }
    }

    void OpenShop()
    {
        shopPanel.SetActive(true);    // ...abre el panel de la tienda
        Time.timeScale = 0f;          // ...y pausa el juego.
        interactionPrompt.gameObject.SetActive(false); // Ocultamos el cartel mientras compra
    }
}