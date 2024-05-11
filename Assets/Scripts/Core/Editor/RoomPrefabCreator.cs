using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomPrefabCreator : MonoBehaviour
{
    [MenuItem("GameObject/Create new Room Prefab")]
    static void CreateRoomPrefab()
    {
        GameObject room = new GameObject("Room", typeof(Room), typeof(SpriteRenderer));
        Selection.activeGameObject = room;
    }
}
