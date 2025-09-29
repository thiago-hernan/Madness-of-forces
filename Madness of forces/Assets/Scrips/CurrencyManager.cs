using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public TextMeshProUGUI sugarText;
    public int currentSugar { get; private set; }

    [Header("UI Feedback")]
    public GameObject notificationObject;
    public float notificationTime = 2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        if (notificationObject != null)
        {
            notificationObject.SetActive(false);
        }
    }

    // ----> ESTA ES UNA DE LAS FUNCIONES QUE FALTABA <----
    public void AddSugar(int amount)
    {
        currentSugar += amount;
        UpdateUI();
    }

    // ----> ESTA ES LA OTRA FUNCIÓN QUE FALTABA <----
    public bool SpendSugar(int amount)
    {
        if (currentSugar >= amount)
        {
            currentSugar -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            ShowNotification();
            return false;
        }
    }

    private void UpdateUI()
    {
        if (sugarText != null)
        {
            sugarText.text = "Azúcar: " + currentSugar;
        }
    }

    public void ShowNotification()
    {
        if (notificationObject != null)
        {
            StopAllCoroutines();
            StartCoroutine(NotificationCoroutine());
        }
    }

    private IEnumerator NotificationCoroutine()
    {
        notificationObject.SetActive(true);
        yield return new WaitForSecondsRealtime(notificationTime);
        notificationObject.SetActive(false);
    }
}