using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    public float speed = 5.0f;
    public GameObject powerupIndicatorContainer;

    private PushPowerup pPow;
    private MissilePowerup mPow;
    private GroundPoundPowerup gPow;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private bool movementEnabled = true;
    private bool alive = true;


    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        pPow = GetComponent<PushPowerup>();
        mPow = GetComponent<MissilePowerup>();
        gPow = GetComponent<GroundPoundPowerup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            MoveForward();
            AdjustPowerUpIndicatorPosition();
        }

        if (transform.position.y <= -10)
        {
            alive = false;
        }
    }

    private void AdjustPowerUpIndicatorPosition()
    {
        powerupIndicatorContainer.transform.position = transform.position;
    }

    private void MoveForward()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * (forwardInput * speed), ForceMode.Force);
        float sideInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(focalPoint.transform.right * (sideInput * speed), ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            int pId = other.gameObject.GetComponent<Powerup>().GetPowerUpID();
            ActivatePowerUp(pId);
            Destroy(other.gameObject);
        }
    }

    private void ActivatePowerUp(int id)
    {
        switch (id) 
        {
            case 0: pPow.ActivatePowerUp(); break;
            case 1: mPow.ActivatePowerup(); break;
            case 2: gPow.ActivatePowerUp(); break;
            default: break;
        }
    }

    public void PlayerMovementOn()
    {
        movementEnabled = true;
    }

    public void PlayerMovementOff()
    {
        movementEnabled = false;
    }

    public bool IsAlive()
    {
        return alive;
    }

}
