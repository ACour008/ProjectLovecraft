using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootPool", menuName = "Create/Items/Loot Pool")]
public class LootPool : ScriptableObject
{
    public List<WeaponProfile> weapons;
}