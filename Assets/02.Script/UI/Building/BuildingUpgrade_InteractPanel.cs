using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgrade_InteractPanel : MonoBehaviour
{
    // Controller
    public BuildingUpgradePanel UpgradePanel;

    // UI
    public Text BuildingTitle;
    public Text ConstructionCostText;
    public Text BuildingStatText;
    public Button ConstructButton;
    public Text ConstructBtnText;

    // Data
    private BuildingData currentData;
    private StructureBuilder currentBuilder;
    private int selectedGrade;

    public void Initialize()
    {

    }
    public void OpenPanel(BuildingData data, StructureBuilder builder)
    {
        currentData = data;
        currentBuilder = builder;
    }
    public void ClosePanel()
    {

    }
    public void Refresh(int grade)
    {
        selectedGrade = grade;
        BuildingTitle.text = $"[{currentData.BuildingName}] <color=#FFA500FF>{selectedGrade + 1}</color> 단계";

        RefreshCost();
        RefreshStatText();

        BuildingStatus status = PlayerBuilding.Instance.GetBuildingStatus(currentData.BuildingCode);
        if (selectedGrade < status.Grade)
        {
            SetConstructButtonActive("상급건물 건설됨!", false);
            return;
        }
        else if (selectedGrade == status.Grade)
        {
            SetConstructButtonActive("이미 건설됨!", false);
            return;
        }

        int gradeDifference = selectedGrade - status.Grade;
        if (gradeDifference > 1)
        {
            SetConstructButtonActive("이전단계 건설 필요!", false);
            return;
        }
        BuildingCost[] costs = currentData.StatsByGrade[selectedGrade].ConstructionCost;
        for (int i = 0; i < costs.Length; ++i)
        {
            ItemData item = ItemDB.Instance.GetItemData(costs[i].NeedItem);

            InventoryItem needItem = null;
            if (PlayerInventory.Instance.AllItems.TryGetValue(item.ItemCode, out needItem))
            {
                if (needItem.ItemCount == costs[i].NeedItemCount)
                {
                    SetConstructButtonActive("건설!", true);
                    return;
                }
            }
            SetConstructButtonActive("재료 부족!", false);
        }
    }
    private void RefreshCost()
    {
        StringBuilder builder = new StringBuilder();
        BuildingCost[] costs = currentData.StatsByGrade[selectedGrade].ConstructionCost;

        builder.Append($"{currentData.StatsByGrade[selectedGrade].RequiredGold} 원");
        builder.AppendLine();
        for (int i = 0; i < costs.Length; ++i)
        {
            ItemData item = ItemDB.Instance.GetItemData(costs[i].NeedItem);
            builder.Append($"[{item.Name}] : {costs[i].NeedItemCount}");
            if (i < costs.Length - 1)
                builder.AppendLine();
        }
        ConstructionCostText.text = builder.ToString();
        builder.Clear();
    }
    private void RefreshStatText()
    {
        StringBuilder builder = new StringBuilder();
        StatAdditional[] stats = currentData.StatsByGrade[selectedGrade].BuildingStats;

        for (int i = 0; i < stats.Length; ++i)
        {
            builder.Append($"{UIText_Util.Instance.GetKorStatByEng(stats[i].StatName)} + {stats[i].StatValue}");
            if (i < stats.Length - 1)
                builder.AppendLine();
        }
        BuildingStatText.text = builder.ToString();
    }
    private void SetConstructButtonActive(string text, bool interactable)
    {
        ConstructBtnText.text = text;
        ConstructButton.interactable = interactable;
    }
    public void ConstructBuilding()
    {
        BuildingStatus status = PlayerBuilding.Instance.GetBuildingStatus(currentData.BuildingCode);
        status.Grade = selectedGrade;

        currentBuilder.CreateBuilding(status.BuildingCode);
        UpgradePanel.ClosePanel();
    }
}
