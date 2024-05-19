using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RoomWall
{
    [SerializeField] GameObject closed;
    [SerializeField] GameObject open;
    [SerializeField] GameObject door;
    [SerializeField] public Waypoint start;

    public void SetOpen()
    {
        closed.SetActive(false);
        open.SetActive(true);
    }

    public void SetClosed()
    {
        closed.SetActive(true);
        open.SetActive(false);
    }
}

public class RoomController : MonoBehaviour
{
    public Room room { get; private set; }
    RoomManager manager;

    [SerializeField] public RoomWall northWall;
    [SerializeField] public RoomWall eastWall;
    [SerializeField] public RoomWall southWall;
    [SerializeField] public RoomWall westWall;
    
    [SerializeField] GameObject treasureChestPrefab;

    Dictionary<Direction, RoomWall> roomWalls = new Dictionary<Direction, RoomWall>();
    public List<Waypoint> waypoints = new List<Waypoint>();
    public EnemySpawner enemySpawner;

    WaypointType waypointType
    {
        get
        {
            switch(room.roomType)
            {
                case RoomType.Start:
                    return WaypointType.PlayerStart;
                case RoomType.Boss:
                    return WaypointType.Boss;
                case RoomType.Health:
                case RoomType.Combat:
                case RoomType.Treasure:
                case RoomType.Normal:
                    return WaypointType.RoomSpecific;
                default:
                    return WaypointType.None;
            }
        }
    }

    public void Init(Room room, RoomManager manager, RoomConfig config)
    {
        this.room = room;
        this.manager = manager;
        name = room.ToString();
        enemySpawner = GetComponent<EnemySpawner>();
        SetRoomWalls();

        transform.position = config.GetWorldPosition(room.position);
        transform.localScale = config.GetLocalScale();
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (direction == Direction.None || direction == Direction.All)
                continue;

            if (HasEntrance(direction))
                roomWalls[direction].SetOpen();
            else
                roomWalls[direction].SetClosed();
        }

        CollectWaypoints();
    }

    void SetRoomWalls()
    {
        roomWalls[Direction.North] = northWall;
        roomWalls[Direction.East] = eastWall;
        roomWalls[Direction.South] = southWall;
        roomWalls[Direction.West] = westWall;
    }

    void CollectWaypoints()
    {
        waypoints = FindObjectsByType<Waypoint>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .Where(x => x.waypointType == waypointType)
            .ToList();
    }

    public void CreateItems()
    {
        switch(room.roomType)
        {
            case RoomType.Treasure:
                SpawnTreasure();
                break;
            case RoomType.Combat:
                SpawnEnemies(100f, 4, 6);
                break;
            case RoomType.Normal:
                SpawnEnemies(0.75f, 1, 4);
                break;
            default:
                break;
        }
    }

    void SpawnTreasure()
    {
        Spawn(treasureChestPrefab);
    }

    void SpawnEnemies(float chance, int minSpawns, int maxSpawns)
    {
        enemySpawner.Spawn(chance, minSpawns, maxSpawns);
    }

    GameObject Spawn(GameObject prefab)
    {
        Transform spawnPoint = waypoints[Random.Range(0, waypoints.Count)].transform;
        return Instantiate(prefab, transform.TransformPoint(spawnPoint.localPosition), spawnPoint.rotation, transform);
    }

    public RoomWall GetWallAt(Direction direction)
    {
        if (HasEntrance(direction) && roomWalls.TryGetValue(direction, out RoomWall wall))
            return wall;
        return null;
    }

    public bool HasEntrance(Direction direction)
    {
        return room.HasEntrance(direction);
    }

    public void OnDoorTriggered(Direction direction)
    {
        manager.OnDoorTriggered(this, direction);
    }
}
