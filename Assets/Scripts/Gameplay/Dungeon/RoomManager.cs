using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
        return directions[Random.Range(0, directions.Count)];
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
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}


// public class RoomManager : MonoBehaviour
// {
//     public int numRooms = 50;
//     public Room startRoom;
//     [HideInInspector] public Room endRoom;
    
//     [SerializeField] private RoomAssets roomAssets;

//     private Dictionary<Vector3, Room> placedRoomPositions = new Dictionary<Vector3, Room>();
//     private Direction previousDirection;

//     public void Start()
//     {
//         Spawn();
//     }

//     public void Spawn()
//     {
//         placedRoomPositions.Clear();
//         Room current = startRoom;

//         for (int i = 0; i < numRooms; i++)
//         {
//             var direction = GetRandomDirection(current);
            
//             Debug.Log($"{current} chose {direction}");

//             if (direction == Direction.None)
//             {
//                 Debug.Log($"direction is none. Breaking at {current}");
//                 break;
//             }
//             GameObject roomPrefab = GetRoomPrefab(direction);
//             if (roomPrefab == null)
//             {
//                 Debug.Log($"no existing roomPrefab. Breaking at {current}");
//                 break;
//             }

//             var position = GetNeighborPosition(current, direction);
//             if (placedRoomPositions.TryGetValue(position, out Room existing))
//             {
//                 Debug.Log($"But there's a room there, so we're moving there");
//                 current = existing;

//             }
//             else
//             {
//                 var room = Instantiate(roomPrefab, position, Quaternion.identity)
//                     .GetComponent<Room>();

//                 placedRoomPositions.Add(position, room);
//                 current = room;
//             }
//         }
//     }

//     private Direction GetRandomDirection(Room room)
//     {
//         Direction entrances = room.entrances;
//         List<Direction> eligibleDirections = new List<Direction>();

//         foreach (Direction direction in Enum.GetValues(typeof(Direction)))
//         {
//             if ((entrances & direction) == 0)
//                 continue;

//             eligibleDirections.Add(direction);
//         }

//         if (eligibleDirections.Count == 0)
//             return Direction.None;

//         return eligibleDirections[UnityEngine.Random.Range(0, eligibleDirections.Count)];
//     }

//     private GameObject GetRoomPrefab(Direction direction)
//     {
//         switch (direction)
//         {
//             case Direction.North:
//                 return GetRandomRoom(roomAssets.southRooms);
//             case Direction.South:
//                 return GetRandomRoom(roomAssets.northRooms);
//             case Direction.East:
//                 return GetRandomRoom(roomAssets.westRooms);
//             case Direction.West:
//                 return GetRandomRoom(roomAssets.eastRooms);
//         }

//         return null;
//     }
    
//     private GameObject GetRandomRoom(List<RoomAssetInfo> rooms)
//     {
//         return rooms[UnityEngine.Random.Range(0, rooms.Count)].roomPrefab;
//     }

//     private Vector3 GetNeighborPosition(Room neighbor, Direction direction)
//     {
//         switch (direction)
//         {
//             case Direction.North:
//                 return neighbor.transform.position + new Vector3(0, 1, 0);
//             case Direction.South:
//                 return neighbor.transform.position + new Vector3(0, -1, 0);
//             case Direction.East:
//                 return neighbor.transform.position + new Vector3(1, 0, 0);
//             case Direction.West:
//                 return neighbor.transform.position + new Vector3(-1, 0, 0);
//         }

//         return Vector3.zero;
//     }
// }
