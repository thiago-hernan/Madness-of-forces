using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int daño = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ya no buscamos 'EnemigoKamikaze' o 'EnemigoTerrestre'.
        // Buscamos CUALQUIER componente que cumpla con el contrato 'IDañable'.
        IDañable enemigo = other.GetComponent<IDañable>();

        // Si el objeto con el que chocamos tiene un componente que se puede dañar...
        if (enemigo != null)
        {
            // ...le decimos que reciba daño, sin importar qué tipo de enemigo sea.
            enemigo.RecibirDaño(daño);

            // Destruimos la bala después de impactar
            Destroy(gameObject);
        }
    }
}