using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameObject pausePanel; // Panel de pausa en el Canvas
    private bool isPaused = true; // Estado del juego
    public GameObject pauseButton; // Botón de pausa

    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // Reanuda el juego
            AudioListener.pause = false; // Reanuda el audio
            pausePanel.SetActive(false); // Oculta el panel
            pauseButton.SetActive(true); // Muestra el botón de pausa
        }
    }
}

