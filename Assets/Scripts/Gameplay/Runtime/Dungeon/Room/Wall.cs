using UnityEngine;

public class Wall : MonoBehaviour
{
    public Direction direction;
    [SerializeField] GameObject closed;
    [SerializeField] GameObject open;
    [SerializeField] GameObject door;
    public Waypoint entranceStart;

    public void SetOpen()
    {
        closed.SetActive(false);
        open.SetActive(true);
    }

    public void SetClosed()
    {
        closed.SetActive(true);
        open.SetActive(false);
    }
}
