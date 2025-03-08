using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class WarriorControllerBlue : MonoBehaviour
{
    public GameObject canvasHud;
    private float maxHealth = 100f; // Salud del personaje
    private float damageAmount = 20f; // Daño al ser herido
    private float currentHealth; // Salud actual del personaje
    [SerializeField] private Image healthBar; // Referencia al Image del Healthbar
    public float velocity; // Velocidad del personaje
    public Animator animator; // Referencia al Animator
    public LiveManagerBlue liveManager; // Referencia al LifeManager
    private Rigidbody2D rb; // Referencia al Rigidbody2D
    int contAttackMini = 0; //Contador para saber cuantas veces ha sido herida con el agua
    int contMonedas; //Contador para saber cuantas veces ha sido herida con el dragon
    private int monedasMax = 30; //Máximo de monedas posibles
    public TMP_Text contMonedasText; //Texto para mostrar cantidad de monedas
    private GameObject currentDragon; // Referencia al dragón actual
    public AudioSource coinSound; // Referencia al sonido de recolectar monedas
    public GameObject panelGameOver; // Referencia al panel de Game Over
    private int vidasActuales;
    public EfectoSangre efectoSangre; // Arrastra el objeto con el script EfectoSangre en el Inspector
    public GameObject efectoSangrePanel;

    void Start()
    {
        vidasActuales = DataManager.vidas;
        rb = GetComponent<Rigidbody2D>(); // Obtén el Rigidbody2D del personaje
        contMonedasText.text = DataManager.recolectados.ToString() + "/" + monedasMax.ToString();
        contMonedas = DataManager.recolectados;
        currentDragon = GameObject.Find("AdultWhiteDragon");
        currentHealth = DataManager.healthBarWarrior;
        UpdateHealthBar();
    }

    void Update()
    {
        DataManager.vidas = liveManager.CurrentLives();
        DataManager.recolectados = contMonedas;
        DataManager.healthBarWarrior = currentHealth;

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
        if (contMonedas == 14)
        {
            Destroy(currentDragon); // Destruye el dragón
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

         if (other.gameObject.CompareTag("pasarEscena")) // Detecta colisión con el portal
        {
            //Pasar a la siguiente escena
            SceneManager.LoadScene("GreenScene");
            
        }

        if (other.gameObject.CompareTag("miniDragon")) // Detecta colisión con la poción
        {
            animator.SetTrigger("herido");
            TakeDamage();
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dragon"))
        {
            animator.SetTrigger("herido");
            TakeDamage();
        }
        
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth; // Normaliza el valor
    }

}

