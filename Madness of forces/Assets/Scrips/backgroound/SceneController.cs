using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        // Si se presiona la tecla 'R', reinicia el último nivel jugado.
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Comprobamos si hay un nombre de escena guardado.
            if (!string.IsNullOrEmpty(GameSession.previousSceneName))
            {
                // Si lo hay, cargamos esa escena.
                SceneManager.LoadScene(GameSession.previousSceneName);
            }
            else
            {
                // Si por alguna razón no hay nada guardado, cargamos una escena por defecto.
                SceneManager.LoadScene("Sandbox");
            }
        }
    }
}