using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3;
    private Rigidbody enemyRb;
    private GameObject player;
    private bool onGround = true;
    private bool movementAllowed = true;
    public bool killOnFall = true;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround && movementAllowed)
        {
            if (playerController.IsAlive())
            {
                Move();
            }
        }
        DestroyIfFallen();
    }

    private void Move()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }

    private void DestroyIfFallen()
    {
        if (transform.position.y < -10 && killOnFall)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = true;
        }
    }

    public void StopMovement()
    {
        movementAllowed = false;
        enemyRb.velocity = Vector3.zero;
    }
}
