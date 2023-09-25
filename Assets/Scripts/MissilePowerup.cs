using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissilePowerup : MonoBehaviour
{
    public GameObject powerupIndicator;
    public GameObject missilePrefab;
    private float cooldownTime = 0.5f;
    private bool powerUpActive = false;
    private bool allowLaunching = false;
    private float powerUpDuration = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && powerUpActive && allowLaunching)
        {
            allowLaunching = false;
            SpawnMissiles();
            StartCoroutine(CountMissileLaunchCooldown());
        }
    }

    IEnumerator CountMissileLaunchCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        allowLaunching = true;
    }

    public void SpawnMissiles()
    {
        Enemy[] enemies = GetListOfEnemies();
        foreach (Enemy enemy in enemies) 
        {
            MissileBehavior missile = 
                Instantiate(missilePrefab, transform.position, 
                Quaternion.LookRotation(Vector3.up,Vector3.up))
                .GetComponent<MissileBehavior>();
            missile.SetTarget(enemy.gameObject);
        }
    }

    private Enemy[] GetListOfEnemies()
    {
        return FindObjectsByType<Enemy>(FindObjectsSortMode.None)
            .Where(enemy => enemy.gameObject.GetComponent<BossEnemy>() == null)
            .ToArray();
    }

    public void ActivatePowerup()
    {
        //stop the wait time in case the power is taken more than once
        StopCoroutine(RunPowerUpTimeWindow());
        powerUpActive = true;
        allowLaunching = true;
        powerupIndicator.SetActive(true);
        StartCoroutine(RunPowerUpTimeWindow());
    }

    IEnumerator RunPowerUpTimeWindow()
    {
        yield return new WaitForSeconds(powerUpDuration);
        powerUpActive = false;
        allowLaunching = false;
        powerupIndicator.SetActive(false);
    }
}
