using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomConfig : ScriptableObject
{
    public GameObject roomPrefab;
    public Vector2 roomSize;
    public float roomPixelsPerUnit;

    public Vector3 GetLocalScale()
    {
        return new Vector3
        (
            Shell.instance.uiManager.screenWidth / (roomSize.x / roomPixelsPerUnit),
            Shell.instance.uiManager.screenHeight / (roomSize.y / roomPixelsPerUnit),
            1f
        );
    }

    public Vector3 GetWorldPosition(Vector2Int dungeonCoordinates)
    {
        Vector3 localScale = GetLocalScale();

        return new Vector3
        (
            dungeonCoordinates.x * (roomSize.x / roomPixelsPerUnit) * localScale.x,
            dungeonCoordinates.y * (roomSize.y / roomPixelsPerUnit) * localScale.y,
            0
        ); 
    }
}