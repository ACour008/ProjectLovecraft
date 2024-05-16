using UnityEngine.InputSystem;

public abstract class GameMode
{
    public virtual InputActionMap inputActionMap => InputData.instance.ui.actionMap;
    public bool isActive => GameModeManager.instance.currentMode == this;

    public virtual void OnBegin() {}
    public virtual void OnEnd() {}
    public virtual void OnActivate()
    {
        inputActionMap?.Enable();
    }
    public virtual void OnDeactivate()
    {
        inputActionMap?.Disable();
    }
    public virtual void OnUpdate() {}
}
