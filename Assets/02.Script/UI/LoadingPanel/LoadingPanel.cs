using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Text LoadingText;
    public void Initialize()
    {

    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetLoadingText_MapLoading(string mapName)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("맵을 로딩중입니다...");
        builder.AppendLine($"<color=orange>{mapName}</color>");
        LoadingText.text = builder.ToString();
    }
}
