using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpendableEffect
{
    public string StatName;
    public int StatAmount;
    public int EffectDuration;
}
[System.Serializable]
public class ExpendableData : ItemData
{
    public ExpendableEffect[] Effects;
}
