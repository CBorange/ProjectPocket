using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public float FallingLimit;
    public Transform PlayerStartPos;
    public Vector3[] SpecificPos;
    public MonsterSpawner[] Spawners;

    private void Start()
    {
        for (int i = 0; i < Spawners.Length; ++i)
        {
            Spawners[i].Initialize();
        }
    }
    private void FixedUpdate()
    {
        if (PlayerActManager.Instance.transform.position.y < FallingLimit)
        {
            PlayerStat.Instance.GetDamage(20);
            PlayerActManager.Instance.transform.position = PlayerStartPos.position;
        }
    }
}
