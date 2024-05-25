using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LootManager : MonoBehaviour
{
    LootPool lootPool;
    public List<WeaponProfile> allWeapons;
    List<WeaponProfile> availableWeapons;

    public Task LoadAssets()
    {
        TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
        LoadAssets(source);
        return source.Task;
    }
    
    async void LoadAssets(TaskCompletionSource<bool> source)
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<LootPool>("WeaponLootPool");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            lootPool = (LootPool)handle.Result;
            allWeapons = lootPool.weapons;
            availableWeapons = allWeapons;
        }

        Addressables.Release(handle);
        source.SetResult(true);
    }

    public Weapon GetRandomWeapon(Transform firePoint)
    {
        if (availableWeapons.Count == 0)
        {
            Debug.Log("Returning nothing");
            return null;
        }
        int index = Random.Range(0, availableWeapons.Count);
        WeaponProfile selectedProfile = availableWeapons[index];
        availableWeapons.RemoveAt(index);

        if (selectedProfile.bulletPrefab != null)
            return new PrefabShotWeapon(selectedProfile, firePoint);
        else if (selectedProfile.lineRendererPrefab != null)
            return new RaycastShotWeapon(selectedProfile, firePoint);
        else
            Debug.Log($"No profile named {selectedProfile}");

        return null;
    }

    public void ResetPool()
    {
        // also need to account for weapons the player has.
        availableWeapons = new List<WeaponProfile>(allWeapons);
    }
}
