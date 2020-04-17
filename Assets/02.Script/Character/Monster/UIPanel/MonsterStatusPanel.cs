using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatusPanel : MonoBehaviour
{
    // Controller
    private MonsterStat monsterStat;

    // Object
    private GameObject MonsterObject;

    // UI
    public Slider HPSlider;

    public void Initialize(MonsterStat stat, GameObject monsterObj)
    {
        monsterStat = stat;
        MonsterObject = monsterObj;

        stat.Attach_ChangedCallback(ChangedStatus);
    }
    private void OnEnable()
    {
        ChangedStatus();
    }
    private void FixedUpdate()
    {
        transform.position = MonsterObject.transform.position + new Vector3(0, 2, 0);
        transform.rotation = MonsterObject.transform.rotation;
    }
    private void ChangedStatus()
    {
        if (monsterStat == null)
            return;
        HPSlider.maxValue = monsterStat.MaxHealthPoint;
        HPSlider.value = monsterStat.HealthPoint;
    }
}
