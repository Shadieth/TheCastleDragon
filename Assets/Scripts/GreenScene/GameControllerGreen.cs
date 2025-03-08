using UnityEngine;

public class GameControllerGreen : MonoBehaviour
{
    private GameObject currentDragon; // Referencia al dragón actual
    public GameObject flecha;         // Referencia a la flecha
    private GameObject bloqueoFinal;  // Referencia al bloqueo final
    private Vector2 flechaPosInicial; // Guarda la posición inicial de la flecha
    private float speed = 10f;          // Velocidad del movimiento
    private float amplitude = 0.05f;    // Amplitud del movimiento

    void Start()
    {
        // Busca los objetos necesarios al inicio
        currentDragon = GameObject.Find("AdultGreenDragon");
        bloqueoFinal = GameObject.Find("BloqueoFinal");

        if (flecha != null)
        {
            flecha.GetComponent<SpriteRenderer>().enabled = false; // Oculta la flecha
            flechaPosInicial = flecha.transform.position; // Guarda la posición inicial
        }
        else
        {
            Debug.LogError("La flecha no está asignada en el GameController.");
        }
    }

    void Update()
    {
        // Verifica si el dragón ha sido destruido
        if (currentDragon == null)
        {
            Debug.Log("¡Has derrotado al dragón!");

            // Destruye el bloqueo final si existe
            if (bloqueoFinal != null)
            {
                Destroy(bloqueoFinal);
            }

            // Muestra la flecha y la hace oscilar en 2D
            if (flecha != null)
            {
                flecha.GetComponent<SpriteRenderer>().enabled = true;

                // Movimiento oscilante en el eje Y (2D)
                flecha.transform.position = new Vector2(flechaPosInicial.x, flechaPosInicial.y + Mathf.Sin(Time.time * speed) * amplitude);
            }
        }
    }
}
