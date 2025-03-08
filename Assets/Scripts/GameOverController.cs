using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void salir()
    {
        Application.Quit(); // Cierra la aplicación en un build
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Detiene la ejecución en el editor de Unity
        #endif
    }

    public void volvrMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipalScene");
    }
}
