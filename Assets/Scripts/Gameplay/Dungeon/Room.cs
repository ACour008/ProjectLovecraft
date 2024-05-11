using System;
using UnityEngine;

[Flags]
public enum Direction
{
    North = 0b1000,
    East = 0b0100,
    South = 0b0010,
    West = 0b0001
}

public class Room : MonoBehaviour
{
    public Direction entrances;
    [HideInInspector] public Direction lockedDoors;

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

}