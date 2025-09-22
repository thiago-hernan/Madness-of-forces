using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int daño = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Mensaje 1: Nos avisa que ha chocado con CUALQUIER COSA.
        Debug.Log("Bala chocó con: " + other.name);

        // Verificamos si chocamos con un objeto que tenga el tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Mensaje 2: Nos avisa que el objeto SÍ tiene el tag "Enemy".
            Debug.Log("¡Impacto en un Enemigo!");

            EnemigoKamikaze enemigo = other.GetComponent<EnemigoKamikaze>();

            if (enemigo != null)
            {
                enemigo.RecibirDaño(daño);
            }
            else
            {
                // Mensaje 3: Nos avisa si el objeto tiene el tag pero no el script.
                Debug.Log("ERROR: El objeto tiene tag 'Enemy' pero no el script 'EnemigoKamikaze'.");
            }

            Destroy(gameObject);
        }
    }
}