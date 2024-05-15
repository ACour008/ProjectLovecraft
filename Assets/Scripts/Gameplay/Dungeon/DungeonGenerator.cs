using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator
{
    public int numRooms = 20;
    public int combatRoomsPercentage = 5;
    public int capRoomPercentage = 25;
    public Vector2Int startPosition = Vector2Int.zero;
    public Direction startDirections = Direction.All;
    public Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();


    private Room startRoom;
    private Room bossRoom;

    public void Generate()
    {
        if ((int)startDirections == -1)
            startDirections = Direction.All;

        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        startRoom = bossRoom = new Room(startPosition, startDirections);
        startRoom.roomType = RoomType.Start;
        rooms.Add(startPosition, startRoom);

        int maxAttempts = 100;
        int attempts = 0;
        while (rooms.Count < numRooms && attempts < maxAttempts)
        {
            Direction entrance = GetRandomEntrance(bossRoom);
            Vector2Int newPosition = bossRoom.position + entrance.GetDirectionVector();

            if (!IsPositionOccupied(newPosition))
            {
                Room newRoom = new Room(newPosition, GetRandomDirections(entrance.GetOppositeDirection()));
                ConnectRooms(bossRoom, newRoom, entrance);
                rooms.Add(newPosition, newRoom);
                bossRoom = newRoom;
            }
            else
            {
                attempts++;
            }
        }

        bossRoom.roomType = RoomType.Boss;
        AddCapRooms();
        CloseDungeon();
        AddRoomTypes();
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
        return rooms.TryGetValue(position, out _);
    }

    List<Direction> GetAvailableEntrances(Room room)
    {
        List<Direction> availableEntrances = new List<Direction>();

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if ((room.entrances & direction) != 0 && !IsPositionOccupied(room.position + direction.GetDirectionVector()))
                availableEntrances.Add(direction);
        }

        return availableEntrances;
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

    void AddCapRooms()
    {
        var capRooms = new List<Room>();
        float chanceOfCreation = capRoomPercentage / 100f;
        foreach (var room in rooms.Values)
        {
            if (room.roomType == RoomType.Boss)
                continue; 

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction == Direction.None || direction == Direction.All)
                    continue;
                
                if (chanceOfCreation >= Random.Range(0f, 1f))
                    continue;

                Vector2Int position = room.position + direction.GetDirectionVector();
                if ((room.entrances & direction) != 0 && !IsPositionOccupied(position))
                {
                    var newRoom = new Room(position, direction.GetOppositeDirection());
                    ConnectRooms(room, newRoom, direction);
                    capRooms.Add(newRoom);
                }
            }
        }

        foreach(var room in capRooms)
            rooms[room.position] = room;
    }

    void CloseDungeon()
    {
        foreach (Room room in rooms.Values)
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

    void AddRoomTypes()
    {
        HashSet<Room> processedRooms = new HashSet<Room>();

        AddRoomType(RoomType.Health, 1, processedRooms);
        AddRoomType(RoomType.Treasure, 1, processedRooms);
        AddRoomType(RoomType.Combat, Mathf.CeilToInt(numRooms * combatRoomsPercentage / 100f), processedRooms);
    }

    void AddRoomType(RoomType roomType, int numRooms, HashSet<Room> processed)
    {
        for (int i = 0; i < numRooms; i++)
        {
            var selectedRoom = rooms.ElementAt(Random.Range(1, rooms.Count - 2)).Value; // dont select start or boss room.
            while (processed.Contains(selectedRoom))
                selectedRoom = rooms.ElementAt(Random.Range(1, rooms.Count - 2)).Value;
        
            selectedRoom.roomType = roomType;
            processed.Add(selectedRoom);
        }
    }
}
