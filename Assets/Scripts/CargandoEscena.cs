using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CargandoEscena
{
    public static String  siguienteEscena;

    public static void EscenaCarga(string nombre)
    {
        siguienteEscena = nombre;
        SceneManager.LoadScene("Cargando");
    }   
}

    
