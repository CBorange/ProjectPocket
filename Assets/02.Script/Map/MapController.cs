using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform PlayerStartPos;
    public MonsterSpawner[] Spawners;

    private void Start()
    {
        for (int i = 0; i < Spawners.Length; ++i)
        {
            Spawners[i].Initialize();
        }
    }
}
