using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public int numRooms = 20;
    public int combatRoomsPercentage = 5;
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

        foreach (var room in rooms)
            CreateRoom(room);
    }

    void GenerateDungeon()
    {
        startRoom = new Room(startPosition, startDirections);
        startRoom.roomType = RoomType.Start;
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

        bossRoom.roomType = RoomType.Boss;
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
            var selectedRoom = rooms[Random.Range(1, rooms.Count - 2)]; // dont select start or boss room.
            while (processed.Contains(selectedRoom))
                selectedRoom = rooms[Random.Range(1, rooms.Count - 2)];
        
            selectedRoom.roomType = roomType;
            processed.Add(selectedRoom);
        }
    }

    public void CreateRoom(Room room)
    {
        var renderer = new GameObject(room.roomType.ToString(), typeof(SpriteRenderer))
            .GetComponent<SpriteRenderer>();
        
        if (roomAssets.spriteLookup.TryGetValue(room.entrances, out Sprite sprite))
        {   
            renderer.sprite = sprite;
            renderer.transform.position = room.position.AsVector3();
            renderer.transform.SetParent(this.transform);
        }
        else
            Debug.Log($"Didn't find sprite for {room} with {room.entrances}");
    }
}
