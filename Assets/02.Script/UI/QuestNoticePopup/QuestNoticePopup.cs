using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNoticePopup : MonoBehaviour
{
    #region Singleton
    private static QuestNoticePopup instance;
    public static QuestNoticePopup Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<QuestNoticePopup>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("QuestNoticePopup").AddComponent<QuestNoticePopup>();
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
    private void Start()
    {
        var objs = FindObjectsOfType<QuestNoticePopup>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        Initialize();
    }
    #endregion
    // Data
    private List<QuestNoticeText> deactiveTextPool;
    private List<QuestNoticeText> activeTextPool;

    private void Initialize()
    {
        deactiveTextPool = new List<QuestNoticeText>();
        activeTextPool = new List<QuestNoticeText>();

        for (int i = 0; i < transform.childCount; ++i)
            deactiveTextPool.Add(transform.GetChild(i).GetComponent<QuestNoticeText>());
        for (int i = 0; i < deactiveTextPool.Count; ++i)
        {
            deactiveTextPool[i].Initialize(ReturnFromActivePool);
            deactiveTextPool[i].gameObject.SetActive(false);
        }
    }
    public void PrintNotice_KillMonster(int questCode, int monsterCode, int curretKillCount, int goalKillCount)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            MonsterData monsterData = MonsterDB.Instance.GetMonsterData(monsterCode);
            deactiveTextPool[0].Refresh($"{questData.QuestName} : [<color=green>{monsterData.MonsterKorName}</color>] 사냥 ({curretKillCount} / {goalKillCount})");
            deactiveTextPool[0].transform.SetAsLastSibling();
            activeTextPool.Add(deactiveTextPool[0]);
            deactiveTextPool.RemoveAt(0);
        }
        else
        {
            activeTextPool[0].ReleaseText();
            PrintNotice_KillMonster(questCode, monsterCode, curretKillCount, goalKillCount);
        }
    }
    public void PrintNotice_Discussion(int questCode, int npcCode)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            NPCData npcData = NpcDB.Instance.GetNPCData(npcCode);
            deactiveTextPool[0].Refresh($"{questData.QuestName} : [<color=green>{npcData.Name}</color>] 대화 완료");
            deactiveTextPool[0].transform.SetAsLastSibling();
            activeTextPool.Add(deactiveTextPool[0]);
            deactiveTextPool.RemoveAt(0);
        }
        else
        {
            activeTextPool[0].ReleaseText();
            PrintNotice_Discussion(questCode, npcCode);
        }
    }
    public void PrintNotice_GetItem(int questCode, int itemCode, int curretItemCount, int goalItemCount)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            ItemData itemData = ItemDB.Instance.GetItemData(itemCode);
            deactiveTextPool[0].Refresh($"{questData.QuestName} : [<color=green>{itemData.Name}</color>] 획득 ({curretItemCount} / {goalItemCount})");
            deactiveTextPool[0].transform.SetAsLastSibling();
            activeTextPool.Add(deactiveTextPool[0]);
            deactiveTextPool.RemoveAt(0);
        }
        else
        {
            activeTextPool[0].ReleaseText();
            PrintNotice_GetItem(questCode, itemCode, curretItemCount, goalItemCount);
        }
    }
    public void PrintNotice_Building(int questCode, int buildingCode, int curretBuildingGrade, int goalBuildingGrade)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            BuildingData buildingData = BuildingDB.Instance.GetBuildingData(buildingCode);
            deactiveTextPool[0].Refresh($"{questData.QuestName} : [<color=green>{buildingData.BuildingName}</color>] 업그레이드 ({curretBuildingGrade + 1} / {goalBuildingGrade + 1})");
            deactiveTextPool[0].transform.SetAsLastSibling();
            activeTextPool.Add(deactiveTextPool[0]);
            deactiveTextPool.RemoveAt(0);
        }
        else
        {
            activeTextPool[0].ReleaseText();
            PrintNotice_Building(questCode, buildingCode, curretBuildingGrade, goalBuildingGrade);
        }
    }
    public void PrintNotice_QuestComplete(int questCode)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            deactiveTextPool[0].Refresh($"<color=orange>{questData.QuestName} 완료!</color>");
            deactiveTextPool[0].transform.SetAsLastSibling();
            activeTextPool.Add(deactiveTextPool[0]);
            deactiveTextPool.RemoveAt(0);
        }
        else
        {
            activeTextPool[0].ReleaseText();
            PrintNotice_QuestComplete(questCode);
        }
    }

    private void ReturnFromActivePool(QuestNoticeText text)
    {
        activeTextPool.Remove(text);
        deactiveTextPool.Add(text);
    }
}
