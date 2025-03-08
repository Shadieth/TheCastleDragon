using UnityEngine;

public class RedDragonController : MonoBehaviour
{
    private float speed = 1f; // Velocidad del movimiento del dragón
    private float distance = 1f; // Distancia que se moverá hacia arriba y abajo
    public GameObject firePrefab; // Prefab del fuego
    public Transform fireSpawnPoint; // Punto de aparición del fuego
    private float fireSpeed = 5f; // Velocidad del fuego (en el eje y, hacia abajo)
    private float fireRate = 1f; // Frecuencia de disparo (en segundos)

    private Vector3 startPosition; // Posición inicial del dragón
    private float nextFireTime = 0f; // Control del tiempo para disparar

    // Start is called before the first frame update
    void Start()
    {
        // Guardar la posición inicial del dragón
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento oscilante en el eje y
        float yMovement = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        transform.position = startPosition + new Vector3(0, yMovement, 0);

        // Disparar automáticamente cada 'fireRate' segundos
        if (Time.time >= nextFireTime)
        {
            ShootFire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootFire()
    {
        // Instanciar el prefab del fuego en el punto de disparo
        GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);

        // Añadir velocidad al fuego hacia abajo
        Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(0, -fireSpeed); // Mover el fuego hacia abajo (eje y negativo)
        }

        // Destruir el fuego después de 3 segundos para evitar acumulación
        Destroy(fire, 3f);
    }
}
