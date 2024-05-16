using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


enum LoadState { None, Loading, Loaded, Done }

[DefaultExecutionOrder(-100)]
public class Shell : MonoBehaviour
{
    public static Shell instance { get; private set; }
    public RoomManager roomManager;
    public UIManager uiManager;
    public GameModeManager gameModeManager;
    
    /* Could put this in to a game data w/ all assets that need to be loaded.
    /* - input data
    /* - room config
    /* - player prefab
    /* We will see;
    */
    public InputData inputData;
    public GameObject player;
    
    LoadState loadState;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        Debug.Log("Shell init");
        instance = new GameObject("Shell",
            typeof(Shell),
            typeof(RoomManager),
            typeof(UIManager),
            typeof(GameModeManager)).GetComponent<Shell>();

        DontDestroyOnLoad(instance.gameObject);

        instance.roomManager = instance.gameObject.GetComponent<RoomManager>();
        instance.uiManager = instance.gameObject.GetComponent<UIManager>();
        instance.gameModeManager = instance.gameObject.GetComponent<GameModeManager>();

        instance.LoadAllAssets();
    }

    async void LoadAllAssets()
    {
        loadState = LoadState.Loading;
        await roomManager.LoadAssets();

        AsyncOperationHandle handle = Addressables.LoadAssetAsync<InputData>("InputData");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            inputData = (InputData)handle.Result;

        Addressables.Release(handle);
        loadState++;
    }
    
    void OnDisable()
    {
        gameModeManager.EndPlayMode();
    }

    void Update()
    {
        if (loadState == LoadState.Loaded)
        {
            gameModeManager.BeginMode(new StandardGameMode());
            loadState++;
        }

        if (loadState == LoadState.Done)
        {
            gameModeManager.OnUpdate();
        }
    }

    public async void LoadPlayer()
    {
        if (player)
            return;

        AsyncOperationHandle handle = Addressables.LoadAssetAsync<GameObject>("Player");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            player = (GameObject)handle.Result;

        Addressables.Release(handle);

        player = Instantiate(player, Vector3.zero, Quaternion.identity, null);
        var playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        playerSprite.sortingLayerName = "Default";
        playerSprite.sortingOrder = 999;
    }
}
