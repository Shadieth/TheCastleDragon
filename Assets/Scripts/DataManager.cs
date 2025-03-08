using UnityEngine;
using Image = UnityEngine.UI.Image;

public class DataManager : MonoBehaviour
{
    public static int vidas;
    public static int recolectados;
    public static float savedVolume = 1f; // Volumen por defecto en 100%
    public static int resolution;
    internal static int language;
    public static float healthBarWarrior;
    public static Image[] corazones;
}

