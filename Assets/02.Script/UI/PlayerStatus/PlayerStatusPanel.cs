using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusPanel : MonoBehaviour, UIPanel
{
    // UI
    public Slider HP_Slider;
    public Text HP_Text;
    public Slider EXP_Slider;
    public Text EXP_Text;
    public Text AP_Text;
    public Text SP_Text;

    public void Initialize()
    {
        PlayerStat.Instance.AttachUICallback(Changed_PlayerStat);
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

        // HP
        HP_Slider.maxValue = stat.Max_HealthPoint;
        HP_Slider.value = stat.HealthPoint;
        HP_Text.text = $"{stat.HealthPoint} / {stat.Max_HealthPoint}";
        // Stat
        AP_Text.text = stat.AttackPoint.ToString();
        SP_Text.text = stat.ShieldPoint.ToString();
        // EXP
        EXP_Slider.maxValue = stat.LevelupExperience;
        EXP_Slider.value = stat.CurrentExperience;
        EXP_Text.text = $"{stat.CurrentExperience} / {stat.LevelupExperience}";
    }
}
