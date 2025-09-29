using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �Muy importante para cambiar de escena!

public class MenuManager : MonoBehaviour
{
    // Esta funci�n la llamar� el bot�n "Play"
    public void IniciarSandbox()
    {
        // Carga la escena que se llama "Sandbox". Aseg�rate de que el nombre es exacto.
        SceneManager.LoadScene("Sandbox");
    }

    // Esta funci�n la llamar� el bot�n "Quit"
    public void SalirDelJuego()
    {
        // Escribe un mensaje en la consola para saber que funciona en el editor
        Debug.Log("Saliendo del juego...");

        // Cierra la aplicaci�n (solo funciona cuando el juego est� compilado, no en el editor de Unity)
        Application.Quit();
    }
}