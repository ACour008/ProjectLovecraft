using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoomAssetInfo
{
    public GameObject roomPrefab;
    public int selectionWeight;
}


public class RoomAssets : ScriptableObject
{

    public GameObject startRoom;
    public List<RoomAssetInfo> northRooms;
    public List<RoomAssetInfo> eastRooms;
    public List<RoomAssetInfo> westRooms;
    public List<RoomAssetInfo> southRooms;
}
