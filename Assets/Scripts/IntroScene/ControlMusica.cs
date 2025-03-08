using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlMusica : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;  // Mixer de audio
    [SerializeField] private Toggle toggleMute;      // Toggle para mutear
    [SerializeField] private Slider sliderMusica;    // Slider para controlar el volumen
    public GameObject mute;
    public GameObject noMute;

    private const string VOLUME_KEY = "VolumenMusica";   // Clave para guardar el volumen
    private const string MUTE_KEY = "MusicaMute";        // Clave para guardar el estado de mute
    private const float MIN_VOLUME_DB = -80f;           // Volumen mínimo en decibeles

    void Start()
    {
        // Cargar valores guardados
        float volumen = PlayerPrefs.GetFloat(VOLUME_KEY, 0.5f);
        bool isMuted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;

        // Aplicar valores a UI
        sliderMusica.value = volumen;
        toggleMute.isOn = isMuted;

        // Aplicar volumen inicial
        ApplyVolume(volumen, isMuted);
    }

    void Update()
    {
        DataManager.savedVolume = audioMixer.GetFloat("VolumenMusica", out float dB) ? Mathf.Pow(10, dB / 20) : 0.5f;
        
        // Si está muteado, no permitir cambios de volumen
        if (toggleMute.isOn)
        {
            ApplyVolume(sliderMusica.value, true);
            return;
        } 

        // Aplicar cambios al mover el slider
        ApplyVolume(sliderMusica.value, false);
    }

    private void ApplyVolume(float value, bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat("VolumenMusica", MIN_VOLUME_DB);
        }
        else
        {
            float dB = (value > 0.0001f) ? Mathf.Log10(value) * 20 : MIN_VOLUME_DB;
            audioMixer.SetFloat("VolumenMusica", dB);
            PlayerPrefs.SetFloat(VOLUME_KEY, value);
        }

        // Guardar estado de mute
        PlayerPrefs.SetInt(MUTE_KEY, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleMute()
    {
        ApplyVolume(sliderMusica.value, toggleMute.isOn);
    }
}

