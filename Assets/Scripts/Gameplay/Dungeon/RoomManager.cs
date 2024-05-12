using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathGenerator
{
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var prevPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = prevPosition + Direction2D.GetRandomDirection();
            path.Add(newPosition);
            prevPosition = newPosition;
        }

        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    public static Vector2Int GetRandomDirection()
    {
        return directions[UnityEngine.Random.Range(0, directions.Count)];
    }
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] private int iterations = 30;
    public int walkLength = 30;
    public bool startRandomlyEachIteration = true;

    public void Start()
    {
        Generate();
    }


    public void Generate()
    {
        HashSet<Vector2Int> positions = RandomWalk();

        foreach (var position in positions)
            Debug.Log(position);
    }

    protected HashSet<Vector2Int> RandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iterations; i++)
        {
            var path = PathGenerator.RandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}
