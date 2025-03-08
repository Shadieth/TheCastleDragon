using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EfectoSangre : MonoBehaviour
{
    public Image imagenSangre; // Asigna la imagen de sangre en el Inspector
    private float duracion = 0.1f; // Duración del efecto de sangre

    void Start()
    {
        if (imagenSangre != null)
            imagenSangre.color = new Color(imagenSangre.color.r, imagenSangre.color.g, imagenSangre.color.b, 0f); // Inicia invisible
    }

    public void ActivarSangre()
    {
        StartCoroutine(FadeSangre());
    }

    private IEnumerator FadeSangre()
    {
        // Aparece (fade-in)
        for (float t = 0; t < duracion; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 0.3f, t / duracion);
            imagenSangre.color = new Color(imagenSangre.color.r, imagenSangre.color.g, imagenSangre.color.b, alpha);
            yield return null;
        }

        // Espera un momento
        yield return new WaitForSeconds(0.2f);

        // Desaparece (fade-out)
        for (float t = 0; t < duracion; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duracion);
            imagenSangre.color = new Color(imagenSangre.color.r, imagenSangre.color.g, imagenSangre.color.b, alpha);
            yield return null;
        }
    }
}

