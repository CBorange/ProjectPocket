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
        if (PlayerCoordinator.Instance.PlayerPos.y < FallingLimit)
        {
            PlayerActManager.Instance.GetDamage(20);
            PlayerCoordinator.Instance.SetPlayerPosition(PlayerStartPos.position);
        }
    }
}
