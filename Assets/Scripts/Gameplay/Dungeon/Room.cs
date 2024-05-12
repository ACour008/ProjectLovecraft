using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction
{
    None = 0,
    All = North | East | West | South,
    North = 0b1000,
    East = 0b0100,
    South = 0b0010,
    West = 0b0001
}

public class Room : IEquatable<Room>
{
    public Vector2Int position;
    public Direction entrances;
    [HideInInspector] public Direction lockedDoors;

    public Room(Vector2Int position, Direction entrances)
    {
        this.position = position;
        this.entrances = entrances;
        this.lockedDoors = Direction.None;
    }
    public Room(Vector2Int position, Direction entrances, Direction lockedDoors)
    {
        this.position = position;
        this.entrances = entrances;
        this.lockedDoors = lockedDoors;
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

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if  (obj == null || !(obj is Room))
            return false;

        return this.position.Equals(((Room)obj).position);
    }

    public bool Equals(Room other)
    {
        if (other == null)
            return false;
        
        return this.position.Equals(other.position);
    }

    public override string ToString() => $"Room at {position}";
}