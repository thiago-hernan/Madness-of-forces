using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    public Sprite corazonLleno;
    public Sprite corazonVacio;
    public Image prefabCorazonUI;
    private List<Image> corazones = new List<Image>();

    public void SetupHearts(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            AddHeartContainer();
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            corazones[i].sprite = (i < currentHealth) ? corazonLleno : corazonVacio;
        }
    }

    public void AddHeartContainer()
    {
        Image nuevoCorazon = Instantiate(prefabCorazonUI, transform);
        corazones.Add(nuevoCorazon);
    }
}