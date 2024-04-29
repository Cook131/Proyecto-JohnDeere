using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class Cargando : MonoBehaviour
{
    public Slider slider;
    public Text texto;

    public GameObject panelCarga;
    // Start is called before the first frame update

    public void Carga(int siguienteEscena)
    {
        StartCoroutine(CargarEscena(siguienteEscena));
    }

    IEnumerator CargarEscena(int siguienteEscena)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(siguienteEscena);
        panelCarga.SetActive(true);

        while (!operacion.isDone)
        {
            float progreso = Mathf.Clamp01(operacion.progress / 0.9f);
            slider.value = progreso;
            texto.text = progreso * 100f + "%...";
            yield return null;
        }
    }
}
