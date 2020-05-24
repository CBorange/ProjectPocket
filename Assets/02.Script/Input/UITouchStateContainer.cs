using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITouchStateContainer : MonoBehaviour
{
    #region Singleton
    private static UITouchStateContainer instance;
    public static UITouchStateContainer Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UITouchStateContainer>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("UITouchStateContainer").AddComponent<UITouchStateContainer>();
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
        var objs = FindObjectsOfType<UITouchStateContainer>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // Data
    private bool possibleToControll = true;
    public bool PossibleToControll
    {
        get { return possibleToControll; }
        set { possibleToControll = value; }
    }
}
