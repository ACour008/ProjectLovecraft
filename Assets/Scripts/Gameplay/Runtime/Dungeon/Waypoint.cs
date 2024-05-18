using UnityEngine;

public enum WaypointType
{
    PlayerStart,
    PlayerEnter,
    Boss,
    RoomSpecific,
    None,
}

public class Waypoint : MonoBehaviour
{
    public WaypointType waypointType;
    public int spawnRadius;

}
