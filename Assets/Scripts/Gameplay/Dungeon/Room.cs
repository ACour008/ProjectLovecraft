using System;
using System.Collections.Generic;
using UnityEngine;

[Flags, Serializable]
public enum Direction
{
    None = 0,
    North = 0b1000,
    East = 0b0100,
    South = 0b0010,
    West = 0b0001,
    All = North | East | West | South
}

[Serializable]
public enum RoomType
{
    Normal = 0,
    Health = 1,
    Treasure = 2,
    Combat = 3,
    Start = 4,
    Boss = 5,
}

public class Room
{
    public Vector2Int position;
    public Direction entrances;
    public Dictionary<Direction, Room> neighbors = new Dictionary<Direction, Room>();
    [HideInInspector] public Direction lockedDoors;
    public RoomType roomType;

    public Room(Vector2Int position, Direction entrances)
    {
        this.position = position;
        this.entrances = entrances;
        lockedDoors = Direction.None;
        roomType = RoomType.Normal;
    }
    public Room(Vector2Int position, Direction entrances, Direction lockedDoors)
    {
        this.position = position;
        this.entrances = entrances;
        this.lockedDoors = lockedDoors;
        roomType = RoomType.Normal;
    }

    public void LockDoor(Direction direction)
    {
        lockedDoors |= direction;
    }

    public void UnlockDoor(Direction direction)
    {
        lockedDoors &= ~direction;
    }

    public bool IsLocked(Direction direction)
    {
        return (lockedDoors & direction) != 0;
    }

    public override string ToString() => $"Room ({roomType}) at {position}";
}