using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomConfig : ScriptableObject
{
    public GameObject roomPrefab;
    public float roomSize;
    public float roomPixelsPerUnit;

    public Vector3 GetLocalScale()
    {
        return new Vector3
        (
            Shell.instance.uiManager.screenWidth / (roomSize / roomPixelsPerUnit),
            Shell.instance.uiManager.screenHeight / (roomSize / roomPixelsPerUnit),
            1f
        );
    }

    public Vector3 GetWorldPosition(Vector2Int dungeonCoordinates)
    {
        Vector3 localScale = GetLocalScale();

        return new Vector3
        (
            dungeonCoordinates.x * (roomSize / roomPixelsPerUnit) * localScale.x,
            dungeonCoordinates.y * (roomSize / roomPixelsPerUnit) * localScale.y,
            0
        ); 
    }
}