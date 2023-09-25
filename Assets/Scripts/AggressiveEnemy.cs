using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    public float attackStrength = 2;
    private bool allowHit = true;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!collision.gameObject.GetComponent<PushPowerup>().IsPowerupActive() && allowHit)
            {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 direction = collision.gameObject.transform.position - transform.position;
                playerRb.AddForce(direction.normalized * attackStrength, ForceMode.Impulse);
            }
        }
    }

    public void TurnOffHit()
    {
        allowHit = false;
    }
}
