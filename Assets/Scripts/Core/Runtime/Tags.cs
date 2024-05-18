// Generated from TagsGenerator.cs

public static class Tags
{
    public const string Untagged = nameof(Untagged);
    public const string Respawn = nameof(Respawn);
    public const string Finish = nameof(Finish);
    public const string EditorOnly = nameof(EditorOnly);
    public const string MainCamera = nameof(MainCamera);
    public const string Player = nameof(Player);
    public const string GameController = nameof(GameController);
}

public static class Layers
{
    public const int Default = 0;
    public const int TransparentFX = 1;
    public const int IgnoreRaycast = 2;
    public const int Player = 3;
    public const int Water = 4;
    public const int UI = 5;
    public const int Wall = 6;
    public const int CombatZone = 7;
    public const int LeaveTrigger = 8;
}
public static class LayerMasks
{
    public const int Default = 1 << Layers.Default;
    public const int TransparentFX = 1 << Layers.TransparentFX;
    public const int IgnoreRaycast = 1 << Layers.IgnoreRaycast;
    public const int Player = 1 << Layers.Player;
    public const int Water = 1 << Layers.Water;
    public const int UI = 1 << Layers.UI;
    public const int Wall = 1 << Layers.Wall;
    public const int CombatZone = 1 << Layers.CombatZone;
    public const int LeaveTrigger = 1 << Layers.LeaveTrigger;
}
