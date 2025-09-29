using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // NUEVO: Enum para definir nuestros tipos de armas de forma clara.
    public enum WeaponType { Normal, Potente, Escopeta, Rapida }

    [Header("Configuraci�n General")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerController playerController;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 1.5f;
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;
    private float fireTimer; // MODIFICADO: Ahora es un temporizador general.

    [Header("Gesti�n de Armas")]
    // NUEVO: Lista de armas que el jugador ha desbloqueado.
    public List<WeaponType> unlockedWeapons = new List<WeaponType>();
    // NUEVO: �ndice para saber qu� arma de la lista estamos usando.
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

        // Si el jugador hace clic izquierdo y el temporizador est� listo
        if (Input.GetMouseButtonDown(0) && fireTimer <= 0)
        {
            Shoot();
        }

        // NUEVO: L�gica para cambiar de arma con la rueda del rat�n.
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon(1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchWeapon(-1);
        }
    }

    // MODIFICADO: �Esta es la funci�n principal que ha cambiado!
    void Shoot()
    {
        // Obtenemos el tipo de arma actual de nuestra lista de desbloqueadas.
        WeaponType currentWeapon = unlockedWeapons[currentWeaponIndex];
        audioSource.PlayOneShot(sonidoDisparo); // El sonido se reproduce igual para todas.

        // Usamos un 'switch' para decidir qu� hacer seg�n el arma equipada.
        switch (currentWeapon)
        {
            // CASO 1: Arma Normal (TU C�DIGO ORIGINAL EST� AQU�)
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
                // Hacemos la bala m�s grande y r�pida para que se sienta potente
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
                        // Creamos una peque�a desviaci�n aleatoria para cada bala
                        Vector2 spread = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                        Vector2 direction = (playerController.lastMoveDirection + spread).normalized;
                        rbEscopeta.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
                    }
                    Destroy(bulletEscopeta, bulletLifeTime);
                }
                break;

            // CASO 4: Cadencia Alta
            case WeaponType.Rapida:
                fireTimer = 0.1f; // Cadencia muy r�pida
                GameObject bulletRapida = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                // Hacemos la bala m�s peque�a para que se sienta menos da�ina
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

    // NUEVO: Funci�n para cambiar entre las armas desbloqueadas.
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

    // NUEVO: Funci�n P�BLICA para que la tienda pueda desbloquear armas.
    public void UnlockWeapon(WeaponType weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
            Debug.Log("Arma desbloqueada: " + weapon);
        }
    }
}