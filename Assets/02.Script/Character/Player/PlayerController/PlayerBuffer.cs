using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffer : MonoBehaviour
{
    #region Singleton
    private static PlayerBuffer instance;
    public static PlayerBuffer Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerBuffer>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerBuffer").AddComponent<PlayerBuffer>();
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
        var objs = FindObjectsOfType<PlayerBuffer>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public void ApplyStatEffectByExpendable(ExpendableData data)
    {
        ExpendableEffect[] effects = data.Effects;
        for (int i = 0; i < effects.Length; ++i)
        {
            if (effects[i].StatName.Equals("HealthPoint"))
                PlayerStat.Instance.Heal(effects[i].StatAmount);
        }
    }
}
