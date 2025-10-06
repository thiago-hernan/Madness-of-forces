using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Enum para definir nuestros tipos de armas de forma clara.
    public enum WeaponType { Normal, Potente, Escopeta, Rapida }

    [Header("Configuración General")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerController playerController;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 1.5f;
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;
    private float fireTimer;

    [Header("Gestión de Armas")]
    // Lista de armas que el jugador ha desbloqueado en ESTA PARTIDA.
    public List<WeaponType> unlockedWeapons = new List<WeaponType>();
    private int currentWeaponIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Nos aseguramos de que el jugador siempre empiece con el arma normal.
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

        // Lógica para cambiar de arma con la rueda del ratón.
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon(1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchWeapon(-1);
        }
    }

    void Shoot()
    {
        // Obtenemos el tipo de arma actual de nuestra lista de desbloqueadas.
        WeaponType currentWeapon = unlockedWeapons[currentWeaponIndex];
        if (sonidoDisparo != null) audioSource.PlayOneShot(sonidoDisparo);

        // Usamos un 'switch' para decidir qué hacer según el arma equipada.
        switch (currentWeapon)
        {
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

            case WeaponType.Potente:
                fireTimer = 1.5f; // Cadencia muy lenta
                GameObject bulletPotente = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bulletPotente.transform.localScale = new Vector3(2, 2, 2);
                Rigidbody2D rbPotente = bulletPotente.GetComponent<Rigidbody2D>();
                if (rbPotente != null)
                {
                    rbPotente.AddForce(playerController.lastMoveDirection * (bulletSpeed * 1.5f), ForceMode2D.Impulse);
                }
                Destroy(bulletPotente, bulletLifeTime);
                break;

            case WeaponType.Escopeta:
                fireTimer = 0.8f; // Cadencia media
                for (int i = 0; i < 5; i++)
                {
                    GameObject bulletEscopeta = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rbEscopeta = bulletEscopeta.GetComponent<Rigidbody2D>();
                    if (rbEscopeta != null)
                    {
                        Vector2 spread = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                        Vector2 direction = (playerController.lastMoveDirection + spread).normalized;
                        rbEscopeta.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
                    }
                    Destroy(bulletEscopeta, bulletLifeTime);
                }
                break;

            case WeaponType.Rapida:
                fireTimer = 0.1f; // Cadencia muy rápida
                GameObject bulletRapida = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

    public void UnlockWeapon(WeaponType weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
            Debug.Log("Arma desbloqueada: " + weapon);
        }
    }
}