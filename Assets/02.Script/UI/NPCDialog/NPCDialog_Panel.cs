using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPCDialog_Panel : MonoBehaviour
{
    // UI
    public Text NPC_NameText;
    public Text NPC_IntroduceText;
    public Button ShopBtn;
    public Button QuestBtn;
    public Button DisccusionBtn;
    public Button CloseBtn;

    // Data
    private NPCData currentNPC;
    private Dictionary<string, Action> buttonActiveFunctions;

    // Panels
    public NPCDialog_DiscussionController DiscussionController;

    public void Initialize()
    {
        buttonActiveFunctions = new Dictionary<string, Action>();
        buttonActiveFunctions.Add("Quest", ActiveQuestBtn);
        buttonActiveFunctions.Add("Shop", ActiveShopBtn);
        buttonActiveFunctions.Add("Discussion", ActiveDisccusionBtn);
    }
    public void OpenPanel(NPCData data)
    {
        currentNPC = data;
        gameObject.SetActive(true);
        RefreshPanel();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void RefreshPanel()
    {
        NPC_NameText.text = currentNPC.Name;
        NPC_IntroduceText.text = currentNPC.Introduce;

        // Refresh Button
        DeactiveAllButton();

        CloseBtn.gameObject.SetActive(true);
        if (currentNPC.Behaviours.Length == 0)
            return;
        for (int i = 0; i < currentNPC.Behaviours.Length; ++i)
        {
            buttonActiveFunctions[currentNPC.Behaviours[i]]();
        }
    }

    // Button Actives
    public void DeactiveAllButton()
    {
        ShopBtn.gameObject.SetActive(false);
        QuestBtn.gameObject.SetActive(false);
        DisccusionBtn.gameObject.SetActive(false);
        CloseBtn.gameObject.SetActive(false);
    }
    private void ActiveQuestBtn()
    {
        QuestBtn.gameObject.SetActive(true);
    }
    private void ActiveShopBtn()
    {
        ShopBtn.gameObject.SetActive(true);
    }
    private void ActiveDisccusionBtn()
    {
        DisccusionBtn.gameObject.SetActive(true);
    }

    // Button method
    public void OpenQuestPanel()
    {
        UIPanelTurner.Instance.Open_NPC_QuestPanel(currentNPC);
    }

    public void StartDiscussion()
    {
        DeactiveAllButton();
        string[] questDiscussion = PlayerQuest.Instance.GetDiscussionWhenTalkToNPC(currentNPC.NPCCode);
        if (questDiscussion == null)
            DiscussionController.StartDiscussion(currentNPC.Disccusion);
        else
        {
            NPC_ControllerGroup.Instance.QuestStateWasChanged();
            DiscussionController.StartDiscussion(questDiscussion);
        }
    }
}
