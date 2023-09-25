using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyPrefabData[] enemyPrefabs;
    [SerializeField]
    private GameObject[] powerupPrefab;
    
    public float spawnRange = 9;
    public Vector3 spawnPoint = new Vector3(0, 0, 0);
    
    private int enemyCount;
    private int waveNumber = 1;
    private int totalProbWeight;
    private bool onCurrentWave = false;
    private bool countNextWave = true;
    

    // Start is called before the first frame update
    void Start()
    {
        //SpawnEnemyWave(waveNumber);
        totalProbWeight = GetWeightSum();
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnAtRandomPosition(GetEnemyPrefab());
        }
        SpawnAtRandomPosition(GetRandomPowerUp());
    }



    private GameObject GetEnemyPrefab()
    {
        int randomWeight = Random.Range(1, totalProbWeight + 1);

        return GetEnemyByWeight(randomWeight);
    }

    private int GetWeightSum()
    {
        int sum = 0;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            sum += enemyPrefabs[i].probWeight;
        }
        return sum;
    }

    private GameObject GetEnemyByWeight(int weight)
    {
        int currentSum = 0;
        int selectedIndex = 0;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            currentSum += enemyPrefabs[i].probWeight;
            if (weight <= currentSum)
            {
                selectedIndex = i;
                break;
            }
        }
        return enemyPrefabs[selectedIndex].prefab;

    }

    private GameObject GetRandomPowerUp()
    {
        return powerupPrefab[Random.Range(0,powerupPrefab.Length)];
    }

    private void SpawnAtRandomPosition(GameObject prefabObject)
    {
        Instantiate(prefabObject, GetRandomSpawnPosition(), prefabObject.transform.rotation);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0, spawnPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (onCurrentWave)
        {
            CountEnemiesOnStage();
        }
    }

    private void CountEnemiesOnStage()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (FindObjectsOfType<BossEnemy>().Length > 0)
        {
            enemyCount--; //do not count the boss
        }
        Debug.Log($"Found Enemies: {enemyCount}");

        if (enemyCount <= 0)
        {
            if (countNextWave)
            {
                waveNumber++;
            }
            onCurrentWave = false;
        }
    }

    private void StartWaveWith(int enemiesNumber)
    {
        SpawnEnemyWave(enemiesNumber);
        onCurrentWave = true;
    }

    public void StartNewWave()
    {
        if (!onCurrentWave)
        {
            countNextWave = true;
            StartWaveWith(waveNumber);
        }
    }

    public void StartNewWaveWith(int withNumberOfEnemies)
    {
        if (!onCurrentWave)
        {
            countNextWave = false;
            StartWaveWith(withNumberOfEnemies);
        }
    }

    public bool IsReadyForNewWave()
    {
        return !onCurrentWave;
    }
}

[System.Serializable]
public class EnemyPrefabData
{
    public GameObject prefab;
    public int probWeight;
}
