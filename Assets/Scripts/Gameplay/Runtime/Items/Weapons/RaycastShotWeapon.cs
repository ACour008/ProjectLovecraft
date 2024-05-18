using System.Collections;
using UnityEngine;

public class RaycastShotWeapon : Weapon
{
    public LineRenderer lineRenderer;
    public float lineLength = 100f;

    public RaycastShotWeapon(WeaponProfile profile, Transform firePoint) : base(profile, firePoint) { }

    public override void OnEquip(WorldActor actor)
    {
        lineRenderer = GameObject.Instantiate(profile.lineRendererPrefab).GetComponent<LineRenderer>();
        lineRenderer.gameObject.SetActive(false);
    }

    public override void OnUnequip()
    {
        GameObject.Destroy(lineRenderer);
    }

    public override void Fire()
    {
        base.Fire();
        if (canFire)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.SetPosition(0, firePoint.position);

            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
            if (hitInfo)
            {
                lineRenderer.SetPosition(1, hitInfo.point);
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                if (enemy)
                    enemy.TakeDamage(damage);
            }
            
            Vector3 direction = Shell.instance.player.transform.GetChild(0).up;
            lineRenderer.SetPosition(1, firePoint.position + (direction * lineLength));
            Shell.instance.player.StartCoroutine(WaitTilBlastEnd());
        }
    }

    public IEnumerator WaitTilBlastEnd()
    {
        yield return new WaitForSeconds(profile.blastLifetime);
        lineRenderer.gameObject.SetActive(false);
    }

    public override void OnInteract(WorldActor actor)
    {
        actor.AddWeapon(this);
    }
}
