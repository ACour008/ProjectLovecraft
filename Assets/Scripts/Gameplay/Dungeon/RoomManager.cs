using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomManager : MonoBehaviour
{
    public Room startRoom;
    [HideInInspector] public Room endRoom;
    private Dictionary<Direction, Room> neighbors = new Dictionary<Direction, Room>();

    public void Start()
    {
        SpawnRooms();
    }

    public void SpawnRooms()
    {
        var startRoomEntrances = startRoom.entrances;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if ((startRoomEntrances & direction) != 0)
                Debug.Log($"Spawning new room at {direction}");
        }
    }

    public void AddNeighbor(Direction direction, Room neighbor)
    {
        if (!neighbors.TryAdd(direction, neighbor))
            Debug.Log($"Room already exists at direction {direction}.");
    }
}
