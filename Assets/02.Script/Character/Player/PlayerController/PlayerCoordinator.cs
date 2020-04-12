using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoordinator : MonoBehaviour
{
    #region Singleton
    private static PlayerCoordinator instance;
    public static PlayerCoordinator Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerCoordinator>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerCoordinator").AddComponent<PlayerCoordinator>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerCoordinator>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public Transform player;
    public void SetPlayerPosition(Vector3 pos)
    {
        player.transform.position = pos;
    }
}
