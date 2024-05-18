using UnityEngine;

public abstract class Weapon : Interactable
{
    public WeaponProfile profile;
    public int damage;
    public int rounds;
    public int maxRounds;
    public Transform firePoint;
    public bool canFire => rounds > 0;

    public Weapon(WeaponProfile profile, Transform firePoint)
    {
        this.profile = profile;
        this.firePoint = firePoint;
        Init();
    }

    protected Weapon(WeaponProfile profile)
    {
        this.profile = profile;
    }

    void Init()
    {
        damage = profile.damage;
        rounds = profile.startingRounds;
        maxRounds = profile.maxRounds;
        OnInit();

    }

    public abstract void OnInteract(WorldActor actor);

    protected virtual void OnInit() { }

    public virtual void OnEquip(WorldActor actor) { }

    public virtual void OnUnequip() { }

    public virtual void Fire()
    {
        if (canFire)
            rounds--;
    }
}
