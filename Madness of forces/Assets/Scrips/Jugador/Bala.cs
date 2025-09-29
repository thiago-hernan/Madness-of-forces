using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int da�o = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ya no buscamos 'EnemigoKamikaze' o 'EnemigoTerrestre'.
        // Buscamos CUALQUIER componente que cumpla con el contrato 'IDa�able'.
        IDa�able enemigo = other.GetComponent<IDa�able>();

        // Si el objeto con el que chocamos tiene un componente que se puede da�ar...
        if (enemigo != null)
        {
            // ...le decimos que reciba da�o, sin importar qu� tipo de enemigo sea.
            enemigo.RecibirDa�o(da�o);

            // Destruimos la bala despu�s de impactar
            Destroy(gameObject);
        }
    }
}