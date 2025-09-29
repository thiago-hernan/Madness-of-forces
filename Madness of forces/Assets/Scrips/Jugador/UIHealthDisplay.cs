using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

public class UIHealthDisplay : MonoBehaviour
{
    [Header("Sprites de Corazón")]
    public Sprite corazonLleno;   // El sprite "Corazon"
    public Sprite corazonVacio;   // El sprite "CorazonRoto"

    [Header("UI")]
    public Image prefabCorazonUI; // Un prefab de una imagen de UI

    private List<Image> corazones = new List<Image>();

    // Crea los contenedores de corazón iniciales
    public void SetupHearts(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            Image nuevoCorazon = Instantiate(prefabCorazonUI, transform);
            corazones.Add(nuevoCorazon);
        }
    }

    // Actualiza los sprites de los corazones según la vida actual
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            if (i < currentHealth)
            {
                corazones[i].sprite = corazonLleno;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }
}