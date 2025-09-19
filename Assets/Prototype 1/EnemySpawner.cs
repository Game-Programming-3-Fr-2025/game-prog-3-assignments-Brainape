using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject[] enemyPrefabs;
    public float waveDuration = 20f, spawnInterval = 2f;
    public int baseSpawnCount = 2;
    public float growth = 1.5f;
    public int maxEnemies = 100;

    float gameTime, spawnTimer;
    int currentWave;
    List<GameObject> enemies = new();

    void Update()
    {
        gameTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Update wave every 20s
        currentWave = Mathf.FloorToInt(gameTime / waveDuration);

        // Spawn check
        if (spawnTimer >= spawnInterval && enemies.Count < maxEnemies)
        {
            spawnTimer = 0;
            int count = Mathf.CeilToInt(baseSpawnCount * Mathf.Pow(growth, currentWave));
            for (int i = 0; i < count; i++)
                enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], GetSpawnPos(), Quaternion.identity));
        }

        enemies.RemoveAll(e => e == null);
    }

    Vector3 GetSpawnPos()
    {
        Vector2 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float o = 2f;
        return Random.Range(0, 4) switch
        {
            0 => new Vector3(player.position.x - bounds.x - o, Random.Range(-bounds.y, bounds.y)),
            1 => new Vector3(player.position.x + bounds.x + o, Random.Range(-bounds.y, bounds.y)),
            2 => new Vector3(Random.Range(-bounds.x, bounds.x), player.position.y + bounds.y + o),
            _ => new Vector3(Random.Range(-bounds.x, bounds.x), player.position.y - bounds.y - o),
        };
    }
}
