using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text contMonedasText;  // Asegúrate de que este Text esté asignado en el Inspector
    private string savePath;
    private int monedasMax = 32;  // Número máximo de monedas que se pueden recolectar

    // Los datos temporales que se capturan al iniciar la escena
    private int initialVidas;
    private int initialRecolectados;
    private float initialSavedVolume;
    private int initialResolution;
    private int initialLanguage;
    private float initialHealthBarWarrior;

    private GameData data;

    private void Start()
    {
        // Establecer las monedas iniciales dependiendo de la escena
        SetInitialMonedas();

        // Actualizar el texto de las monedas
        UpdateMonedasText();

        // Captura los valores iniciales de las variables al iniciar la escena
        initialVidas = PlayerData.vidas;
        initialRecolectados = PlayerData.recolectados;
        initialSavedVolume = PlayerData.savedVolume;
        initialResolution = PlayerData.resolution;
        initialLanguage = PlayerData.language;
        initialHealthBarWarrior = PlayerData.healthBarWarrior;

        // Configura la ruta para guardar el archivo (usando la ruta persistente de la aplicación)
        savePath = Application.persistentDataPath + "/savegame.json";
    }

    // Guardar partida
    public void SaveGame()
    {
        // Crear un objeto con los datos iniciales guardados
        data = new GameData
        {
            sceneName = SceneManager.GetActiveScene().name, // Guarda la escena actual
            vidas = PlayerData.vidas, // Usa los valores capturados al inicio
            recolectados = PlayerData.recolectados,
            savedVolume = PlayerData.savedVolume,
            resolution = PlayerData.resolution,
            language = PlayerData.language,
            healthBarWarrior = PlayerData.healthBarWarrior
        };

        // Convertir los datos a formato JSON
        string json = JsonUtility.ToJson(data, true);

        // Sobrescribir el archivo savegame.json con los nuevos datos
        File.WriteAllText(savePath, json);

        Debug.Log("Juego guardado en: " + savePath);
        Debug.Log("Datos guardados: " + json); // Log de los datos guardados para verificar
    }

    // Cargar partida
    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            // Leer los datos del archivo JSON
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);

            // Cargar la escena guardada
            SceneManager.LoadScene(data.sceneName);

            // Restablecer el tiempo del juego para que no esté en pausa
            Time.timeScale = 1;  // Reanudar el juego (despausar)

            // Restaurar el volumen del juego
            AudioListener.volume = data.savedVolume;

            // Aquí puedes usar una corrutina o esperar que la escena se cargue para restaurar los datos del jugador
            StartCoroutine(LoadGameDataAfterSceneLoad());
        }
        else
        {
            Debug.LogWarning("No hay partida guardada.");
        }
    }

    // Método para cargar los datos después de cargar la escena
    private IEnumerator LoadGameDataAfterSceneLoad()
    {
        // Esperar hasta que la escena se haya cargado completamente
        yield return new WaitForEndOfFrame();

        // Restaurar los valores guardados (por ejemplo, vidas, recolectados, etc.)
        PlayerData.vidas = data.vidas;
        PlayerData.recolectados = data.recolectados;
        PlayerData.savedVolume = data.savedVolume;
        PlayerData.resolution = data.resolution;
        PlayerData.language = data.language;
        PlayerData.healthBarWarrior = data.healthBarWarrior;

        // Restaurar volumen después de cargar los datos
        if (PlayerData.savedVolume >= 0f && PlayerData.savedVolume <= 1f)
        {
            AudioListener.volume = PlayerData.savedVolume;
        }
        else
        {
            Debug.LogWarning("El volumen cargado es inválido, se usará el valor por defecto.");
            AudioListener.volume = 1f; // Volumen por defecto
        }

        Debug.Log("Datos de la partida cargados.");
    }
    
    void SetInitialMonedas()
    {
        // Obtener el nombre de la escena actual
        string sceneName = SceneManager.GetActiveScene().name;

        // Establecer las monedas iniciales según el nombre de la escena
        switch (sceneName)
        {
            case "RedScene":
                DataManager.recolectados = 0;  // Escena 1: 0 monedas
                break;
            case "BlueScene":
                DataManager.recolectados = 8;  // Escena 2: 8 monedas
                break;
            case "GreenScene":
                DataManager.recolectados = 16; // Escena 3: 16 monedas
                break;
            default:
                DataManager.recolectados = 0;  // Valor por defecto si no coincide con ninguna de las anteriores
                break;
        }
    }

    void UpdateMonedasText()
    {
        // Actualiza el texto con las monedas actuales y el máximo
        contMonedasText.text = DataManager.recolectados.ToString() + "/" + monedasMax.ToString();
    }

    // Método para recolectar una moneda y actualizar el texto
    public void RecolectarMoneda()
    {
        // Incrementa las monedas recolectadas
        DataManager.recolectados++;

        // Actualiza el texto de las monedas
        UpdateMonedasText();
    }
}



