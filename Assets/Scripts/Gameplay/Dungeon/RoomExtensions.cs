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

    public static Vector2Int GetDirectionVector(this Direction direction)
    {
        if (direction == Direction.North)
            return new Vector2Int(0, 1);
        if (direction == Direction.South)
            return new Vector2Int(0, -1);
        if (direction == Direction.East)
            return new Vector2Int(1, 0);
        if (direction == Direction.West)
            return new Vector2Int(-1, 0);

        return Vector2Int.zero;
        
    }

    public static Vector2Int GetOppositeDirectionVector(this Direction direction)
    {
        if (direction == Direction.North)
            return new Vector2Int(0, -1);
        if (direction == Direction.South)
            return new Vector2Int(0, 1);
        if (direction == Direction.East)
            return new Vector2Int(-1, 0);
        if (direction == Direction.West)
            return new Vector2Int(1, 0);

        return Vector2Int.zero;
    }
}
