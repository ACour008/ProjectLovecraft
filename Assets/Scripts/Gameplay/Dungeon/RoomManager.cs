using System;
using UnityEngine;



// Maybe turn to ScriptableObject
[Serializable]
public struct RoomConfig
{
    [SerializeField] public GameObject roomPrefab;
    [SerializeField] public float positionOffset;
    [SerializeField] public float scale;

}

public class RoomManager : MonoBehaviour
{
    public RoomConfig config;
    private DungeonGenerator generator = new DungeonGenerator();

    void Start()
    {
        generator.Generate();

        foreach (Room room in generator.rooms.Values)
            CreateRoom(room);
    }
    public void CreateRoom(Room room)
    {
        RoomController roomController = Instantiate(config.roomPrefab, transform).GetComponent<RoomController>();
        roomController.Init(room, config);

        if (room.roomType == RoomType.Start)
            AddWaypoint(WaypointType.Player, roomController);
        else if (room.roomType == RoomType.Boss)
            AddWaypoint(WaypointType.Boss, roomController);
        else if (room.roomType == RoomType.Combat)
            AddWaypoint(WaypointType.Combat, roomController);
        else if (room.roomType == RoomType.Health)
            AddWaypoint(WaypointType.Health, roomController);
        else if (room.roomType == RoomType.Treasure)
            AddWaypoint(WaypointType.Treasure, roomController);
        else
        {
            float chance = UnityEngine.Random.Range(0, 1);
            if (chance < 0.75f)
                AddMobSpawners();
        }
    }

    public void AddWaypoint(WaypointType type, RoomController room, float radius = 0)
    {
        Waypoint waypoint = new GameObject($"Waypoint", typeof(Waypoint))
            .GetComponent<Waypoint>();
        
        waypoint.waypointType = type;
        waypoint.transform.SetParent(room.transform, false);
        // To do: Create random position based on given radius (from center);
        waypoint.transform.position = room.transform.position;
    }

    public void AddMobSpawners()
    {

    }
}
