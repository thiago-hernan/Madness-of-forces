using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SurvivalManager : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float totalTime = 180f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("Sistema de Dificultad")]
    public float difficultyInterval = 20f;
    private float nextDifficultyIncrease;
    public SpawnerSimple enemySpawner; // Asegúrate que el nombre del script del spawner coincida

    [Header("Condición de Victoria")]
    // MODIFICADO: Ahora es una referencia a un objeto en la escena, no un prefab.
    public GameObject winCubeObject;
    private bool hasWon = false;

    void Start()
    {
        currentTime = totalTime;
        nextDifficultyIncrease = totalTime - difficultyInterval;

        // MODIFICADO: Nos aseguramos de que el cubo esté desactivado al empezar.
        if (winCubeObject != null)
        {
            winCubeObject.SetActive(false);
        }
    }

    void Update()
    {
        // ... (Esta función no necesita cambios) ...
        if (hasWon) return;
        if (Time.timeScale > 0 && currentTime > 0) { currentTime -= Time.deltaTime; }
        UpdateTimerUI();
        if (currentTime > 0 && currentTime <= nextDifficultyIncrease)
        {
            if (enemySpawner != null) enemySpawner.IncreaseDifficulty();
            nextDifficultyIncrease -= difficultyInterval;
        }
        if (currentTime <= 0 && !hasWon)
        {
            WinGame();
        }
    }

    void UpdateTimerUI()
    {
        // ... (Esta función no necesita cambios) ...
        if (timerText != null)
        {
            if (currentTime > 0)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                timerText.text = "RAPIDO, LA SALIDA YA ESTA DISPONIBLE, CORREEEE";
            }
        }
    }

    void WinGame()
    {
        hasWon = true;
        Debug.Log("¡HAS SOBREVIVIDO!");

        if (enemySpawner != null) enemySpawner.StopSpawning();

        // ----> LÓGICA PRINCIPAL MODIFICADA <----
        // En lugar de Instanciar, ahora activamos el objeto que ya existe.
        if (winCubeObject != null)
        {
            winCubeObject.SetActive(true);
        }
    }
}