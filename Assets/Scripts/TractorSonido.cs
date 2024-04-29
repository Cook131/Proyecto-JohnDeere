using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorSonido : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    public AudioClip clip;
    public float volumenInicial = 0.5f;
    public float volumenMAximo =1.0f;
    public float velAumento = 0.1f;
    private bool aumentando =false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volumenInicial;

        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (audioSource.volume > volumenMAximo)
            {
                audioSource.volume += velAumento * Time.deltaTime;
            }
        }
        else
        {
            aumentando = false;
        }

        if (!aumentando && audioSource.volume > volumenInicial)
        {
            audioSource.volume -= velAumento * Time.deltaTime;
        }
    }
}
