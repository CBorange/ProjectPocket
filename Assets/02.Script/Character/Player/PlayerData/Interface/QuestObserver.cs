using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface QuestObserver
{
    void Update_KillMonster(int questCode, int monsterCode, int currentKillCount, int goalKillCount);
    void Update_Discussion(int questCode, int npcCode);
    void Update_GetItem(int questcode, int itemCode, int currentItemCount, int goalItemCount);
    void Update_Building(int questCode, int buildingCode, int curretBuildingGrade, int goalBuildingGrade);
    void Update_QuestComplete(int questCode);
}
