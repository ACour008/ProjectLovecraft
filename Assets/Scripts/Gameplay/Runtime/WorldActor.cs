using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public abstract class WorldActor : MonoBehaviour
{
    public CharacterProfile profile;
    public List<Weapon> inventory = new List<Weapon>();
    public Weapon currentWeapon;
    public Weapon secondaryWeapon;

    public Transform currentTarget;

    public WeaponsChest nearbyChest { get; set; }

    public Transform firePoint;
    [SerializeField] LineRenderer _weaponLineRenderer;

    public LineRenderer weaponLineRenderer
    {
        get => _weaponLineRenderer;
        set => _weaponLineRenderer = value;
    }
    public virtual void TakeDamage(int damage)
    {
    }

    public void Fire()
    {
        currentWeapon?.Fire();
    }

    public void Interact()
    {
        Debug.Log("Interact!");
        if (nearbyChest != null)
            nearbyChest.OnInteract(this);
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
