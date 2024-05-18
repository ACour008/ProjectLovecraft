using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProfile", menuName = "Create/Items/Weapon Profile")]
public class WeaponProfile : ScriptableObject
{
    public bool usesRaycast;
    public string weaponName;
    public int damage;
    public int startingRounds;
    public int maxRounds;
    public GameObject bulletPrefab;
    public LineRenderer lineRendererPrefab;
    public float lineLength;
    public float blastLifetime = 0.3f;
}
