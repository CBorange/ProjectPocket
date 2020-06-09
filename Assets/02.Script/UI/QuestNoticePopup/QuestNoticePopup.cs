using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNoticePopup : MonoBehaviour, QuestObserver
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
            Debug.Log(gameObject.name);
            Destroy(gameObject);
            return;
        }
        Initialize();
    }
    #endregion

    // Data
    private Queue<QuestNoticeText> deactiveTextPool;
    private Queue<QuestNoticeText> activeTextPool;

    public void Initialize()
    {
        deactiveTextPool = new Queue<QuestNoticeText>();
        activeTextPool = new Queue<QuestNoticeText>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            QuestNoticeText text = transform.GetChild(i).GetComponent<QuestNoticeText>();
            text.Initialize(ReturnFromActivePool);
            text.gameObject.SetActive(false);
            deactiveTextPool.Enqueue(text);
        }
    }
    public void Update_KillMonster(int questCode, int monsterCode, int curretKillCount, int goalKillCount)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            MonsterData monsterData = MonsterDB.Instance.GetMonsterData(monsterCode);

            QuestNoticeText text = deactiveTextPool.Dequeue();
            text.Refresh($"{questData.QuestName} : [<color=green>{monsterData.MonsterKorName}</color>] 사냥 ({curretKillCount} / {goalKillCount})");
            text.transform.SetAsLastSibling();

            activeTextPool.Enqueue(text);
        }
        else
        {
            activeTextPool.Peek().ReleaseText();
            Update_KillMonster(questCode, monsterCode, curretKillCount, goalKillCount);
        }
    }
    public void Update_Discussion(int questCode, int npcCode)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            NPCData npcData = NpcDB.Instance.GetNPCData(npcCode);

            QuestNoticeText text = deactiveTextPool.Dequeue();
            text.Refresh($"{questData.QuestName} : [<color=green>{npcData.Name}</color>] 대화 완료");
            text.transform.SetAsLastSibling();

            activeTextPool.Enqueue(text);
        }
        else
        {
            activeTextPool.Peek().ReleaseText();
            Update_Discussion(questCode, npcCode);
        }
    }
    public void Update_GetItem(int questCode, int itemCode, int curretItemCount, int goalItemCount)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            ItemData itemData = ItemDB.Instance.GetItemData(itemCode);

            QuestNoticeText text = deactiveTextPool.Dequeue();
            text.Refresh($"{questData.QuestName} : [<color=green>{itemData.Name}</color>] 획득 ({curretItemCount} / {goalItemCount})");
            text.transform.SetAsLastSibling();

            activeTextPool.Enqueue(text);
        }
        else
        {
            activeTextPool.Peek().ReleaseText();
            Update_GetItem(questCode, itemCode, curretItemCount, goalItemCount);
        }
    }
    public void Update_Building(int questCode, int buildingCode, int curretBuildingGrade, int goalBuildingGrade)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);
            BuildingData buildingData = BuildingDB.Instance.GetBuildingData(buildingCode);

            QuestNoticeText text = deactiveTextPool.Dequeue();
            text.Refresh($"{questData.QuestName} : [<color=green>{buildingData.BuildingName}</color>] 업그레이드 ({curretBuildingGrade + 1} / {goalBuildingGrade + 1})");
            text.transform.SetAsLastSibling();

            activeTextPool.Enqueue(text);
        }
        else
        {
            activeTextPool.Peek().ReleaseText();
            Update_Building(questCode, buildingCode, curretBuildingGrade, goalBuildingGrade);
        }
    }
    public void Update_QuestComplete(int questCode)
    {
        if (deactiveTextPool.Count > 0)
        {
            QuestData questData = QuestDB.Instance.GetQuestData(questCode);

            QuestNoticeText text = deactiveTextPool.Dequeue();
            text.Refresh($"<color=orange>{questData.QuestName} 완료!</color>");
            text.transform.SetAsLastSibling();

            activeTextPool.Enqueue(text);
        }
        else
        {
            activeTextPool.Peek().ReleaseText();
            Update_QuestComplete(questCode);
        }
    }

    private void ReturnFromActivePool()
    {
        deactiveTextPool.Enqueue(activeTextPool.Dequeue());
    }
}
