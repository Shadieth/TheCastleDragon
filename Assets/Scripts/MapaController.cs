using UnityEngine;
using UnityEngine.UI; // Para el uso de Image

public class MapaController : MonoBehaviour
{
    public Image mapaRed;
    public Image mapaBlue;
    public Image mapaGreen;
    public int mapaActual;

    void Start()
    {
        // Si el mapa es el actual, lo dejamos con opacidad 1 (totalmente visible)
        // Los demás mapas se vuelven más opacos (ejemplo: 0.5f)
        SetOpacity(mapaRed, mapaActual == 1);
        SetOpacity(mapaBlue, mapaActual == 2);
        SetOpacity(mapaGreen, mapaActual == 3);
    }

    void SetOpacity(Image mapa, bool isActive)
    {
        if (mapa != null)
        {
            Color colorActual = mapa.color;
            colorActual.a = isActive ? 1f : 0.1f; // El activo es 1, los demás 0.5
            mapa.color = colorActual;
        }
    }
}

