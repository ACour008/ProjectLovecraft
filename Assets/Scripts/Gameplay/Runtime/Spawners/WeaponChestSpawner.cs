using UnityEngine;

public class WeaponChestSpawner : Spawner
{
    [SerializeField] GameObject treasureChestPrefab;

    public override void Spawn(float chanceOfSpawn, int minNumSpawns, int maxNumSpawns)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
        GameObject chest = Instantiate(treasureChestPrefab, spawnPoint.transform.position, Quaternion.identity, spawnPoint);
        Debug.Log(chest);
    }

}
