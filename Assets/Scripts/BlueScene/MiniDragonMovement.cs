using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour
{
    private float speed = 1f; // Velocidad de movimiento
    public float waitTime = 2f; // Tiempo antes de cambiar de dirección
    private Vector2 targetPosition;
    private float minX, maxX, minY, maxY;

    private float limitMarginY = 2f; // Reduce el área de movimiento del eje Y
    private float limitMarginX = 4f; // Reduce el área de movimiento del eje X

    void Start()
    {
        // Definir los límites de la pantalla
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Reducir el área de movimiento agregando un margen
        minX = bottomLeft.x + limitMarginX;
        maxX = topRight.x - limitMarginX;
        minY = bottomLeft.y + limitMarginY;
        maxY = topRight.y - limitMarginY;

        StartCoroutine(MoveRandomly());
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // Elegir un nuevo destino dentro de los nuevos límites
            targetPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Mover hasta la nueva posición
            while ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Esperar un poco antes de elegir un nuevo destino
            yield return new WaitForSeconds(waitTime);
        }
    }
}


