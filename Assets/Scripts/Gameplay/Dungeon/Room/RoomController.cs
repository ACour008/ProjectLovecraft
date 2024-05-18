using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Dictionary<Direction, RoomWall> roomWalls = new Dictionary<Direction, RoomWall>();

    public void Init(Room room, RoomManager manager, RoomConfig config)
    {
        this.room = room;
        this.manager = manager;
        name = room.ToString();
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
    }

    void SetRoomWalls()
    {
        roomWalls[Direction.North] = northWall;
        roomWalls[Direction.East] = eastWall;
        roomWalls[Direction.South] = southWall;
        roomWalls[Direction.West] = westWall;
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
