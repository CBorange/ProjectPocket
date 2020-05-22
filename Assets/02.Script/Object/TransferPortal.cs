using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPortal : MonoBehaviour
{
    public string ConnectMapName;
    public int ConnectNextMapLoadPosIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PLAYER"))
            MapLoader.Instance.LoadMap(ConnectMapName, ConnectNextMapLoadPosIndex, false);
    }
}
