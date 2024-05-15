using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoomWall
{
    [SerializeField] GameObject closed;
    [SerializeField] GameObject open;
    [SerializeField] GameObject door;
    
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
    Room room;

    [SerializeField] public RoomWall northWall;
    [SerializeField] public RoomWall eastWall;
    [SerializeField] public RoomWall southWall;
    [SerializeField] public RoomWall westWall;

    Dictionary<Direction, RoomWall> roomWalls = new Dictionary<Direction, RoomWall>();

    public void Init(Room room, RoomConfig config)
    {
        this.room = room;
        this.name = room.ToString();
        SetRoomWalls();

        transform.position = new Vector3(room.position.x, room.position.y, 0) * config.positionOffset;
        transform.localScale = Vector3.one * config.scale;

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (direction == Direction.None || direction == Direction.All)
                continue;

            if ((room.entrances & direction) == 0)
                roomWalls[direction].SetClosed();
            else
                roomWalls[direction].SetOpen();
        }
    }

    void SetRoomWalls()
    {
        roomWalls[Direction.North] = northWall;
        roomWalls[Direction.East] = eastWall;
        roomWalls[Direction.South] = southWall;
        roomWalls[Direction.West] = westWall;
    }
}
