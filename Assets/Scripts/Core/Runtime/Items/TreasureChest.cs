using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public WeaponProfile weaponProfile;
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

    public void Open(Player player)
    {
        if (!isOpen)
        {
            Debug.Log("Opening chest");
            isOpen = true;
            // Play animations here.
            Weapon newWeapon = CreateWeapon(weaponProfile, player.firePoint);
            player.AddWeapon(newWeapon);
        }
    }

    public Weapon CreateWeapon(WeaponProfile profile, Transform firePoint)
    {
        if (profile.bulletPrefab != null)
            return new PrefabShotWeapon(profile, firePoint);
        else if (profile.lineRendererPrefab != null)
            return new RaycastShotWeapon(profile, firePoint);
        else
            throw new System.ArgumentException("Invalid weapon profile");
    }
}
