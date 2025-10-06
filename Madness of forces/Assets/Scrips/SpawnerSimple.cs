using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSimple : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject[] enemigosPrefabs;
    public float tiempoEntreSpawns = 3f;
    public float radioSpawn = 10f;

    void Start()
    {
        StartCoroutine(CicloDeSpawn());
    }

    IEnumerator CicloDeSpawn()
    {
        while (true)
        {
            // ----> CAMBIO REALIZADO AQUÍ <----
            // 1. AHORA, PRIMERO GENERA un enemigo al instante.
            SpawnearEnemigo();

            // 2. DESPUÉS, espera el tiempo definido antes de la siguiente ronda.
            yield return new WaitForSeconds(tiempoEntreSpawns);
        }
    }

    void SpawnearEnemigo()
    {
        if (enemigosPrefabs.Length == 0) return;
        GameObject enemigoElegido = enemigosPrefabs[Random.Range(0, enemigosPrefabs.Length)];
        if (enemigoElegido == null)
        {
            Debug.LogWarning("Espacio vacío en la lista de prefabs del Spawner.", this.gameObject);
            return;
        }
        Vector2 posicionAleatoria = Random.insideUnitCircle * radioSpawn;
        Vector3 posicionDeSpawn = transform.position + (Vector3)posicionAleatoria;
        Instantiate(enemigoElegido, posicionDeSpawn, Quaternion.identity);
    }

    // --- Funciones para el Survival Manager ---
    public void IncreaseDifficulty()
    {
        tiempoEntreSpawns *= 0.85f;
        if (tiempoEntreSpawns < 1.5f) { tiempoEntreSpawns = 1.5f; }
    }

    public void StopSpawning()
    {
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
    }
}