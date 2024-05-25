using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class RoomController : MonoBehaviour
{
    public Room room { get; private set; }
    DungeonManager manager;

    Dictionary<Direction, Wall> walls = new Dictionary<Direction, Wall>();
    public Waypoint playerStartWaypoint;
    public List<Waypoint> roomSpawnPoints;
    public List<Waypoint> bossStartWaypoints;
    public EnemySpawner enemySpawner;
    public WeaponChestSpawner weaponChestSpawner;
    public bool isActiveRoom;

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

    public void Init(Room room, DungeonManager manager, RoomConfig config)
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
                walls[direction].SetOpen();
            else
                walls[direction].SetClosed();
        }

        AssignWaypoints();
    }

    void SetRoomWalls()
    {
        Wall[] wallComponents = GetComponentsInChildren<Wall>();
        foreach (Wall wall in wallComponents)
        {
            if (wall.direction == Direction.All || wall.direction == Direction.None)
            {
                Debug.LogWarning($"Wall in {this.room} improperly set. Needs one explicit direction");
                continue;
            }

            walls[wall.direction] = wall;
        }
    }

    void AssignWaypoints()
    {
        weaponChestSpawner.AddSpawnPoints(roomSpawnPoints);
        enemySpawner.AddSpawnPoints(roomSpawnPoints);
    }

    public void CreateItems()
    {
        Debug.Log("CreateItems");
        switch(room.roomType)
        {
            case RoomType.Treasure:
                weaponChestSpawner.Spawn(100f, 1, 1);
                break;
            case RoomType.Combat:
                enemySpawner.Spawn(100f, 4, 6);
                break;
            case RoomType.Normal:
                enemySpawner.Spawn(0.5f, 1, 4);
                break;
            default:
                break;
        }
    }


    public Wall GetWallAt(Direction direction)
    {
        if (HasEntrance(direction) && walls.TryGetValue(direction, out Wall wall))
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

    public void OnRoomEntered()
    {
        CreateItems();
        isActiveRoom = true;
    }

    public void OnRoomExited()
    {
        isActiveRoom = false;
    }
}
