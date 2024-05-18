using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRadius = 1f;

    public void Spawn(GameObject prefab, Transform parent, Vector3 position)
    {
        Vector3 randomPosition = position + (Vector3)(Random.insideUnitCircle * spawnRadius);
        Instantiate(prefab, randomPosition, Quaternion.identity, parent);
    }
}
