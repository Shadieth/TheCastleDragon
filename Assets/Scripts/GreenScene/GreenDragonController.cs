using UnityEngine;

public class GreenDragonController : MonoBehaviour
{
    private float speed = 1f; // Velocidad del movimiento del dragón
    private float distance = 0.5f; // Distancia que se moverá hacia arriba y abajo
    public GameObject firePrefab; // Prefab del fuego
    public Transform fireSpawnPoint; // Punto de aparición del fuego
    private float fireSpeed = 10f; // Velocidad del fuego
    private float fireRate = 1f; // Frecuencia de disparo (en segundos)

    private Vector3 startPosition; // Posición inicial del dragón
    private float nextFireTime = 1f; // Control del tiempo para disparar
    private int fireDirectionIndex = 0; // Índice para cambiar dirección de disparo
    private Vector2[] fireDirections = new Vector2[]
    {
        new Vector2(0, -1),  // Abajo
        new Vector2(1, 0),   // Derecha
        new Vector2(0, 1),   // Arriba
        new Vector2(-1, 0)   // Izquierda
    };

    private float[] fireRotations = new float[]
    {
        180f,    // Abajo 
        -90f,  // Derecha
        0f,  // Arriba (sin rotación)
        90f    // Izquierda
    };

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimiento oscilante en el eje Y
        float yMovement = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        transform.position = startPosition + new Vector3(0, yMovement, 0);

        // Disparar automáticamente en secuencia cada 'fireRate' segundos
        if (Time.time >= nextFireTime)
        {
            ShootFire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootFire()
    {
        // Obtener la dirección actual de disparo
        Vector2 fireDirection = fireDirections[fireDirectionIndex];
        float fireRotation = fireRotations[fireDirectionIndex];

        // Instanciar el prefab del fuego en el punto de disparo con la rotación adecuada
        GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.Euler(0, 0, fireRotation));

        // Aplicar velocidad al fuego en la dirección seleccionada
        Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = fireDirection * fireSpeed;
        }

        // Avanzar al siguiente índice para cambiar dirección en el próximo disparo
        fireDirectionIndex = (fireDirectionIndex + 1) % fireDirections.Length;

        // Destruir el fuego después de 3 segundos para evitar acumulación
        Destroy(fire, 0.5f);
    }
}

