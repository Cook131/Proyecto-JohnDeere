using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO.Ports; //Libreria para comunicacion serial

public class GetObjects : MonoBehaviour
{
    public SerialPort serialPort = new SerialPort("COM5", 115200);
    public string textValue;
    public Text textElement;
    public int puntos = 0;
    public int fertilizante = 0;
    public bool meta = false;
    private Vector3 m_prevPosition;
    private float m_velocity;
    private float m_acceleration;
    private float lastSpeed;
    private Rigidbody rb;
    private float updateSpeed = 0.5f;
    private float timer = 0.0f;
    public string textValue2;
    public Text textElement2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_prevPosition = rb.position;   

         if (!serialPort.IsOpen)
        {
            serialPort.Open(); //Abrimos una nueva conexión de puerto serie
            serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
        }

        if (serialPort.IsOpen)
        {
            var dataByte = new byte[] { 0x00 };
            serialPort.Write(dataByte, 0, 1);// cuando empieza el juego mandamos un dato para limpiar los LEDs del FPGA
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cabbage"))
        {
            Destroy(other.gameObject);
            puntos ++;
            Debug.Log(puntos);
        }
        if(other.CompareTag("WaterCan"))
        {
            fertilizante += 2;
            Destroy(other.gameObject);
            Debug.Log("Conseguiste Fertilizante");
        }
        if(other.CompareTag("Basket"))
        {   
            meta = true;
            Destroy(other.gameObject);
            Debug.Log("Completaste la tarea");
        }

            byte[] data1 = BitConverter.GetBytes(puntos);// send score to the FPGA
            serialPort.Write(data1, 0, 1);
            string HexString = BitConverter.ToString(data1).Replace("-", " ");
            Debug.Log(HexString);
            
            //byte[] data2 = BitConverter.GetBytes(fertilizante);// send score to the FPGA


        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger");
        }
    }

    private void Update()
    {
        textValue = "Puntos: " + puntos + "\n" + " Fertilizante: " + fertilizante;
        textElement.text = textValue;

        if(meta)
        {
            textValue = "Tarea Completada!!!!!";
            textElement.text = textValue;
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= updateSpeed)
        {
            Vector3 currentPosition = rb.position;
            m_velocity = Vector3.Distance(currentPosition,m_prevPosition)/ timer;
            m_acceleration = (m_velocity-lastSpeed)/timer;
            m_prevPosition = currentPosition;
            lastSpeed = m_velocity;
            timer = 0.0f;
            textValue2 = "Velocidad: " + m_velocity.ToString("F3") + "Km/h" + "\n" + "Aceleracion: " + m_acceleration.ToString("F3") + "m/s^2";
            textElement2.text = textValue2;
        }
        
    }
}
