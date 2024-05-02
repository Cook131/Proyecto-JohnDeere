using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlEscena : MonoBehaviour
{
    // Start is called before the first frame update
    public void Escena(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Win(bool meta)
    {
        SceneManager.LoadScene("Win");
    }
}
