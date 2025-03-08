using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pausePanel; // Panel de pausa en el Canvas
    private bool isPaused = false; // Estado del juego
    public GameObject pauseButton; // Botón de pausa

    private void Start() {
        pausePanel.SetActive(false); // Oculta el panel de pausa al inicio
    }

    public void TogglePause()
    {
        isPaused = true; // Cambia el estado del juego

        if (isPaused)
        {
            Time.timeScale = 0f; // Congela el juego
            AudioListener.pause = true; // Pausa el audios
            pausePanel.SetActive(true); // Muestra el panel de pausa
            pauseButton.SetActive(false); // Oculta el botón de pausa
        }
        
    }
}
