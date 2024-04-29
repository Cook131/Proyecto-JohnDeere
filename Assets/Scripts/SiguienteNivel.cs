using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SiguienteNivel : MonoBehaviour
{
    // Start is called before the first frame update
    public void NextNivel(string name)
    {
        LevelControl.instancia.desbloqueados++;
        LevelControl.instancia.AumentarNiveles();
        SceneManager.LoadScene(name);
    }
}
