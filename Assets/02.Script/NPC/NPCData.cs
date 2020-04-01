using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData
{
    // JSON Parsing
    public string Name;
    public int NPCCode;
    public string Introduce;
    public string[] Behaviours;
    public int[] Quest;
    public string[] Disccusion;
    public string[] Shop;


    // Quest
    [System.NonSerialized]
    public QuestData[] QuestDatas;

    public NPCData() { }
}
