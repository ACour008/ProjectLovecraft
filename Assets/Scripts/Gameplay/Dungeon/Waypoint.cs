using UnityEngine;

public enum WaypointType
{
    Player,
    Boss,
    Combat,
    Health,
    Treasure,
}
public class Waypoint : MonoBehaviour
{
    public WaypointType waypointType;
}
