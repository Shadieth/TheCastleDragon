using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class WarriorControllerGreen : MonoBehaviour
{
    public GameObject canvasHud;
    private float maxHealth = 100f; // Salud del personaje
    private float damageAmount = 20f; // Daño al ser herido
    private float currentHealth; // Salud actual del personaje
        [SerializeField] private Image healthBar; // Referencia al Image del Healthbar
    public float velocity; // Velocidad del personaje
    public Animator animator; // Referencia al Animator
    public LiveManagerGreen liveManager; // Referencia al LifeManager
    private Rigidbody2D rb; // Referencia al Rigidbody2D
    int contFuego = 0; //Contador para saber cuantas veces ha sido herida con el agua
    int contMonedas; //Contador para saber cuantas veces ha sido herida con el dragon
    int monedasMax = 32; //Máximo de monedas posibles
    public TMP_Text contMonedasText; //Texto para mostrar cantidad de monedas
    private float attackTime = 0; // Tiempo acumulado de "atacando"
    private float requiredAttackTime = 1f; // Tiempo necesario para destruir el objeto
    private bool isCollidingWithDragon = false; // Variable para saber si está colisionando con el dragón
    private GameObject currentDragon; // Referencia al dragón actual
    private GameObject currentDragon_; // Referencia al dragón actual
    public AudioSource coinSound; // Referencia al sonido de recolectar monedas
    public GameObject panelYouWin; // Referencia al panel final
    private int vidasActuales;
    public GameObject panelGameOver; // Referencia al panel de Game Over
    public GameObject antesDePasarEscena;
    public EfectoSangre efectoSangre; // Arrastra el objeto con el script EfectoSangre en el Inspector
    public GameObject efectoSangrePanel;

    void Start()
    {
        vidasActuales = DataManager.vidas;
        rb = GetComponent<Rigidbody2D>(); // Obtén el Rigidbody2D del personaje
        contMonedasText.text = DataManager.recolectados.ToString() + "/" + monedasMax.ToString();
        contMonedas = DataManager.recolectados;
        currentHealth = DataManager.healthBarWarrior;
        UpdateHealthBar();
    }

    void Update()
    {
        currentDragon_ = GameObject.Find("AdultGreenDragon");
        DataManager.healthBarWarrior = currentHealth;
        DataManager.vidas = liveManager.CurrentLives();
        DataManager.recolectados = contMonedas;

        // Establecer velocidad de movimiento
        float velocityX = Input.GetAxis("Horizontal") * Time.deltaTime * velocity;
        float velocityY = Input.GetAxis("Vertical") * Time.deltaTime * velocity;

        // Establecer animaciones
        animator.SetFloat("movementX", velocityX);
        animator.SetFloat("movementY", velocityY);

        // Animación de ataque
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("atacando", true);
        }
        else
        {
            animator.SetBool("atacando", false);
        }

        // Cambiar la dirección del sprite según el movimiento horizontal
        if (velocityX < 0)
        {
            transform.localScale = new Vector3(-0.7114583f, 0.7114583f, 0.7114583f);
        }
        else if (velocityX > 0)
        {
            transform.localScale = new Vector3(0.7114583f, 0.7114583f, 0.7114583f);
        }

        // Mover al personaje
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + velocityX, position.y + velocityY, position.z);

        // Si está en colisión con el dragón y cumple las condiciones
        if (isCollidingWithDragon && animator.GetBool("atacando") && contMonedas == 30)
        {
            attackTime += Time.deltaTime; // Acumula tiempo en cada frame

            if (attackTime >= requiredAttackTime) // Si el tiempo acumulado es suficiente
            {
                Destroy(currentDragon); // Destruye el dragón
            }
        }

        if (contMonedas == 30 && currentDragon_ == null) 
        {
            Destroy(antesDePasarEscena);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            // Solo queremos manejar la colisión con el objeto que tenga el tag "Dragon"
        if (collision.gameObject.CompareTag("Dragon") && animator.GetBool("atacando") && contMonedas == 30)
        {
            isCollidingWithDragon = true; // Marca que está colisionando con el dragón
            currentDragon = collision.gameObject; // Guarda la referencia al dragón actual
        }

        if (collision.gameObject.CompareTag("fuegoSuelo")) // Detecta colisión con la poción
        {
            contFuego += 1;
            animator.SetTrigger("herido");
            TakeDamage();
        }  
        if (collision.gameObject.CompareTag("Dragon") && contMonedas < 30) // Detecta colisión con el dragón
        {
            animator.SetTrigger("herido");
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (liveManager.CurrentLives() > 0)
        {
            efectoSangre.ActivarSangre(); // Activa el efecto de sangre
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Evita valores negativos
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
        currentHealth = maxHealth;
        // Llama a LifeManager para restar una vida
        liveManager.LoseLife();
        }
        

        if (liveManager.CurrentLives() <= 0)
        {
        Time.timeScale = 0;
        AudioListener.pause = true; // Pausa el audios
        canvasHud.SetActive(false);
        efectoSangrePanel.SetActive(false);
        panelGameOver.SetActive(true);
        }
    }

    private void OnTriggerEnter2D( Collider2D other)
    {
        if (other.gameObject.CompareTag("Moneda")) // Detecta colisión con la moneda
        {
            contMonedas += 1;
            //Destruye la moneda
            Destroy(other.gameObject);
            //Actualiza el contador de monedas
            contMonedasText.text = contMonedas.ToString() + "/" + monedasMax.ToString();
            coinSound.Play();
        }

        if (other.gameObject.CompareTag("Fuego")) // Detecta colisión con el fuego
        {
            animator.SetTrigger("golpeFuego");
            TakeDamage();
            //Destruye el fuego
            Destroy(other.gameObject);
        } 

         if (other.gameObject.CompareTag("pasarEscena") && contMonedas == 30 && currentDragon_ == null) // Detecta colisión con el portal
        {
            Time.timeScale = 0;
            AudioListener.pause = true; // Pausa el audios
            canvasHud.SetActive(false);
            panelYouWin.SetActive(true); 
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth; // Normaliza el valor
    }

}

