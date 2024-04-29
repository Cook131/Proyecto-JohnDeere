using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum Axle
{
    Front,
    Rear
}

public struct Wheel
{
    public GameObject model; 
    public Axle axle;
    public WheelCollider collider;
}

[RequireComponent(typeof(Rigidbody))]

public class CarControl : MonoBehaviour
{
    [SerializeField]
    private float masAcceleration = 20.0f;

    [SerializeField]
    private float maxSteerAngle = 45.0f;

    [SerializeField]
    private float turnSenstivity = 0.5f;

    private float inputX,inputY;

    private Rigidbody rb;

    public List<AxleInfo> axleInfos =  new List<AxleInfo>();

    /*private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;*/

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        GetInput();
        Move();

    }

    private void GetInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.isFront)
            {
                var _steer = maxSteerAngle * inputX * turnSenstivity;
                axleInfo.leftWheel.steerAngle = Mathf.Lerp(axleInfo.leftWheel.steerAngle, _steer, 0.5f);
                axleInfo.rightWheel.steerAngle = Mathf.Lerp(axleInfo.rightWheel.steerAngle, _steer, 0.5f);            }

            if (axleInfo.isBack)
            {
                axleInfo.leftWheel.motorTorque = masAcceleration * inputY * 500 * Time.deltaTime;
                axleInfo.rightWheel.motorTorque = masAcceleration * inputY * 500 * Time.deltaTime;
            }

            AnimateWheels(axleInfo.leftWheel, axleInfo.visualLeftWheel);
            AnimateWheels(axleInfo.rightWheel, axleInfo.visualRightWheel);
        }

       
    }

    private void AnimateWheels(WheelCollider wheelCollider, Transform wheelTransform)
    {
        UnityEngine.Quaternion rot;
        UnityEngine.Vector3 pos;
        UnityEngine.Vector3 rotate = UnityEngine.Vector3.zero;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public Transform visualLeftWheel;
    public Transform visualRightWheel;

    public bool isBack;
    public bool isFront;
}
