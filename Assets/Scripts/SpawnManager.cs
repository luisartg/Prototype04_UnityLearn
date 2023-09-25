using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private WaveSpawner waveSpawner;
    [SerializeField]
    private GameObject bossPrefab;
    private BossEnemy bossEnemy;
    private int bossEnemies = 2;
    [SerializeField]
    private int normalWaves = 4;
    private int wavesLeft;
    private bool bossActive = false;

    // Start is called before the first frame update
    void Start()
    {
        wavesLeft = normalWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSpawner.IsReadyForNewWave())
        {
            if (wavesLeft <= 0 && !bossActive)
            {
                //spawnBoss
                bossEnemy = Instantiate(
                    bossPrefab,
                    new Vector3(0, 3, 0),
                    bossPrefab.transform.rotation)
                    .GetComponent<BossEnemy>();
                bossActive = true;
                waveSpawner.StartNewWaveWith(bossEnemies);
            }
            else if (bossActive)
            {
                if (bossEnemy == null)
                {
                    bossActive = false;
                    bossEnemies++;
                    wavesLeft = normalWaves;
                }
                else if (bossEnemy.lives > 0)
                {
                    bossEnemy.DealDamage();
                    Debug.Log($"Boss Lives {bossEnemy.lives}");
                    if (bossEnemy.lives > 0)
                    {
                        waveSpawner.StartNewWaveWith(bossEnemies);
                    }
                }
                else
                {
                    Debug.Log("Waiting for boss to die");
                }
            }
            else
            {
                wavesLeft--;
                waveSpawner.StartNewWave();
            }


        }
    }
}
