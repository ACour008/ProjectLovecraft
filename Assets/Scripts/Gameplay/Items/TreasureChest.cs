using UnityEngine;

public class TreasureChest : MonoBehaviour, Interactable
{
    public LootPool lootPool;
    public bool isOpen;


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
        if (!isOpen)
        {
            Debug.Log("Opening chest");
            isOpen = true;
            // Play open animations here.
            
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
