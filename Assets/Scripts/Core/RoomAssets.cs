using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoomAssetInfo
{
    public Direction direction;
    public Sprite roomSprite;
    public int selectionWeight;
}

[CreateAssetMenu(fileName = "SpriteAssets", menuName = "Create/SpriteAssets")]
public class RoomAssets : ScriptableObject
{
    public List<RoomAssetInfo> allRooms;

    public Dictionary<Direction, Sprite> spriteLookup = new Dictionary<Direction, Sprite>();

    public void CreateSpriteLookup()
    {
        foreach (var assetInfo in allRooms)
        {
            Direction direction = assetInfo.direction;

            // Unity automatically converts "Everything" to -1, so change it back to All.
            if ((int)direction == -1)
                direction = Direction.All;

            spriteLookup[direction] = assetInfo.roomSprite;
        }
    }
}