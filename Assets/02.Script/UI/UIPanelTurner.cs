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
    public ShopPanel ShopPanel;
    public Player_QuestPanel player_QuestPanel;
    public ResourceInteractPanel resourceInteractPanel;
    public GatheringProgressPanel gatheringProgressPanel;
    public BuildingUpgradePanel buildingUpgradePanel;
    public BuildingInteractPanel buildingInteractPanel;
    public UniversalNoticePanel universalNoticePanel;
    public PlayerStatPanel playerStatPanel;

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
    public void Open_NPCDialogPanel(NPCData talkingNPC, NPC_Controller controller)
    {
        dialog_Panel.OpenPanel(talkingNPC, controller);
    }
    public void Open_ShopPanel(NPCData currentNPC)
    {
        ShopPanel.OpenPanel(currentNPC);
    }
    public void Open_NPC_QuestPanel(NPCData currentNPC)
    {
        npc_QuestPanel.OpenPanel(currentNPC);
    }
    public void Open_Player_QuestPanel()
    {
        player_QuestPanel.OpenPanel();
    }
    public void Open_ResourceInteractPanel(ResourceController controller, Vector3 resourceScreenPos)
    {
        resourceInteractPanel.OpenPanel(controller, resourceScreenPos);
    }
    public void Open_GatheringProgressPanel(ResourceController controller)
    {
        gatheringProgressPanel.OpenPanel(controller);
    }
    public void Open_BuildingInteractPanel(BuildingData data, BuildingController controller, Vector3 buildingScreenPos)
    {
        buildingInteractPanel.OpenPanel(data, controller, buildingScreenPos);
    }
    public void Open_BuildingUpgradePanel(BuildingData data, StructureBuilder builder)
    {
        buildingUpgradePanel.OpenPanel(data, builder);
    }
    public void Open_UniveralNoticePanel(string title, string contents, float viewTime)
    {
        universalNoticePanel.OpenPanel(title, contents, viewTime);
    }
    public void Open_PlayerStatPanel()
    {
        playerStatPanel.OpenPanel();
    }
}
