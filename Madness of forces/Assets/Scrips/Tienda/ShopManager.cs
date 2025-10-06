using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para los botones

public class ShopManager : MonoBehaviour
{
    [Header("Costos de Armas")]
    public int shotgunCost = 100;
    public int powerfulCost = 150;
    public int rapidCost = 200;

    [Header("Botones de la Tienda")]
    public Button shotgunButton;
    public Button powerfulButton;
    public Button rapidButton;

    [Header("Costos de Vida")]
    public int healthCost = 10;
    public int extraHeartCost = 50;

    
    private WeaponController weaponController;

    public void BuyHealthRecovery()
    {
      
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        // Si la compra es exitosa (tiene suficiente azúcar)
        if (playerHealth != null && CurrencyManager.instance.SpendSugar(healthCost))
        {
            playerHealth.RecoverAllHealth(); // Llama a la función para curarlo
            Debug.Log("Vida recuperada!");
        }
    }

    public void BuyExtraHeart()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null && CurrencyManager.instance.SpendSugar(extraHeartCost))
        {
            playerHealth.IncreaseMaxHealth(1); // Llama a la función para darle un corazón
            Debug.Log("¡Corazón extra comprado!");
        }
    }


    void Start()
    {
        // MODIFICADO: Buscamos el WeaponController del jugador al iniciar
        weaponController = FindObjectOfType<WeaponController>();
    }

    // --- Funciones para los botones de compra ---

    public void BuyShotgun()
    {
        if (CurrencyManager.instance.SpendSugar(shotgunCost))
        {
            // MODIFICADO: Llama a la función UnlockWeapon de tu script
            weaponController.UnlockWeapon(WeaponController.WeaponType.Escopeta);
            shotgunButton.interactable = false; // Desactiva el botón para no comprarla de nuevo
        }
    }

    public void BuyPowerful()
    {
        if (CurrencyManager.instance.SpendSugar(powerfulCost))
        {
            weaponController.UnlockWeapon(WeaponController.WeaponType.Potente);
            powerfulButton.interactable = false;
        }
    }

    public void BuyRapid()
    {
        if (CurrencyManager.instance.SpendSugar(rapidCost))
        {
            weaponController.UnlockWeapon(WeaponController.WeaponType.Rapida);
            rapidButton.interactable = false;
        }
    }

    // --- Función para el botón de cerrar ---

    public void CloseShop()
    {
        gameObject.SetActive(false); // Desactiva este panel
        Time.timeScale = 1f; // Reanuda el tiempo del juego
    }
}