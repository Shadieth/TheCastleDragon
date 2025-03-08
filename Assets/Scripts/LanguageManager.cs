using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;
    private Translations translations;

    // Aquí tienes ejemplos de campos que representan los textos que deseas traducir.
    public TMP_Text opcionesText;
    public TMP_Text resolutionText;
    public TMP_Text musicaFondoText;
    public TMP_Text languageText;
    public TMP_Text empezarNuevaPartidaText;
    public TMP_Text cargarPartidaExistenteText;
    public TMP_Text menuPrincipalText;
    public TMP_Text salirText;
    public TMP_Text options;

    void Start()
    {
        AddLanguageOptions();  // Añadir las opciones de idioma al Dropdown
        LoadTranslations();    // Cargar las traducciones desde el archivo JSON
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged); // Escuchar los cambios del dropdown
    }
    void Update()
    {
        DataManager.language = languageDropdown.value;
    }

    // Añadir las opciones "Español" e "Inglés" al Dropdown dinámicamente
    void AddLanguageOptions()
    {
        // Limpiar las opciones actuales, en caso de que haya otras
        languageDropdown.ClearOptions();

        // Crear una lista de opciones con los idiomas
        var languageOptions = new System.Collections.Generic.List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Español"),
            new TMP_Dropdown.OptionData("Inglés")
        };

        // Añadir las opciones al dropdown
        languageDropdown.AddOptions(languageOptions);

        // Establecer el idioma predeterminado (Español en este caso)
        languageDropdown.value = 0;  // 0 es el índice de "Español"
    }

    void LoadTranslations()
    {
        // Cargar el archivo JSON desde Resources
        TextAsset json = Resources.Load<TextAsset>("languages");
        if (json != null)
        {
            // Deserializar el JSON a la clase Translations
            translations = JsonUtility.FromJson<Translations>(json.text);

            SetLanguage("es"); // Establecer idioma por defecto (español)
        }
        else
        {
            Debug.LogError("El archivo de traducciones no se encontró en Resources.");
        }
    }

    public void OnLanguageChanged(int index)
    {
        // Cambiar el idioma según el valor seleccionado en el Dropdown
        string selectedLanguage = index == 0 ? "es" : "en";
        SetLanguage(selectedLanguage);
    }

    void SetLanguage(string language)
    {
        // Seleccionar las traducciones correspondientes al idioma elegido
        Texts currentLanguage = language == "es" ? translations.es : translations.en;
        UpdateUI(currentLanguage); // Actualizar los textos en la UI
    }

    void UpdateUI(Texts currentLanguage)
    {
        // Aquí solo se actualizan los textos de los elementos TMP_Text
        opcionesText.text = currentLanguage.options;
        resolutionText.text = currentLanguage.resolution;
        musicaFondoText.text = currentLanguage.background_music;
        languageText.text = currentLanguage.language;
        empezarNuevaPartidaText.text = currentLanguage.empezarNuevaPartidaText;
        cargarPartidaExistenteText.text = currentLanguage.cargarPartidaExistenteText;
        menuPrincipalText.text = currentLanguage.menuPrincipalText;
        salirText.text = currentLanguage.salirText;
        options.text = currentLanguage.options;
    }
}



[System.Serializable]
public class Texts
{
    public string options;
    public string resolution;
    public string language;
    public string background_music;
    public string pistaEnemigo;
    public string textPistaEnemigo;
    public string pistaWarriorText;
    public string textPistaWarrior;
    public string recorridoMapaText;
    public string juegoPausadoText;
    public string cargarPartidaText;
    public string guardarPartidaText;
    public string salirText;
    public string empezarNuevaPartidaText;
    public string cargarPartidaExistenteText;
    public string menuPrincipalText;
    public string panelYouWin;
    public string panelGameOver;
    public string volverMenuPrincipal;
    //Textos únicos de la escena Blue
    public string pistaEnemigoBlue;
    public string textPistaEnemigoBlue;
    public string pistaBabyDragonBlue;
    public string textPistaBabyDragonBlue;
    public string textPistaWarriorBlue;
    //Textos únicos de la escena Green
    public string textPistaEnemigoGreen;
    public string pistaFuegoGreen;
    public string textPistaFuegoGreen;
}

[System.Serializable]
public class Translations
{
    public Texts es;  // Traducción para español
    public Texts en;  // Traducción para inglés
}








