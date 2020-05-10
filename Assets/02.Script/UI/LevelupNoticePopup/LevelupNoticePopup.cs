using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelupNoticePopup : MonoBehaviour
{
    // UI
    public Text NoticeText;

    public void OpenPanel(int level)
    {
        gameObject.SetActive(true);
        NoticeText.text = $"<color=orange>{level}</color> 레벨을 달성하였습니다!";

        Invoke("ClosePanel", 1.0f);
    }
    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
