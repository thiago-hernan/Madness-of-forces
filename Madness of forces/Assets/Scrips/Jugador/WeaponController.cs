using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // NUEVO: Enum para definir nuestros tipos de armas de forma clara.
    public enum WeaponType { Normal, Potente, Escopeta, Rapida }

    [Header("Configuración General")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerController playerController;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 1.5f;
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;
    private float fireTimer; // MODIFICADO: Ahora es un temporizador general.

    [Header("Gestión de Armas")]
    // NUEVO: Lista de armas que el jugador ha desbloqueado.
    public List<WeaponType> unlockedWeapons = new List<WeaponType>();
    // NUEVO: Índice para saber qué arma de la lista estamos usando.
    private int currentWeaponIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // NUEVO: Nos aseguramos de que el jugador siempre empiece con el arma normal.
        if (!unlockedWeapons.Contains(WeaponType.Normal))
        {
            unlockedWeapons.Add(WeaponType.Normal);
        }
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        // Si el jugador hace clic izquierdo y el temporizador está listo
        if (Input.GetMouseButtonDown(0) && fireTimer <= 0)
        {
            Shoot();
        }

        // NUEVO: Lógica para cambiar de arma con la rueda del ratón.
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon(1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchWeapon(-1);
        }
    }

    // MODIFICADO: ¡Esta es la función principal que ha cambiado!
    void Shoot()
    {
        // Obtenemos el tipo de arma actual de nuestra lista de desbloqueadas.
        WeaponType currentWeapon = unlockedWeapons[currentWeaponIndex];
        audioSource.PlayOneShot(sonidoDisparo); // El sonido se reproduce igual para todas.

        // Usamos un 'switch' para decidir qué hacer según el arma equipada.
        switch (currentWeapon)
        {
            // CASO 1: Arma Normal (TU CÓDIGO ORIGINAL ESTÁ AQUÍ)
            case WeaponType.Normal:
                fireTimer = 0.5f; // Cadencia de disparo
                GameObject bulletNormal = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rbNormal = bulletNormal.GetComponent<Rigidbody2D>();
                if (rbNormal != null)
                {
                    rbNormal.AddForce(playerController.lastMoveDirection * bulletSpeed, ForceMode2D.Impulse);
                }
                Destroy(bulletNormal, bulletLifeTime);
                break;

            // CASO 2: Arma Lenta y Potente
            case WeaponType.Potente:
                fireTimer = 1.5f; // Cadencia muy lenta
                GameObject bulletPotente = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                // Hacemos la bala más grande y rápida para que se sienta potente
                bulletPotente.transform.localScale = new Vector3(2, 2, 2);
                Rigidbody2D rbPotente = bulletPotente.GetComponent<Rigidbody2D>();
                if (rbPotente != null)
                {
                    rbPotente.AddForce(playerController.lastMoveDirection * (bulletSpeed * 1.5f), ForceMode2D.Impulse);
                }
                Destroy(bulletPotente, bulletLifeTime);
                break;

            // CASO 3: Escopeta
            case WeaponType.Escopeta:
                fireTimer = 0.8f; // Cadencia media
                // Creamos 5 balas en un bucle
                for (int i = 0; i < 5; i++)
                {
                    GameObject bulletEscopeta = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rbEscopeta = bulletEscopeta.GetComponent<Rigidbody2D>();
                    if (rbEscopeta != null)
                    {
                        // Creamos una pequeña desviación aleatoria para cada bala
                        Vector2 spread = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                        Vector2 direction = (playerController.lastMoveDirection + spread).normalized;
                        rbEscopeta.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
                    }
                    Destroy(bulletEscopeta, bulletLifeTime);
                }
                break;

            // CASO 4: Cadencia Alta
            case WeaponType.Rapida:
                fireTimer = 0.1f; // Cadencia muy rápida
                GameObject bulletRapida = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                // Hacemos la bala más pequeña para que se sienta menos dañina
                bulletRapida.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Rigidbody2D rbRapida = bulletRapida.GetComponent<Rigidbody2D>();
                if (rbRapida != null)
                {
                    rbRapida.AddForce(playerController.lastMoveDirection * bulletSpeed, ForceMode2D.Impulse);
                }
                Destroy(bulletRapida, bulletLifeTime);
                break;
        }
    }

    // NUEVO: Función para cambiar entre las armas desbloqueadas.
    void SwitchWeapon(int direction)
    {
        currentWeaponIndex += direction;
        if (currentWeaponIndex >= unlockedWeapons.Count)
        {
            currentWeaponIndex = 0;
        }
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = unlockedWeapons.Count - 1;
        }
        Debug.Log("Arma cambiada a: " + unlockedWeapons[currentWeaponIndex]);
    }

    // NUEVO: Función PÚBLICA para que la tienda pueda desbloquear armas.
    public void UnlockWeapon(WeaponType weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
            Debug.Log("Arma desbloqueada: " + weapon);
        }
    }
}