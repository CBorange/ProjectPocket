using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        player.transform.position = UserInfoProvider.Instance.LastPos;
    }
}
