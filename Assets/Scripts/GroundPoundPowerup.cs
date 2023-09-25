using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundPowerup : MonoBehaviour
{
    public GameObject powerupIndicator;
    public float airPosition = 8;
    public float speed = 10;
    public float effectDistance = 3;
    public float powerupDuration = 3;
    public float coyoteTimeDuration = 0.2f;
    public float pushForce = 15;
    //public GameObject explosion;
    public ParticleSystem explosionPS;
    private float groundPosition;
    private bool powerupActive = false;
    private int jumpStep = 0;

    private Rigidbody enemyRb;
    private Rigidbody ballRb;
    private PlayerController player;



    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
    }

    // If power active
    // Start going to airPosition at speed
    // wait just some time on air
    // start going to groundposition at speed
    // when on ground, apply an impulse force to all enemies at a defined distance

    // Update is called once per frame
    void Update()
    {
        if (powerupActive)
        {
            CheckForJump();
        }

        if (jumpStep > 0)
        {
            GoUp();
            CoyoteTime();
            GoDown();
            Stomp();
        }
    }

    private void CheckForJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpStep == 0)
        {
            jumpStep = 1;
            groundPosition = transform.position.y;
            MovementConstraints(true);
        }
    }

    private void GoUp()
    {
        if (jumpStep == 1)
        {
            transform.Translate(Vector3.up * (speed * Time.deltaTime), Space.World);
            if (transform.position.y >= airPosition)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    airPosition,
                    transform.position.z);
                jumpStep = 2;
            }
        }
    }

    private void MovementConstraints(bool applyConstraints)
    {
        if (applyConstraints)
        {
            player.PlayerMovementOff();
            ballRb.useGravity = false;
            ballRb.isKinematic = true;
            ballRb.velocity = Vector3.zero;
            ballRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            ballRb.constraints = RigidbodyConstraints.None;
            ballRb.isKinematic = false;
            ballRb.useGravity = true;
            player.PlayerMovementOn();
        }
    }

    private void CoyoteTime()
    {
        if (jumpStep == 2)
        {
            jumpStep = 3;
            StartCoroutine(WaitForCoyoteTime());
        }
    }

    IEnumerator WaitForCoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTimeDuration);
        jumpStep = 4;
    }

    private void GoDown()
    {
        if (jumpStep == 4)
        {
            transform.Translate(Vector3.down * (speed * Time.deltaTime), Space.World);
            if (transform.position.y <= groundPosition)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    groundPosition,
                    transform.position.z);
                jumpStep = 5;
            }
        }
    }

    private void Stomp()
    {
        if (jumpStep == 5)
        {
            ApplyForceToAllEnemiesInRange();
            explosionPS.Play();
            MovementConstraints(false);
            jumpStep = 0;
        }
    }

    private void ApplyForceToAllEnemiesInRange()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            ApplyForceIfInsideLimit(enemy);
        }
    }

    private void ApplyForceIfInsideLimit(Enemy enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        if (Mathf.Abs(direction.magnitude) <= effectDistance)
        {
            enemyRb = enemy.GetComponent<Rigidbody>();
            enemyRb.AddForce(
                new Vector3(
                    direction.normalized.x,
                    MathF.Sin(45),
                    direction.normalized.z)
                * pushForce,
                ForceMode.Impulse);
        }
    }

    public void ActivatePowerUp()
    {
        StopCoroutine(DeactivatePowerup());
        powerupActive = true;
        powerupIndicator.SetActive(true);
        StartCoroutine(DeactivatePowerup());
    }

    IEnumerator DeactivatePowerup()
    {
        yield return new WaitForSeconds(powerupDuration);
        powerupActive = false;
        powerupIndicator.SetActive(false);
    }


}
