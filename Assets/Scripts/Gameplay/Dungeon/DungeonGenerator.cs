using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public int numRooms = 20;
    public Vector2Int startPosition = Vector2Int.zero;
    public Direction startDirections = Direction.All;
    public RoomAssets roomAssets;
    private List<Room> rooms = new List<Room>();


    private Room startRoom;
    private Room bossRoom;

    void Start()
    {
        if ((int)startDirections == -1)
            startDirections = Direction.All;

        roomAssets.CreateSpriteLookup();
        GenerateDungeon();


        for(int i = 0; i < rooms.Count; i++)
        {
            CreateRoom(rooms[i], i);
        }
    }

    void GenerateDungeon()
    {
        startRoom = new Room(startPosition, startDirections);
        rooms.Add(startRoom);

        while (rooms.Count < numRooms)
        {
            Room prevRoom = rooms[rooms.Count - 1];
            Direction entrance = GetRandomEntrance(prevRoom);
            Vector2Int newPosition = prevRoom.position + entrance.GetDirectionVector();

            if (!IsPositionOccupied(newPosition))
            {
                Room newRoom = new Room(newPosition, GetRandomDirections(entrance.GetOppositeDirection()));
                ConnectRooms(prevRoom, newRoom, entrance);
                rooms.Add(newRoom);

                bossRoom = newRoom;
            }
        }

        CloseDungeon();
    }

    Direction GetRandomEntrance(Room room)
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

    bool IsPositionOccupied(Vector2Int position)
    {
        foreach (Room room in rooms)
        {
            if (room.position == position)
                return true;
        }

        return false;
    }

    Direction GetRandomDirections(Direction required)
    {
        var directions = new List<Direction>()
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

        var direction = directions[Random.Range(0, directions.Count)];
        if ((direction & required) == 0)
            direction |= required;
        
        return direction;
    }

    void ConnectRooms(Room room, Room other, Direction entrance)
    {
        room.neighbors[entrance] = other;
        other.neighbors[entrance.GetOppositeDirection()] = room;
    }

    void CloseDungeon()
    {
        foreach (Room room in rooms)
        {
            Direction newEntrances = room.entrances;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction == Direction.None || direction == Direction.All)
                    continue;

                if ((room.entrances & direction) != 0 && !room.neighbors.ContainsKey(direction))
                    newEntrances &= ~direction;
            }

            room.entrances = newEntrances;
        }
    }

    public void CreateRoom(Room room, int index)
    {
        var renderer = new GameObject(nameof(room), typeof(SpriteRenderer))
            .GetComponent<SpriteRenderer>();
        
        if (roomAssets.spriteLookup.TryGetValue(room.entrances, out Sprite sprite))
        {   
            renderer.sprite = sprite;
            renderer.transform.position = room.position.AsVector3();
            renderer.transform.SetParent(this.transform);

            if (index == 0)
                renderer.color = Color.green;
            if (index == rooms.Count - 1)
                renderer.color = Color.blue;
        }
        else
            Debug.Log($"Didn't find sprite for {room} with {room.entrances}");
    }
}
