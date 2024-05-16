using System.Collections.Generic;
using UnityEngine;

public class NullMode : GameMode
{

}


public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;

    // This needs to go into shell, when you make one. Make it a simple Shell.
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    List<GameMode> gameModes = new List<GameMode>();
    public GameMode currentMode => gameModes.Count > 0 ? gameModes[^1] : null;
    public GameMode activeMode;

    // call in shell (when play mode is pressed)
    public void BeginPlayMode()
    {
        gameModes.Clear();
        gameModes.Add(new NullMode());
    }

    // shell.OnDisable
    public void EndPlayMode()
    {
        while (!(currentMode is NullMode))
            EndMode(currentMode);
    }

    // called in shell again.
    public void OnUpdate()
    {
        ActivateCurrentMode();
        activeMode?.OnUpdate();
    }

    public void BeginMode(GameMode mode)
    {
        DeactivateCurrentMode();
        gameModes.Add(mode);
        mode.OnBegin();
    }

    public void EndMode(GameMode mode)
    {
        DeactivateCurrentMode();
        mode.OnEnd();
        gameModes.RemoveAt(gameModes.Count - 1);
    }

    void ActivateCurrentMode()
    {
        if (currentMode != activeMode)
        {
            activeMode = currentMode;
            activeMode?.OnActivate();
        }
    }

    void DeactivateCurrentMode()
    {
        if (activeMode != null)
        {
            activeMode?.OnDeactivate();
            activeMode = null;
        }
    }

    public T GetMode<T>() where T : GameMode
    {
        for (int i = gameModes.Count - 1; i >= 0; --i)
        {
            if (gameModes[i] is T gameMode)
                return gameMode;
        }
        return null;
    }
}
