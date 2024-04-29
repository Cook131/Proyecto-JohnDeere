using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 

using System.IO.Ports; //Libreria para comunicacion serial

public class CarController : MonoBehaviour
{
    public SerialPort serialPort = new SerialPort("COM5", 115200);
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private int value;
    private float gear;
    private float vertgear;
    private float giro;

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

    [SerializeField] private float velocidad;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        m_prevPosition = rb.position; 
        //*********************************************************************************************************************
        //UART interface
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
        //********************************************************************************************************************* 
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
        
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        /*velocidad = Mathf.RoundToInt(rb.velocity.magnitude * 3.6f);
        textValue = "Velocidad: " + velocidad + "Km/h"; */

    }

    private void GetInput() {
        // Steering Input
        horizontalInput = giro;

        // Acceleration Input
        verticalInput = gear*vertgear;

        // Breaking Input
        //isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor() {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
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

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger");
        }

        if(!serialPort.IsOpen)
        {
            serialPort.Open();
            serialPort.ReadTimeout = -1;
        }

        if(serialPort.IsOpen)
        {
            byte[] data1 = BitConverter.GetBytes(puntos);// send score to the FPGA
            serialPort.Write(data1, 0, 1);
            serialPort.Close();
            string HexString = BitConverter.ToString(data1).Replace("-", " ");
            //Debug.Log(HexString);
            
            //byte[] data2 = BitConverter.GetBytes(fertilizante);// send score to the FPGA
            //serialPort.Write(data2, 0, 1);
        }
    }

    void Update() 
    {

        textValue = "Puntos: " + puntos + "\n" + " Fertilizante: " + fertilizante;
        textElement.text = textValue;

        if(meta)
        {
            textValue = "Tarea Completada!!!!!";
            textElement.text = textValue;
        }


        if (!serialPort.IsOpen)
        {
            serialPort.Open(); //Abrimos una nueva conexión de puerto serie
            serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
        }

        if (serialPort.IsOpen)
        {
            try
            {
                if(serialPort.BytesToRead > 0)
                {
                    value = serialPort.ReadByte();
                    Debug.Log(value);
                    serialPort.Close();
                }
                else
                {
                    value = 0;
                    
                }

                if(value == 0x001)
                {
                    Debug.Log("Primera");
                    gear = 0.2f;
                }
                else if(value == 0x02)
                {
                    Debug.Log("Segunda");
                    gear = 0.5f;
                }
                else if(value == 0x03)
                {
                    Debug.Log("Tercera");
                    gear = 0.7f;
                }
                else if(value == 0x04)
                {
                    Debug.Log("Cuarta");
                    gear = 0.9f;
                }
                else if(value == 0x05)
                {
                    Debug.Log("Quinta");
                    gear = 1.5f;
                }
                else if(value == 0x06)
                {
                    Debug.Log("Retroceso");
                    isBreaking = false;
                    vertgear = -1;
                }
                else if(value == 0x07)
                {
                    Debug.Log("Acelerar");
                    isBreaking = false;
                    vertgear = 1;
                }
                else if(value == 0x08)
                {
                    Debug.Log("Frenar");
                    vertgear = 0;
                    isBreaking = true;
                    ApplyBreaking();

                    
                }
                else if(value == 0x09)
                {
                    Debug.Log("Derecha");
                    giro = 1;
                }
                else if(value == 0x0A)
                {
                    Debug.Log("Izquierda");
                    giro = -1;
                }
                else
                {
                }
            }
                catch { }
            }
        }
}
