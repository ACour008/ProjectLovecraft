using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RoomManager : MonoBehaviour
{
    public RoomConfig roomConfig { get; set; }
    private DungeonGenerator generator = new DungeonGenerator();
    public GameObject dungeon;
    public Dictionary<Room, RoomController> rooms = new Dictionary<Room, RoomController>();
    public RoomController currentRoom;

    public event Action<RoomController> RoomEntered;
    public event Action<RoomController, Direction> RoomExited;

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

        CreateRoom(generator.startRoom);
        // foreach (Room room in generator.rooms.Values)
        //     CreateRoom(room);
    }
    RoomController CreateRoom(Room room)
    {
        RoomController roomController = Instantiate(roomConfig.roomPrefab, dungeon.transform).GetComponent<RoomController>();
        roomController.Init(room, this, roomConfig);
        rooms[room] = roomController;
        return roomController;
    }

    public void OnDoorTriggered(RoomController controller, Direction direction)
    {
        // Room already created
        if (TryGetNeighbor(controller, direction, out RoomController neighbor))
        {
            currentRoom = neighbor;
            RoomExited?.Invoke(neighbor, direction);
        }
        // Create room
        else if (controller.room.neighbors.TryGetValue(direction, out Room nextRoom))
        {
            RoomController next = CreateRoom(nextRoom);
            currentRoom = next;
            RoomExited?.Invoke(next, direction);
        }
        else
            // Slam door shut w/ some screen shake.
            Debug.LogError($"Neighbor {direction} not found");
    }

    public bool TryGetNeighbor(RoomController controller, Direction direction, out RoomController neighbor)
    {
        return rooms.TryGetValue(controller.room.neighbors[direction], out neighbor);
    }

    public void OnRoomEntered()
    {
        currentRoom.CreateItems();
        RoomEntered?.Invoke(currentRoom);
    }
    public void OnRoomExited(RoomController neighbor, Direction direction)
    {
        RoomExited?.Invoke(neighbor, direction);
    }
}
