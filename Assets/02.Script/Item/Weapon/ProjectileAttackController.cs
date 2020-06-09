using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackController : MonoBehaviour
{
    // Data
    private Transform playerTransform;
    private WeaponData data;
    private Color projectileTrailColor;

    // Pool
    private List<ProjectileColiderBox> deactiveProjectilePool;
    private List<ProjectileColiderBox> activeProjectilePool;
    public void Execute()
    {
        if (deactiveProjectilePool.Count > 0)
        {
            deactiveProjectilePool[0].Refresh(playerTransform);
            deactiveProjectilePool[0].Fire(playerTransform.forward, 20f);
            activeProjectilePool.Add(deactiveProjectilePool[0]);
            deactiveProjectilePool.RemoveAt(0);
        }
        else
        {
            activeProjectilePool[0].ForceReturnToPool();
            Execute();
        }
    }
    public void Initialize(Transform playerTrans, WeaponData data, Color bladeTrailColor)
    {
        playerTransform = playerTrans;
        this.data = data;
        projectileTrailColor = bladeTrailColor;

        deactiveProjectilePool = new List<ProjectileColiderBox>();
        activeProjectilePool = new List<ProjectileColiderBox>();
        CreateWavePool();
    }
    private void CreateWavePool()
    {
        GameObject projectilePrefab = AssetBundleCacher.Instance.LoadAndGetAsset("weapon", "WaveProjectileBox") as GameObject;
        for (int i = 0; i < 10; ++i)
        {
            ProjectileColiderBox newProjectile = Instantiate(projectilePrefab).GetComponent<ProjectileColiderBox>();
            newProjectile.transform.parent = transform;
            newProjectile.Initialize(projectileTrailColor, ReturnToPool);
            deactiveProjectilePool.Add(newProjectile);
        }
    }
    private void ReturnToPool(ProjectileColiderBox box)
    {
        deactiveProjectilePool.Add(box);
        activeProjectilePool.Remove(box);
    }
}
