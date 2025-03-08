using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private int resolutionIndex;

    private void Start()
    {
        // Configurar resoluciones
        List<string> options = new List<string>
        {
            "Full HD (1920x1080)",
            "QHD (2560x1440)",
            "4K UHD (3840x2160)"
        };

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        // Cargar configuración guardada
        int savedResolution = PlayerPrefs.GetInt("resolution", 0);
        resolutionDropdown.value = savedResolution;
        SetResolution(savedResolution);


    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                resolutionIndex = 0;
                break;
            case 1:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                resolutionIndex = 1;
                break;
            case 2:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                resolutionIndex = 2;
                break;
        }

         
        PlayerPrefs.SetInt("resolution", index);
    }

    private void Update()
    {
        DataManager.resolution = PlayerPrefs.GetInt("resolution", resolutionIndex);
    }

}


