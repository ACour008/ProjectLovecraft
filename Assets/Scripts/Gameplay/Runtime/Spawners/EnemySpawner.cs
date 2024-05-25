using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner
{

    public int maxSpawnsInRoom = 5;
    public int spawnRadius = 1;
    
    public List<GameObject> allEnemies;
    List<Enemy> spawnedEnemies = new List<Enemy>();

    public int enemyCount => spawnedEnemies.Count;

    public bool hasSpawned;


    public override void Spawn(float chanceOfSpawn, int minNumSpawns, int maxNumSpawns)
    {
        if (spawnedEnemies.Count > enemyCount || hasSpawned)
            return;

        hasSpawned = true;
        if (Random.Range(0f, 1f) < chanceOfSpawn)
        {
            int limit = maxSpawnsInRoom - enemyCount;
            int numSpawns = Random.Range(minNumSpawns, Math.Min(maxNumSpawns + 1, limit));
            for (int i = 0; i < numSpawns; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                GameObject prefab = allEnemies[Random.Range(0, allEnemies.Count)];
                Vector3 position = spawnPoint.position + (Random.insideUnitCircle * spawnRadius).AsVector3();
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                Enemy enemy = Instantiate(prefab, position, rotation, spawnPoint)
                    .GetComponent<Enemy>();
                
                // Subscribe to OnEnemyDeath

                spawnedEnemies.Add(enemy);
            }
        }
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        // unsubscribe to enemy;
        spawnedEnemies.Remove(enemy);
    }
}
