using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar eventos del mouse

public class BotonInteractivo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Efectos Visuales")]
    [Tooltip("Cu�nto crecer� el bot�n. 1.1 es un 10% m�s grande.")]
    public float factorDeAgrandamiento = 1.1f;
    private Vector3 escalaOriginal;

    [Header("Efectos de Sonido")]
    [Tooltip("El sonido que se reproducir� al pasar el mouse por encima.")]
    public AudioClip sonidoHover;
    private AudioSource audioSource;

    void Start()
    {
        // Guardamos la escala inicial del bot�n
        escalaOriginal = transform.localScale;

        // Buscamos el componente AudioSource o lo a�adimos si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Nos aseguramos de que el sonido no se reproduzca al iniciar la escena
        audioSource.playOnAwake = false;
    }

    // Esta funci�n se llama cuando el puntero del mouse entra en el bot�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Agrandamos el bot�n
        transform.localScale = escalaOriginal * factorDeAgrandamiento;

        // Reproducimos el sonido si est� asignado
        if (sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover);
        }
    }

    // Esta funci�n se llama cuando el puntero del mouse sale del bot�n
    public void OnPointerExit(PointerEventData eventData)
    {
        // Devolvemos el bot�n a su tama�o original
        transform.localScale = escalaOriginal;
    }
}