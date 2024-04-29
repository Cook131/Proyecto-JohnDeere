using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausa : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    bool juegopausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegopausado)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        juegopausado = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        juegopausado = false;
    }

    /*public void Restart()
    {
        Time.timeScale = 1;
        juegopausado = false;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    public void Win(string name)
    {
        SceneManager.LoadScene(name);
    }
}
