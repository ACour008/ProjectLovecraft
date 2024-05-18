using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public abstract bool CanExecute();
    public abstract void Execute();
}