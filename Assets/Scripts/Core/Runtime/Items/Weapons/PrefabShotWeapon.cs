using UnityEngine;

public class PrefabShotWeapon : Weapon
{
    public GameObject bulletPrefab;

    public PrefabShotWeapon(WeaponProfile profile, Transform firePoint) : base(profile, firePoint) { }

    protected override void OnInit()
    {
        this.bulletPrefab = profile.bulletPrefab;
    }

    public override void Fire()
    {        
        base.Fire();
        if (canFire)
        {
            Bullet bullet = GameObject.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation)
                .GetComponent<Bullet>();
            bullet.weapon = this;
        }
    }
}
