using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TiltMovement : MonoBehaviour
{
    //private float initialRotation;
    public float movementSpeed = 10;
    private float currentCycleAngle;
    public float arcValue = 30;
    // Start is called before the first frame update
    void Start()
    {
        //initialRotation = transform.rotation.eulerAngles.z;
        currentCycleAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AdvanceAngle();
        Tilt();
    }

    private void AdvanceAngle()
    {
        currentCycleAngle += (movementSpeed * Time.deltaTime);
        if (currentCycleAngle >= 360)
        {
            currentCycleAngle -= 360;
        }
    }

    private void Tilt()
    {
        transform.localRotation = Quaternion.Euler(
            GetEulerValue(),
            transform.localRotation.eulerAngles.y,
            transform.localRotation.eulerAngles.z);
    }

    private float GetEulerValue()
    {
        return Mathf.Sin(currentCycleAngle) * arcValue;
    }

    
}
