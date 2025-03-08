using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject opciones;
    public GameObject comenzarJuego;


    void Start()
    {
        opciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void MostrarOpciones()
    {
        menuPrincipal.SetActive(false);
        opciones.SetActive(true);
    }

    public void VolverMenuPrincipal()
    {
        opciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void ComenzarJuego()
    {
        SceneManager.LoadScene("RedScene");
        Time.timeScale = 1f;
    }
}
