using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner3D : MonoBehaviour
{
    public Transform player;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float waveDuration = 20f;   
    public float spawnInterval = 2f;   
    public int baseSpawnCount = 2; 
    public float growth = 1.5f;  
    public int maxEnemies = 100;

    private float gameTime = 0f;
    private float spawnTimer = 0f;
    private int currentWave = 0;
    private List<GameObject> enemies = new();

    private void Update()
    {
        gameTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        currentWave = Mathf.FloorToInt(gameTime / waveDuration);

        if (spawnTimer >= spawnInterval && enemies.Count < maxEnemies)
        {
            spawnTimer = 0f;
            int spawnCount = Mathf.CeilToInt(baseSpawnCount * Mathf.Pow(growth, currentWave));

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
            }
        }
        enemies.RemoveAll(e => e == null);
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        GameObject enemy = Instantiate(prefab, spawnPoint.position, rotation);
        enemies.Add(enemy);
    }
}
