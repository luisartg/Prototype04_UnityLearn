using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 500;
    public float turboMultiplier = 4;
    public bool turboActive = false;
    private float currentTurboValue = 1;
    public float turboCooldown = 2;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public GameObject smokePartSys;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        ApplySpeed();
        AlignSpecialEffects();
    }

    private void ApplySpeed()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        CheckForTurbo();

        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * currentTurboValue * Time.deltaTime);
    }

    private void CheckForTurbo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !turboActive)
        {
            currentTurboValue = turboMultiplier;
            turboActive = true;
            smokePartSys.GetComponent<ParticleSystem>().Play();
            StartCoroutine(WaitTurboCooldown());
        }
    }

    IEnumerator WaitTurboCooldown()
    {
        yield return new WaitForSeconds(turboCooldown);
        currentTurboValue = 1;
        turboActive = false;
    }

    private void AlignSpecialEffects()
    {
        Vector3 newPos = transform.position + new Vector3(0, -0.6f, 0);
        powerupIndicator.transform.position = newPos;
        smokePartSys.transform.position = newPos;
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}
