using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class UIManager : MonoBehaviour
{
    public new Camera camera { get; private set; }
    public float screenHeight { get; private set; }
    public float screenWidth { get; private set; }

    SpriteAtlas spriteAtlas;

    void Awake()
    {
        camera = Camera.main;
        screenHeight = 2f * camera.orthographicSize;
        screenWidth = screenHeight * camera.aspect;
    }

    public async void LoadAssets()
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<RoomConfig>("SpriteAtlas");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            // roomConfig = (RoomConfig)handle.Result;
            Debug.Log("Success!");
            
        Addressables.Release(handle);
    }
}
