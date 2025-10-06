using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar eventos del mouse

public class BotonInteractivo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Efectos Visuales")]
    [Tooltip("Cuánto crecerá el botón. 1.1 es un 10% más grande.")]
    public float factorDeAgrandamiento = 1.1f;
    private Vector3 escalaOriginal;

    [Header("Efectos de Sonido")]
    [Tooltip("El sonido que se reproducirá al pasar el mouse por encima.")]
    public AudioClip sonidoHover;
    private AudioSource audioSource;

    void Start()
    {
        // Guardamos la escala inicial del botón
        escalaOriginal = transform.localScale;

        // Buscamos el componente AudioSource o lo añadimos si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Nos aseguramos de que el sonido no se reproduzca al iniciar la escena
        audioSource.playOnAwake = false;
    }

    // Esta función se llama cuando el puntero del mouse entra en el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Agrandamos el botón
        transform.localScale = escalaOriginal * factorDeAgrandamiento;

        // Reproducimos el sonido si está asignado
        if (sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover);
        }
    }

    // Esta función se llama cuando el puntero del mouse sale del botón
    public void OnPointerExit(PointerEventData eventData)
    {
        // Devolvemos el botón a su tamaño original
        transform.localScale = escalaOriginal;
    }
}