using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Objeto Prefab de la bala
    public GameObject bulletPrefab;

    // Posición desde donde se disparan las balas (el cañón)
    public Transform firePoint;

    // Referencia al script del jugador para obtener la dirección
    public PlayerController playerController;

    // Tiempo entre disparos
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    // Velocidad de la bala
    public float bulletSpeed = 10f;

    // Tiempo de vida de la bala
    public float bulletLifeTime = 1.5f;

    void Update()
    {
        // Si el jugador hace clic izquierdo y ha pasado el tiempo de espera
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Instancia la bala en la posición del firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Obtiene el Rigidbody2D de la bala para aplicarle fuerza
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Usa la última dirección de movimiento del jugador para disparar la bala
            rb.AddForce(playerController.lastMoveDirection * bulletSpeed, ForceMode2D.Impulse);
        }

        // Destruye la bala después de un tiempo definido
        Destroy(bullet, bulletLifeTime);
    }
}