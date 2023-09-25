using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPowerup : MonoBehaviour
{
    public bool hasPowerUp = false;
    private float powerUpStrength = 15.0f;
    private float powerUpTime = 7.0f;
    public GameObject powerUpIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePowerUp()
    {
        StopCoroutine(PowerUpCountDownRoutine());
        hasPowerUp = true;
        powerUpIndicator.SetActive(true);
        StartCoroutine(PowerUpCountDownRoutine());
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(powerUpTime);
        hasPowerUp = false;
        powerUpIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = collision.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(direction.normalized * powerUpStrength, ForceMode.Impulse);
            //Debug.Log($"Collided with {collision.gameObject.name} with powerup set to {hasPowerUp}");
        }
    }

    public bool IsPowerupActive()
    {
        return hasPowerUp;
    }
}
