using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Esta variable est�tica guardar� el nombre de la �ltima escena jugada.
    // Al ser 'static', su valor sobrevive entre cambios de escena.
    public static string previousSceneName;
}