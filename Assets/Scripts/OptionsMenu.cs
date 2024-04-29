using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer audioMixer;
    public void PantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void Volumen(float volum)
    {
        audioMixer.SetFloat("Volumen", volum);
    }

    public void Calidad(int calidadIndex)
    {
        QualitySettings.SetQualityLevel(calidadIndex);
    }
}
