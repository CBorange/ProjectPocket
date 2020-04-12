using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // Data
    public GameObject SpawnMobPrefab;
    public float SpawnArea;
    public float SpawnHeight;
    public float SpawnDelay;
    public int MaxSpawnCount;

    private List<GameObject> deactiveMobPool;
    private List<GameObject> activeMobPool;

    public void Initialize()
    {
        deactiveMobPool = new List<GameObject>();
        activeMobPool = new List<GameObject>();

        CreateMonsterPool();
        StartCoroutine(IE_SpawnCycle());
    }
    private void CreateMonsterPool()
    {
        for (int i = 0; i < MaxSpawnCount; ++i)
        {
            GameObject newMonster = Instantiate(SpawnMobPrefab, transform);
            newMonster.gameObject.SetActive(false);

            deactiveMobPool.Add(newMonster);
        }
    }
    private IEnumerator IE_SpawnCycle()
    {
        while (true)
        {
            if (activeMobPool.Count != MaxSpawnCount)
            {
                deactiveMobPool[0].SetActive(true);
                Vector3 spawnPos = CalculateSpawnCoord();
                deactiveMobPool[0].transform.position = spawnPos;
                deactiveMobPool[0].GetComponent<MonsterController>().SpawnCoord = spawnPos;
                deactiveMobPool.RemoveAt(0);

                activeMobPool.Add(deactiveMobPool[0]);
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
    private Vector3 CalculateSpawnCoord()
    {
        float posX = Random.Range(-SpawnArea, SpawnArea + 1);
        float posZ = Random.Range(-SpawnArea, SpawnArea + 1);
        Vector3 coord = transform.position;
        coord.x += posX;
        coord.z += posZ;
        coord.y = SpawnHeight;

        return coord;
    }
}
