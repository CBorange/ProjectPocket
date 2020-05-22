using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathPanel : MonoBehaviour
{
    // UI
    public Text NoticeLossText;

    // Data


    public void OpenPanel(float beforeExp, float lossGold)
    {
        gameObject.SetActive(true);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("경험치 전부 손실");
        builder.AppendLine($"손실량 : <color=green>{beforeExp}</color>");
        builder.AppendLine("골드 일부 손실");
        builder.AppendLine($"손실량 : <color=yellow>{lossGold}</color>");
        builder.AppendLine();
        builder.AppendLine("마을로 귀환합니다.");

        NoticeLossText.text = builder.ToString();
    }
    public void ConfirmDeath()
    {
        gameObject.SetActive(false);
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
        MapLoader.Instance.LoadMap("FirstVillage", -1, true);
    }
}
