using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputData", menuName = "Create/Input/InputData")]
public class InputData : ScriptableObject
{
    static InputData _instance;
    public static InputData instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<InputData>("InputData");
            
            return _instance;
        }
    }

    public InputActionAsset asset;

    public class ActionMapProxy
    {
        public InputActionMap actionMap;
        public ActionMapProxy(InputActionAsset asset, string name)
        {
            actionMap = asset.FindActionMap(name);
        }
    }

    public class GameplayActionMapProxy : ActionMapProxy
    {
        public InputAction movement;
        public InputAction rotate;
        public InputAction fire;
        public InputAction dash;
        public InputAction pause;
        public InputAction interact;
        public InputAction mouseDelta;

        public GameplayActionMapProxy(InputActionAsset asset) : base(asset, "Gameplay")
        {
            movement = actionMap.FindAction("Movement");
            rotate = actionMap.FindAction("Rotate");
            fire = actionMap.FindAction("Fire");
            dash = actionMap.FindAction("Dash");
            interact = actionMap.FindAction("Interact");
            pause = actionMap.FindAction("Pause");
            mouseDelta = actionMap.FindAction("MouseDelta");
        }
    }

    GameplayActionMapProxy _game;
    public GameplayActionMapProxy game => _game ?? (_game = new GameplayActionMapProxy(asset));

    public class UIActionMapProxy : ActionMapProxy
    {
        InputAction mousePosition;
        InputAction mouseLeft;
        InputAction submit;
        InputAction cancel;
        InputAction move;

        public UIActionMapProxy(InputActionAsset asset) : base(asset, "UI")
        {
            mousePosition = actionMap.FindAction("MousePosition");
            mouseLeft = actionMap.FindAction("MouseLeft");
            submit = actionMap.FindAction("Submit");
            cancel = actionMap.FindAction("Cancel");
            move = actionMap.FindAction("Move");
        }
    }

    UIActionMapProxy _ui;
    public UIActionMapProxy ui => _ui ?? (_ui = new UIActionMapProxy(asset));
}
