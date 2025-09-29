using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSimple : MonoBehaviour
{
    [Header("Configuraci�n")]
    public GameObject[] enemigosPrefabs; // Arrastra aqu� a todos tus enemigos (Kamikaze, Volador, etc.)
    public float tiempoEntreSpawns = 3f; // Tiempo inicial entre cada enemigo
    public float radioSpawn = 10f;       // Distancia m�xima a la que pueden aparecer

    void Start()
    {
        // Inicia el ciclo de spawn de forma continua
        StartCoroutine(CicloDeSpawn());
    }

    IEnumerator CicloDeSpawn()
    {
        // Este bucle se repetir� para siempre mientras el spawner est� activo
        while (true)
        {
            // 1. Espera el tiempo definido
            yield return new WaitForSeconds(tiempoEntreSpawns);

            // 2. Llama a la funci�n para crear un enemigo
            SpawnearEnemigo();
        }
    }

    void SpawnearEnemigo()
    {
        // Si la lista est� completamente vac�a, no hace nada.
        if (enemigosPrefabs.Length == 0) return;

        GameObject enemigoElegido = enemigosPrefabs[Random.Range(0, enemigosPrefabs.Length)];

        // ----> NUEVA COMPROBACI�N <----
        // Si el prefab elegido est� vac�o (es un "None"), muestra un aviso y no contin�a.
        if (enemigoElegido == null)
        {
            Debug.LogWarning("Se intent� spawnear un enemigo, pero un espacio en la lista de prefabs del Spawner est� vac�o.", this.gameObject);
            return; // Sale de la funci�n para no causar un error.
        }

        // Elige una posici�n aleatoria en un c�rculo alrededor del spawner
        Vector2 posicionAleatoria = Random.insideUnitCircle * radioSpawn;
        Vector3 posicionDeSpawn = transform.position + (Vector3)posicionAleatoria;

        // Crea el enemigo en esa posici�n
        Instantiate(enemigoElegido, posicionDeSpawn, Quaternion.identity);
    }

    // --- Funciones para que el Survival Manager lo controle ---

    public void IncreaseDifficulty()
    {
        // Reduce el tiempo de espera un 15%, haci�ndolo m�s r�pido
        tiempoEntreSpawns *= 0.85f;

        // Se asegura de que el tiempo no baje de un l�mite razonable
        if (tiempoEntreSpawns < 0.5f)
        {
            tiempoEntreSpawns = 0.5f;
        }
    }

    public void StopSpawning()
    {
        // La forma m�s f�cil de detener el spawner es desactivarlo
        this.enabled = false;
    }

    // Dibuja un c�rculo en el editor para que veas el radio de spawn
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
    }
}