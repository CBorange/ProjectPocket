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
    public Text HPText;
    public Text MonsterNameText;

    public void Initialize(MonsterStat stat, GameObject monsterObj)
    {
        monsterStat = stat;
        MonsterObject = monsterObj;
        gameObject.SetActive(false);

        stat.Attach_ChangedCallback(ChangedStatus);
    }
    public void Respawn()
    {
        gameObject.SetActive(true);
        MonsterNameText.text = monsterStat.CurrentData.MonsterKorName;
        ChangedStatus();
    }
    public void Death()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.position = MonsterObject.transform.position + new Vector3(0, 2, 0);
    }
    private void ChangedStatus()
    {
        if (monsterStat == null)
            return;
        HPSlider.maxValue = monsterStat.MaxHealthPoint;
        HPSlider.value = monsterStat.HealthPoint;
        HPText.text = $"{monsterStat.HealthPoint} / {monsterStat.MaxHealthPoint}";
    }
}
