using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class IntroController : MonoBehaviour
{
    public GameObject text;

    void Start()
    {
        // Desactiva el texto al inicio
        text.gameObject.SetActive(false);

        // Inicia la corrutina para mostrar el texto después de medio segundo
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // Si se presiona la tecla espacio, cambia a la siguiente escena
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    IEnumerator ShowText()
    {
        // Espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);

        // Activa el texto
        text.gameObject.SetActive(true);
    }

    void LoadNextScene()
    {
        // Carga la siguiente escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

