using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WorldActor
{
    public List<Weapon> inventory = new List<Weapon>();
    public Weapon currentWeapon;
    public Weapon secondaryWeapon;

    public TreasureChest nearbyChest { get; set; }

    public Transform firePoint;

    public void Fire()
    {
        currentWeapon?.Fire();
    }

    public void Interact()
    {
        Debug.Log("Interact!");
        // can generalize this to an IWorldInteractable.
        if (nearbyChest != null)
            nearbyChest.Open(this);
    }

    public void AddWeapon(Weapon weapon)
    {
        if (inventory.Contains(weapon))
            return;

        inventory.Add(weapon);
        if (currentWeapon == null)
            EquipWeapon(weapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        Debug.Log($"{weapon} equipped");
        if (currentWeapon != null)
            UnequipWeapon(weapon);

        currentWeapon = weapon;
        currentWeapon.OnEquip(this);
    }

    public void UnequipWeapon(Weapon weapon)
    {
        currentWeapon.OnUnequip();
        currentWeapon = null;
    }

    public void DEBUG_ChangeWeapon()
    {
        Weapon nextWeapon = inventory[(inventory.IndexOf(currentWeapon) + 1) % inventory.Count];
        Debug.Log($"Next: {nextWeapon}");
        EquipWeapon(nextWeapon);
    }
}
