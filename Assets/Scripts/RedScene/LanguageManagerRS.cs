using UnityEngine;
using TMPro;

public class LanguageManagerRS : MonoBehaviour
{
    private Translations translations;

    // Referencias a los textos de la UI
    public TMP_Text pistaEnemigoText;
    public TMP_Text textPistaEnemigo;
    public TMP_Text pistaWarriorText;
    public TMP_Text textPistaWarrior;
    public TMP_Text recorridoMapaText;
    public TMP_Text juegoPausadoText;
    public TMP_Text cargarPartidaText;
    public TMP_Text guardarPartidaText;
    public TMP_Text salirText;
    public TMP_Text panelGameOver;
    public TMP_Text volverMenuPrincipalGO;
    public TMP_Text salirTextGO;
    public TMP_Text cargarPartidaTextGO;

    void Start()
    {
        LoadTranslations(); // Cargar traducciones
        ApplySavedLanguage(); // Aplicar el idioma seleccionado
    }

    void LoadTranslations()
    {
        // Cargar el archivo JSON desde Resources
        TextAsset json = Resources.Load<TextAsset>("languages");
        if (json != null)
        {
            // Deserializar el JSON a la clase Translations
            translations = JsonUtility.FromJson<Translations>(json.text);
        }
        else
        {
            Debug.LogError("El archivo de traducciones no se encontró en Resources.");
        }
    }

    void ApplySavedLanguage()
    {
        if (translations == null)
        {
            Debug.LogError("Las traducciones no se han cargado correctamente.");
            return;
        }

        // Determinar el idioma basado en DataManager.language
        string selectedLanguage = DataManager.language == 0 ? "es" : "en";
        SetLanguage(selectedLanguage);
    }

    void SetLanguage(string language)
    {
        // Seleccionar las traducciones correspondientes
        Texts currentLanguage = language == "es" ? translations.es : translations.en;
        UpdateUI(currentLanguage); // Actualizar los textos en la UI
    }

    void UpdateUI(Texts currentLanguage)
    {
        // Actualizar textos en la UI
        pistaEnemigoText.text = currentLanguage.pistaEnemigo;
        textPistaEnemigo.text = currentLanguage.textPistaEnemigo;
        pistaWarriorText.text = currentLanguage.pistaWarriorText;
        textPistaWarrior.text = currentLanguage.textPistaWarrior;
        recorridoMapaText.text = currentLanguage.recorridoMapaText;
        juegoPausadoText.text = currentLanguage.juegoPausadoText;
        cargarPartidaText.text = currentLanguage.cargarPartidaText;
        guardarPartidaText.text = currentLanguage.guardarPartidaText;
        salirText.text = currentLanguage.salirText; 
        panelGameOver.text = currentLanguage.panelGameOver;
        volverMenuPrincipalGO.text = currentLanguage.volverMenuPrincipal;
        salirTextGO.text = currentLanguage.salirText;
        cargarPartidaTextGO.text = currentLanguage.cargarPartidaText;
    }
}


