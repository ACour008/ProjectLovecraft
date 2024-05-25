using UnityEngine;

public class WeaponsChest : MonoBehaviour, Interactable
{
    [SerializeField] GameObject open;
    [SerializeField] GameObject closed;

    bool isOpen;

    void Start()
    {
        if (!isOpen)
            open.SetActive(false);
            closed.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            Debug.Log("Entered Chest area");
            player.nearbyChest = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            Debug.Log("Leaving Chest area");
            player.nearbyChest = null;
        }
    }

    void Open(WorldActor player)
    {
        // check if player is in combat.
        
        if (!isOpen)
        {
            Debug.Log("Opening chest");
            isOpen = true;
            
            closed.SetActive(false);
            open.SetActive(true);
            
            Weapon newWeapon = Shell.instance.lootManager.GetRandomWeapon(player.firePoint);
            if (newWeapon != null)
            {
                // Play show weapon animations
                newWeapon.OnInteract(player);
            }
            else
                Debug.Log("You found nothing");
        }
        else
        {
            Debug.Log("Already open");
        }
    }

    public void OnInteract(WorldActor actor)
    {
        Open(actor);
    }
}
