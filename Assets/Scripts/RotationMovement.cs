using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{
    public float movementSpeed = 360;
    private float currentCycleAngle;
    // Start is called before the first frame update
    void Start()
    {
        currentCycleAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AdvanceAngle();
        Rotate();
    }

    private void AdvanceAngle()
    {
        currentCycleAngle += (movementSpeed * Time.deltaTime);
        if (currentCycleAngle >= 360)
        {
            currentCycleAngle -= 360;
        }
    }

    private void Rotate()
    {
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x,
            transform.localRotation.eulerAngles.y,
            currentCycleAngle);
    }
}
