using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomExtensions
{
    public static Direction GetOppositeDirection(this Direction direction)
    {
        if (direction == Direction.North)
            return Direction.South;
        if (direction == Direction.South)
            return Direction.North;
        if (direction == Direction.East)
            return Direction.West;
        if (direction == Direction.West)
            return Direction.East;
        
        return Direction.None;
    }
}
