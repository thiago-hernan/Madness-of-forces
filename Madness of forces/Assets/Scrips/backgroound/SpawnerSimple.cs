using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSimple : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject[] enemigosPrefabs; // Arrastra aquí a todos tus enemigos (Kamikaze, Volador, etc.)
    public float tiempoEntreSpawns = 3f; // Tiempo inicial entre cada enemigo
    public float radioSpawn = 10f;       // Distancia máxima a la que pueden aparecer

    void Start()
    {
        // Inicia el ciclo de spawn de forma continua
        StartCoroutine(CicloDeSpawn());
    }

    IEnumerator CicloDeSpawn()
    {
        // Este bucle se repetirá para siempre mientras el spawner esté activo
        while (true)
        {
            // 1. Espera el tiempo definido
            yield return new WaitForSeconds(tiempoEntreSpawns);

            // 2. Llama a la función para crear un enemigo
            SpawnearEnemigo();
        }
    }

    void SpawnearEnemigo()
    {
        // Si la lista está completamente vacía, no hace nada.
        if (enemigosPrefabs.Length == 0) return;

        GameObject enemigoElegido = enemigosPrefabs[Random.Range(0, enemigosPrefabs.Length)];

        // ----> NUEVA COMPROBACIÓN <----
        // Si el prefab elegido está vacío (es un "None"), muestra un aviso y no continúa.
        if (enemigoElegido == null)
        {
            Debug.LogWarning("Se intentó spawnear un enemigo, pero un espacio en la lista de prefabs del Spawner está vacío.", this.gameObject);
            return; // Sale de la función para no causar un error.
        }

        // Elige una posición aleatoria en un círculo alrededor del spawner
        Vector2 posicionAleatoria = Random.insideUnitCircle * radioSpawn;
        Vector3 posicionDeSpawn = transform.position + (Vector3)posicionAleatoria;

        // Crea el enemigo en esa posición
        Instantiate(enemigoElegido, posicionDeSpawn, Quaternion.identity);
    }

    // --- Funciones para que el Survival Manager lo controle ---

    public void IncreaseDifficulty()
    {
        // Reduce el tiempo de espera un 15%, haciéndolo más rápido
        tiempoEntreSpawns *= 0.85f;

        // Se asegura de que el tiempo no baje de un límite razonable
        if (tiempoEntreSpawns < 0.5f)
        {
            tiempoEntreSpawns = 0.5f;
        }
    }

    public void StopSpawning()
    {
        // La forma más fácil de detener el spawner es desactivarlo
        this.enabled = false;
    }

    // Dibuja un círculo en el editor para que veas el radio de spawn
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
    }
}