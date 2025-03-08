using UnityEngine;
using UnityEngine.Audio;

public class MantenerValores : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Mixer de audio

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("VolumenMusica", Mathf.Log10(DataManager.savedVolume) * 20); // Configurar el volumen
        audioMixer.SetFloat("VolumenEfectos", Mathf.Log10(DataManager.savedVolume) * 20); // Configurar el volumen
        PlayerPrefs.SetInt("resolution", DataManager.resolution);
    }

}
