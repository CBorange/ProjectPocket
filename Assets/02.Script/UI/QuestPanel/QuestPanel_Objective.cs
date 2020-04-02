using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel_Objective : MonoBehaviour
{
    // Quest Objective UI
    public Text ContentsText;
    public Text ImpliedObjectiveText;
    public Text DetailedObjectiveText;

    // Buttons
    public Button Accept_Btn;
    public Button Complete_Btn;

    // Data
    private QuestData currnetData;

    public void Initialize()
    {

    }

    public void RefreshToAcceptable(QuestData data)
    {
        DeactiveAllButton();
        currnetData = data;
        Accept_Btn.gameObject.SetActive(true);

        // 퀘스트 설명
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < currnetData.QuestIntroduce.Length; ++i)
        {
            builder.Append(currnetData.QuestIntroduce[i]);
            builder.AppendLine();
        }
        ContentsText.text = builder.ToString();

        // 퀘스트 목표
        ImpliedObjectiveText.text = currnetData.QuestIntroduce_Implied;
        builder = new StringBuilder();
        for (int categoryIdx = 0; categoryIdx < data.QuestCategorys.Length; ++categoryIdx)
        {
            switch(currnetData.QuestCategorys[categoryIdx])
            {
                case "Discussion":
                    builder.Append("대화 :");
                    builder.AppendLine();

                    TargetNPCData[] targetNPC = currnetData.Behaviour_Discussion.TargetNPC;
                    for (int i = 0; i < targetNPC.Length; ++i)
                    {
                        builder.Append($"[{targetNPC[i].NPCName}]와 대화 하세요.");
                        builder.AppendLine();
                    }
                    break;
                case "KillMonster":
                    builder.Append("몬스터 사냥 :");
                    builder.AppendLine();

                    TargetMonsterData[] targetsMob = currnetData.Behaviour_KillMonster.TargetMonster;
                    for (int i = 0; i < targetsMob.Length; ++i)
                    {
                        builder.Append($"[{targetsMob[i].MonsterName}]을(를) [{targetsMob[i].KillCount}]마리 잡으세요.");
                        builder.AppendLine();
                    }
                    break;
                case "Building":
                    builder.Append("몬스터 사냥 :");
                    builder.AppendLine();

                    TargetBuildingData[] targetBuilding = currnetData.Behaviour_Building.TargetBuilding;
                    for (int i = 0; i < targetBuilding.Length; ++i)
                    {
                        builder.Append($"[{targetBuilding[i].BuildingName}]을(를) [{targetBuilding[i].BuildingGrade}]까지 업그레이드 하세요.");
                        builder.AppendLine();
                    }
                    break;
                case "ItemGet":
                    builder.Append("몬스터 사냥 :");
                    builder.AppendLine();

                    TargetItemData[] targetItem = currnetData.Behaviour_GetItem.TargetItem;
                    for (int i = 0; i < targetItem.Length; ++i)
                    {
                        builder.Append($"[{targetItem[i].ItemName}]을(를) [{targetItem[i].ItemCount}]개 얻으세요.");
                        builder.AppendLine();
                    }
                    break;
            }
        }
        DetailedObjectiveText.text = builder.ToString();
    }
    public void RefreshToComplete(QuestData data)
    {
        currnetData = data;
        DeactiveAllButton();
    }
    private void DeactiveAllButton()
    {
        Accept_Btn.gameObject.SetActive(false);
        Complete_Btn.gameObject.SetActive(false);
    }
}
