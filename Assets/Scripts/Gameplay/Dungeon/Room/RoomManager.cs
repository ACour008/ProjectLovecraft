using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RoomManager : MonoBehaviour
{
    public RoomConfig roomConfig { get; set; }
    private DungeonGenerator generator = new DungeonGenerator();
    public GameObject dungeon;

    public Task LoadAssets()
    {
        TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
        LoadAssets(source);
        return source.Task;
    }
    async void LoadAssets(TaskCompletionSource<bool> source)
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<RoomConfig>("RoomConfig");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            roomConfig = (RoomConfig)handle.Result;

        Addressables.Release(handle);
        source.SetResult(true);
    }

    public void Generate()
    {
        generator.Generate();

        dungeon = new GameObject("Dungeon");

        foreach (Room room in generator.rooms.Values)
            CreateRoom(room);
    }
    public void CreateRoom(Room room)
    {
        RoomController roomController = Instantiate(roomConfig.roomPrefab, dungeon.transform).GetComponent<RoomController>();
        roomController.Init(room, roomConfig);

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
