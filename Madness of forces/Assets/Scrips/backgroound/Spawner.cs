using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMejorado : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject[] enemigosPrefabs; // ¡Ahora puedes poner todos tus prefabs aquí!
    public float radioSpawn = 10f; // El radio donde aparecerán los enemigos.
    public float tiempoEntreSpawns = 2f;

    [Header("Límites y Activación")]
    public int maximoEnemigos = 15; // El número máximo de enemigos que este spawner mantendrá vivos.
    public float radioActivacion = 25f; // El spawner solo funcionará si el jugador está dentro de este radio.

    // Lista para mantener un registro de los enemigos que ha creado
    private List<GameObject> enemigosSpawneados = new List<GameObject>();
    private Transform jugador;

    void Start()
    {
        // Busca al jugador automáticamente por su tag
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // En lugar de InvokeRepeating, usamos una Coroutine para tener más control.
        StartCoroutine(CicloDeSpawn());
    }

    // Una Coroutine que se ejecuta en segundo plano
    IEnumerator CicloDeSpawn()
    {
        // Se ejecuta para siempre
        while (true)
        {
            // Limpiamos la lista de enemigos que ya fueron destruidos
            enemigosSpawneados.RemoveAll(enemigo => enemigo == null);

            // Calculamos la distancia al jugador
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            // CONDICIONES PARA SPAWNEAR:
            // 1. El jugador está cerca.
            // 2. No hemos alcanzado el límite de enemigos.
            if (distanciaAlJugador < radioActivacion && enemigosSpawneados.Count < maximoEnemigos)
            {
                SpawnearEnemigo();
            }

            // Espera el tiempo definido antes de volver a empezar el ciclo
            yield return new WaitForSeconds(tiempoEntreSpawns);
        }
    }

    void SpawnearEnemigo()
    {
        // 1. Elige un enemigo aleatorio de la lista de prefabs.
        int indiceAleatorio = Random.Range(0, enemigosPrefabs.Length);
        GameObject enemigoElegido = enemigosPrefabs[indiceAleatorio];

        // 2. Elige una posición aleatoria dentro del radio de spawn.
        Vector2 puntoAleatorioEnCirculo = Random.insideUnitCircle * radioSpawn;
        Vector3 posicionDeSpawn = transform.position + new Vector3(puntoAleatorioEnCirculo.x, puntoAleatorioEnCirculo.y, 0);

        // 3. Crea el enemigo en esa posición.
        GameObject nuevoEnemigo = Instantiate(enemigoElegido, posicionDeSpawn, Quaternion.identity);

        // 4. Añade el nuevo enemigo a nuestra lista para poder contarlo.
        enemigosSpawneados.Add(nuevoEnemigo);
    }

    // Esto es para que puedas ver los radios en el editor de Unity, ¡muy útil!
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioActivacion);
    }
}