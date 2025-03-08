using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class WarriorController : MonoBehaviour
{
    public GameObject canvasHud;
    private float maxHealth = 100f; // Salud del personaje
    private float damageAmount = 20f; // Daño al ser herido
    private float currentHealth; // Salud actual del personaje
    [SerializeField] private Image healthBar; // Referencia al Image del Healthbar
    public float velocity; // Velocidad del personaje
    public Animator animator; // Referencia al Animator
    public LiveManager liveManager; // Referencia al LifeManager
    private Rigidbody2D rb; // Referencia al Rigidbody2D
    private float pushBackAmount = 0.8f; // Distancia a mover hacia atrás (ajustable)
    private bool isKnockedBack = false; // Evita múltiples retrocesos simultáneos
    int contMonedas = 0; //Contador para saber cuantas veces ha sido herida con el dragon
    private int monedasMax = 30; //Máximo de monedas posibles
    public TMP_Text contMonedasText; //Texto para mostrar cantidad de monedas
    private float attackTime = 0; // Tiempo acumulado de "atacando"
    private float requiredAttackTime = 1f; // Tiempo necesario para destruir el objeto
    private bool isCollidingWithDragon = false; // Variable para saber si está colisionando con el dragón
    private GameObject currentDragon; // Referencia al dragón actual
    public AudioSource coinSound; // Referencia al sonido de recolectar monedas
    public GameObject panelGameOver; // Referencia al panel de Game Over
    public EfectoSangre efectoSangre; // Arrastra el objeto con el script EfectoSangre en el Inspector
    public GameObject efectoSangrePanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtén el Rigidbody2D del personaje
        contMonedasText.text = DataManager.recolectados.ToString() + "/" + monedasMax.ToString();
        currentHealth = maxHealth; // Establece la salud actual al máximo
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
        if (isCollidingWithDragon && animator.GetBool("atacando") && contMonedas == 8)
        {
            attackTime += Time.deltaTime; // Acumula tiempo en cada frame

            if (attackTime >= requiredAttackTime) // Si el tiempo acumulado es suficiente
            {
                Destroy(currentDragon); // Destruye el dragón
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            // Solo queremos manejar la colisión con el objeto que tenga el tag "Dragon"
        if (collision.gameObject.CompareTag("Dragon") && animator.GetBool("atacando") && contMonedas == 8)
        {
            isCollidingWithDragon = true; // Marca que está colisionando con el dragón
            currentDragon = collision.gameObject; // Guarda la referencia al dragón actual
        
        }

        if (collision.gameObject.CompareTag("Agua") && !isKnockedBack) // Detecta colisión con la poción
        {
            animator.SetTrigger("herido");
    
            // Calcula la dirección opuesta al punto de colisión
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            // Mueve al personaje hacia atrás por una pequeña distancia
            transform.position = new Vector2(transform.position.x + direction.x * pushBackAmount,
                                             transform.position.y + direction.y * pushBackAmount);

            // Marca que ya fue empujado para evitar múltiples retrocesos
            isKnockedBack = true;

            // Restablecer el retroceso después de un breve momento
            StartCoroutine(ResetKnockback());
            TakeDamage();
        }  
    }

    // Método para restaurar la capacidad de ser empujado después de un breve tiempo
    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f); // Tiempo que dura el retroceso (ajustable)
        isKnockedBack = false; // Permite que se pueda empujar de nuevo
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
            Debug.Log(contMonedas);
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
            SceneManager.LoadScene("BlueScene");
            
        }
        
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth; // Normaliza el valor
    }

}
