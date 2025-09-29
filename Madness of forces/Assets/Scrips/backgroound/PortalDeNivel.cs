using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // O using UnityEngine.UI;

public class PortalDeNivel : MonoBehaviour // <- Asegúrate de que este nombre coincida con el del archivo
{
    [Tooltip("El nombre exacto de la escena que quieres cargar.")]
    public string nombreDeLaEscenaACargar;

    [Tooltip("El objeto de UI que contiene el mensaje a mostrar.")]
    public GameObject mensajeUI;

    private bool jugadorAdentro = false;

    void Start()
    {
        if (mensajeUI != null)
        {
            mensajeUI.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorAdentro && Input.GetKeyDown(KeyCode.H))
        {
            // Comprobamos que el nombre de la escena no esté vacío antes de intentar cargarla
            if (!string.IsNullOrEmpty(nombreDeLaEscenaACargar))
            {
                SceneManager.LoadScene(nombreDeLaEscenaACargar);
            }
            else
            {
                Debug.LogWarning("El nombre de la escena a cargar no está definido en el portal.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAdentro = true;
            if (mensajeUI != null)
            {
                mensajeUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAdentro = false;
            if (mensajeUI != null)
            {
                mensajeUI.SetActive(false);
            }
        }
    }
}