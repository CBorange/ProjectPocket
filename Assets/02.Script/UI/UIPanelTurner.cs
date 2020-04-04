using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelTurner : MonoBehaviour
{
    #region Singleton
    private static UIPanelTurner instance;
    public static UIPanelTurner Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UIPanelTurner>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("UIPanelTurner").AddComponent<UIPanelTurner>();
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
    private void Awake()
    {
        var objs = FindObjectsOfType<UIPanelTurner>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public GameObject UIPanelTurnerButtons;
    public InventoryPanel InventoryPanel;
    public GameObject PlayerInfoPanel;
    public GameObject SettingPanel;
    public LoadingPanel LoadingPanel;
    public NPCDialog_Panel dialog_Panel;
    public NPC_QuestPanel npc_QuestPanel;

    public void Open_UIPanelTurnerBtns()
    {
        if (UIPanelTurnerButtons.activeSelf)
            UIPanelTurnerButtons.SetActive(false);
        else
            UIPanelTurnerButtons.SetActive(true);
    }
    public void Open_Setting()
    {
        SettingPanel.SetActive(true);
    }
    public void Open_PlayerStatus()
    {
        PlayerInfoPanel.SetActive(true);
    }
    public void Open_Invetory()
    {
        InventoryPanel.OpenPanel();
    }
    public void Open_LoadingPanel()
    {
        LoadingPanel.OpenPanel();
    }
    public void Open_NPCDialogPanel(NPCData talkingNPC)
    {
        dialog_Panel.OpenPanel(talkingNPC);
    }
    public void Open_NPC_QuestPanel(QuestData[] questDatas)
    {
        npc_QuestPanel.OpenPanel(questDatas);
    }
}
