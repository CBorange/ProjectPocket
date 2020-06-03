using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGamePanel : MonoBehaviour
{
    // Controller
    public PlayerDataSaver DataSaver;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void SaveAndExit()
    {
        ClosePanel();
        DataSaver.SaveAndPrintResult();
        QuitGameByPlatform();
    }
    public void NotSaveExit()
    {
        ClosePanel();
        QuitGameByPlatform();
    }
    private void QuitGameByPlatform()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
