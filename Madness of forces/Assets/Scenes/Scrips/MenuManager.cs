using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ¡Muy importante para cambiar de escena!

public class MenuManager : MonoBehaviour
{
    // Esta función la llamará el botón "Play"
    public void IniciarSandbox()
    {
        // Carga la escena que se llama "Sandbox". Asegúrate de que el nombre es exacto.
        SceneManager.LoadScene("Sandbox");
    }

    // Esta función la llamará el botón "Quit"
    public void SalirDelJuego()
    {
        // Escribe un mensaje en la consola para saber que funciona en el editor
        Debug.Log("Saliendo del juego...");

        // Cierra la aplicación (solo funciona cuando el juego está compilado, no en el editor de Unity)
        Application.Quit();
    }
}