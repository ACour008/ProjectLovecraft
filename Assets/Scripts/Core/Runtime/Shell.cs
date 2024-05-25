using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


enum LoadState { None, Loading, Loaded, Done }

[DefaultExecutionOrder(-100)]
public class Shell : MonoBehaviour
{
    public static Shell instance { get; private set; }
    public DungeonManager roomManager;
    public UIManager uiManager;
    public GameModeManager gameModeManager;
    public CameraManager cameraManager;
    public LootManager lootManager;
    
    /* Could put this in to a game data w/ all assets that need to be loaded.
    /* - input data
    /* - room config
    /* - player prefab
    /* We will see;
    */
    public InputData inputData;
    private GameObject _player;
    public Player player;

    LoadState loadState;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        Debug.Log("Shell init");
        instance = new GameObject("Shell",
            typeof(Shell),
            typeof(DungeonManager),
            typeof(UIManager),
            typeof(GameModeManager),
            typeof(CameraManager),
            typeof(LootManager)).GetComponent<Shell>();

        DontDestroyOnLoad(instance.gameObject);

        instance.roomManager = instance.gameObject.GetComponent<DungeonManager>();
        instance.uiManager = instance.gameObject.GetComponent<UIManager>();
        instance.gameModeManager = instance.gameObject.GetComponent<GameModeManager>();
        instance.cameraManager = instance.gameObject.GetComponent<CameraManager>();
        instance.lootManager = instance.gameObject.GetComponent<LootManager>();

        instance.LoadAllAssets();
    }

    async void LoadAllAssets()
    {
        loadState = LoadState.Loading;

        await lootManager.LoadAssets();
        await roomManager.LoadAssets();

        AsyncOperationHandle handle = Addressables.LoadAssetAsync<InputData>("InputData");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            inputData = (InputData)handle.Result;

        Addressables.Release(handle);
        loadState++;
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
            cameraManager.OnUpdate();
        }
    }

    public async void LoadPlayer()
    {
        if (_player)
            return;

        AsyncOperationHandle handle = Addressables.LoadAssetAsync<GameObject>("Player");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            _player = (GameObject)handle.Result;

        Addressables.Release(handle);

        _player = Instantiate(_player, Vector3.zero, Quaternion.identity, null);
        player = _player.GetComponent<Player>();
        player.EquipWeapon(new PrefabShotWeapon(lootManager.allWeapons[0], _player.transform.GetChild(0).GetChild(0)));
        var playerSprite = _player.GetComponentInChildren<SpriteRenderer>();
        playerSprite.sortingLayerName = "Player";
        playerSprite.sortingOrder = 0;
    }
}
