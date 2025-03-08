using UnityEngine;

public class WhiteDragonController : MonoBehaviour
{
    public GameObject firePrefab; // Prefab del fuego
    public Transform fireSpawnPoint; // Punto de aparición del fuego
    private float fireSpeed = 3f; // Velocidad del fuego (en el eje y, hacia abajo)
    private float fireRate = 2f; // Frecuencia de disparo (en segundos)

    private float nextFireTime = 0f; // Control del tiempo para disparar

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

