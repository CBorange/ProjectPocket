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
    private Queue<ProjectileColiderBox> deactiveProjectilePool;
    private List<ProjectileColiderBox> activeProjectilePool;
    public void Execute()
    {
        if (deactiveProjectilePool.Count > 0)
        {
            ProjectileColiderBox colBox = deactiveProjectilePool.Dequeue();
            colBox.Refresh(playerTransform);
            colBox.Fire(playerTransform.forward, 20f);
            activeProjectilePool.Add(colBox);
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

        deactiveProjectilePool = new Queue<ProjectileColiderBox>();
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
            deactiveProjectilePool.Enqueue(newProjectile);
        }
    }
    private void ReturnToPool(ProjectileColiderBox projectile)
    {
        activeProjectilePool.Remove(projectile);
        deactiveProjectilePool.Enqueue(projectile);
    }
}
