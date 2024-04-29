using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    public static LevelControl instancia;
    public Button[] botonesNiveles;
    public int desbloqueados;


    public void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
    }

    public void Start()
    {
        if (botonesNiveles.Length > 0)
        {
            for (int i =0; i < botonesNiveles.Length; i++)
            {
                botonesNiveles[i].interactable = false;
            }

            for (int i = 0; i < PlayerPrefs.GetInt("NivelesDesbloqueados", 1); i++)
            {
                botonesNiveles[i].interactable = true;
            }
        }
    }

    public void AumentarNiveles()
    {
        if (desbloqueados > PlayerPrefs.GetInt("NivelesDesbloqueados", 1))
        {
            PlayerPrefs.SetInt("NivelesDesbloqueados", desbloqueados);
        }
    }
}
