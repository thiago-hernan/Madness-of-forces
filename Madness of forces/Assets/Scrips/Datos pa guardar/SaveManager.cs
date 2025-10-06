using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Necesario para Enum.Parse

public static class SaveManager
{
    private const string UnlockedWeaponsKey = "UnlockedWeapons";

    // Guarda la lista de armas desbloqueadas
    public static void SaveUnlockedWeapons(List<WeaponController.WeaponType> weapons)
    {
        // Convertimos la lista de armas a un solo string, separado por comas (ej: "Normal,Escopeta,Potente")
        string weaponsString = string.Join(",", weapons);
        PlayerPrefs.SetString(UnlockedWeaponsKey, weaponsString);
        PlayerPrefs.Save(); // Guarda los datos en el disco
        Debug.Log("Armas guardadas: " + weaponsString);
    }

    // Carga la lista de armas guardadas
    public static List<WeaponController.WeaponType> LoadUnlockedWeapons()
    {
        List<WeaponController.WeaponType> unlockedWeapons = new List<WeaponController.WeaponType>();

        if (PlayerPrefs.HasKey(UnlockedWeaponsKey))
        {
            string weaponsString = PlayerPrefs.GetString(UnlockedWeaponsKey);
            string[] weaponNames = weaponsString.Split(',');

            foreach (string name in weaponNames)
            {
                // Convertimos el nombre (string) de vuelta a nuestro tipo de arma (enum)
                if (Enum.TryParse(name, out WeaponController.WeaponType weaponType))
                {
                    unlockedWeapons.Add(weaponType);
                }
            }
        }

        Debug.Log("Armas cargadas: " + string.Join(",", unlockedWeapons));
        return unlockedWeapons;
    }
}