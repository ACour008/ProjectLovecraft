using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public List<Waypoint> spawnPoints = new List<Waypoint>();

    public bool ContainsSpawnPoint(Waypoint waypoint)
    {
        return spawnPoints.IndexOf(waypoint) != -1;
    }

    public void AddSpawnPoints(List<Waypoint> waypoints)
    {
        foreach (Waypoint waypoint in waypoints)
        {
            if (!ContainsSpawnPoint(waypoint))
                AddSpawnPoint(waypoint);
        }
    }

    public void AddSpawnPoint(Waypoint waypoint)
    {
        spawnPoints.Add(waypoint);
    }

    public void RemoveSpawnPoint(Waypoint waypoint)
    {
        spawnPoints.Remove(waypoint);
    }

    public virtual void Spawn(float chanceOfSpawn, int minNumSpawns, int maxNumSpawns)
    {

    }
}
