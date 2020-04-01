using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog_DiscussionController : MonoBehaviour
{
    // UI
    public NPCDialog_Panel Dialog_Panel;
    public Text DicussionPrintText;
    public Button Next_Btn;
    public Button BackToPanel_Btn;

    // Data
    private string[] discussion;
    private int currentDiscussionIndex;

    public void StartDiscussion(string[] discussion)
    {
        this.discussion = discussion;
        currentDiscussionIndex = 0;

        Next_Btn.gameObject.SetActive(true);
        BackToPanel_Btn.gameObject.SetActive(true);

        RefreshDiscussionContents();
    }
    private void RefreshDiscussionContents()
    {
        DicussionPrintText.text = discussion[currentDiscussionIndex];
    }
    public void EndDiscussion()
    {
        Next_Btn.gameObject.SetActive(false);
        BackToPanel_Btn.gameObject.SetActive(false);
        Dialog_Panel.RefreshPanel();
    }
    public void NextDiscussion()
    {
        currentDiscussionIndex += 1;
        RefreshDiscussionContents();
        if (currentDiscussionIndex == discussion.Length - 1)
        {
            Next_Btn.gameObject.SetActive(false);
            return;
        }
    }
}
