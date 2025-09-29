using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public string victorySceneName = "Victoria"; // Nombre de tu escena de victoria

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ¡Importante! Reanuda el tiempo por si quieres animaciones en la escena de victoria
            Time.timeScale = 1f;
            SceneManager.LoadScene(victorySceneName);
        }
    }
}