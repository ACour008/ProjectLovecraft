using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class RoomManager : MonoBehaviour
{
    public int iterations = 10;
    public int walkLength = 50;
    private HashSet<Room> rooms = new HashSet<Room>();
    public Vector2Int startPosition = new Vector2Int(0, 0);

    void Start()
    {
        for (int i = 0; i < iterations; i++)
        {
            var path = GenerateRooms();
            rooms.UnionWith(path);
        }

        foreach(Room room in rooms)
        {
            Debug.Log($"{room} with {room.entrances})");
        }
    }

    public HashSet<Room> GenerateRooms()
    {
        HashSet<Room> rooms = new HashSet<Room>();
        Room currentRoom = new Room(startPosition, Direction.All);
        rooms.Add(currentRoom);

        for (int i = 0; i < walkLength; i++)
        {
            var randomDirection = GetRandomDirection(currentRoom);
            var position = currentRoom.position + randomDirection.GetDirectionVector();
            var newRoom = new Room(position, GetRandomDirections());
            rooms.Add(newRoom);
            currentRoom = newRoom;
        }

        return rooms;
    }

    public Direction GetRandomDirection(Room room)
    {
        var entrances = room.entrances;
        List<Direction> candidates = new List<Direction>();

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if ((entrances & direction) == 0)
                continue;
            
            candidates.Add(direction);
        }

        if (candidates.Count == 0)
            return Direction.None;
        
        return candidates[Random.Range(0, candidates.Count)];

    }

    public Direction GetRandomDirections()
    {
        var randomDirections = new List<Direction>()
        {
            Direction.All,
            Direction.North | Direction.South,
            Direction.East | Direction.West,
            Direction.North | Direction.East,
            Direction.North | Direction.West,
            Direction.South | Direction.East,
            Direction.South | Direction.West,
            Direction.North | Direction.East| Direction.South,
            Direction.South | Direction.East| Direction.West,
            Direction.North | Direction.South | Direction.East,
            Direction.North | Direction.East | Direction.West
        };

        return randomDirections[Random.Range(0, randomDirections.Count)];
    }
}
