using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusPanel : MonoBehaviour
{
    // UI
    public Slider HP_Slider;
    public Text HP_Text;
    public Slider EXP_Slider;
    public Text EXP_Text;
    public Slider WP_Slider;
    public Text WP_Text;
    public Text AP_Text;
    public Text SP_Text;
    public Text Gold_Text;

    public void Initialize()
    {
        PlayerStat.Instance.AttachUICallback(Changed_PlayerStat);
        Changed_PlayerStat();
    }
    // Open/Close Panel
    public void OpenPanel()
    {
        // UI 초기화
        gameObject.SetActive(true);

    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    private void Changed_PlayerStat()
    {
        PlayerStat stat = PlayerStat.Instance;

        float maxHealthPoint = stat.GetFloatStat("MaxHealthPoint");
        float healthPoint = stat.GetFloatStat("HealthPoint");
        float currentExperience = stat.GetFloatStat("CurrentExperience");
        float levelupExperience = stat.GetFloatStat("LevelupExperience");
        float maxWorkPoint = stat.GetFloatStat("MaxWorkPoint");
        float workPoint = stat.GetFloatStat("WorkPoint");
        float attackPoint = stat.GetFloatStat("AttackPoint");
        float shieldPoint = stat.GetFloatStat("ShieldPoint");
        int gold = stat.GetIntegerStat("Gold");
        // HP
        HP_Slider.maxValue = maxHealthPoint;
        HP_Slider.value = healthPoint;
        HP_Text.text = $"{healthPoint} / {maxHealthPoint}";
        // EXP
        EXP_Slider.maxValue = levelupExperience;
        EXP_Slider.value = currentExperience;
        EXP_Text.text = $"{currentExperience} / {levelupExperience}";
        // WP
        WP_Slider.maxValue = maxWorkPoint;
        WP_Slider.value = workPoint;
        WP_Text.text = $"{workPoint} / {maxWorkPoint}";
        // Stat
        AP_Text.text = attackPoint.ToString();
        SP_Text.text = shieldPoint.ToString();
        // Money
        Gold_Text.text = gold.ToString();
    }
}
