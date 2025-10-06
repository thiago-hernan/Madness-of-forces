using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Esta variable estática guardará el nombre de la última escena jugada.
    // Al ser 'static', su valor sobrevive entre cambios de escena.
    public static string previousSceneName;
}