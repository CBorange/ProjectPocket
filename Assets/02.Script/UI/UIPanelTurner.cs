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

    // Panels
    public GameObject UIPanelTurnerButtons;
    public InventoryPanel InventoryPanel;
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
    public TutorialGuidePanel tutorialGuidePanel;
    public PlayerDeathPanel playerDeathPanel;
    public ExitGamePanel exitGamePanel;

    // Data
    private bool uiPanelCurrentOpen;
    public bool UIPanelCurrentOpen
    {
        get { return uiPanelCurrentOpen; }
    }
    
    private IEnumerator IE_WaitPanelClose(GameObject panelObj)
    {
        uiPanelCurrentOpen = true;
        yield return new WaitUntil(() => !panelObj.activeSelf);
        uiPanelCurrentOpen = false;
    }

    public void Open_UIPanelTurnerBtns()
    {
        if (UIPanelTurnerButtons.activeSelf)
            UIPanelTurnerButtons.SetActive(false);
        else
            UIPanelTurnerButtons.SetActive(true);
    }

    // Full Screen Use Panel
    public void Open_Invetory()
    {
        InventoryPanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(InventoryPanel.gameObject));
    }
    public void Open_LoadingPanel()
    {
        LoadingPanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(LoadingPanel.gameObject));
    }
    public void Open_NPCDialogPanel(NPCData talkingNPC, NPC_Controller controller)
    {
        dialog_Panel.OpenPanel(talkingNPC, controller);
        StartCoroutine(IE_WaitPanelClose(dialog_Panel.gameObject));
    }
    public void Open_ShopPanel(NPCData currentNPC)
    {
        ShopPanel.OpenPanel(currentNPC);
        StartCoroutine(IE_WaitPanelClose(ShopPanel.gameObject));
    }
    public void Open_NPC_QuestPanel(NPCData currentNPC)
    {
        npc_QuestPanel.OpenPanel(currentNPC);
        StartCoroutine(IE_WaitPanelClose(npc_QuestPanel.gameObject));
    }
    public void Open_Player_QuestPanel()
    {
        player_QuestPanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(player_QuestPanel.gameObject));
    }
    public void Open_PlayerStatPanel()
    {
        playerStatPanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(playerStatPanel.gameObject));
    }
    public void Open_BuildingUpgradePanel(BuildingData data, StructureBuilder builder)
    {
        buildingUpgradePanel.OpenPanel(data, builder);
        StartCoroutine(IE_WaitPanelClose(buildingUpgradePanel.gameObject));
    }
    public void Open_TutorialGuidePanel()
    {
        tutorialGuidePanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(tutorialGuidePanel.gameObject));
    }
    public void Open_PlayerDeathPanel(float beforeExp, float lossGold)
    {
        playerDeathPanel.OpenPanel(beforeExp, lossGold);
        StartCoroutine(IE_WaitPanelClose(playerDeathPanel.gameObject));
    }
    public void Open_ExitGamePanel()
    {
        exitGamePanel.OpenPanel();
        StartCoroutine(IE_WaitPanelClose(exitGamePanel.gameObject));
    }

    // Semi Screen Use Panel
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
    public void Open_UniveralNoticePanel(string title, string contents, float viewTime)
    {
        universalNoticePanel.OpenPanel(title, contents, viewTime);
    }
    
    
}
