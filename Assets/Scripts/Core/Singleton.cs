using System;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Lazily-constructed C# singleton (i.e., not a Unity object). The singleton automatically
/// resets when the domain is reloaded and when exiting play mode.
/// 
/// If the class implements IDisposable, it will be disposed automatically on reload.
/// </summary>
public abstract class Singleton<T> : IClearOnLoad
    where T : new()
{
    [ClearOnReload(clearOnExitingPlayMode = true, clearOnExitingEditMode = true)]
    static T _instance;

    public static T instance => _instance ?? (_instance = new T());

    public static T instanceNoAlloc => _instance;

    public virtual bool ClearOnDomainReload(FieldInfo field) => true;
    public virtual bool ClearOnExitPlayMode(FieldInfo field) => true;
    public virtual bool ClearOnExitEditMode(FieldInfo field) => false; // By default, does not clear on exit edit mode
}