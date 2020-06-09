using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Threading.Tasks;

public class LoginUIManager : MonoBehaviour
{
    // 계정 변수
    public InputField ID_AccountInput;
    public InputField PW_AccountInput;

    // Alert 팝업
    public GameObject AlertPopup;
    public Text AlertText;

    // Loading 팝업
    public GameObject LoadingPopup;

    private void Start()
    {
    }
    public void Login()
    {
        LoginProcess();
    }
    private async void LoginProcess()
    {
        string id = ID_AccountInput.text;
        string pw = PW_AccountInput.text;

        ShowLoadingPopup();
        Task<string> accountTask = Task<string>.Factory.StartNew(
            () => DBConnector.Instance.ValiadeAccountOnDB(id, pw));
        await accountTask;
        string accountResult = accountTask.Result;
        if (accountResult.Equals("Success"))
        {
            Task<string> userInfoTask = Task<string>.Factory.StartNew(() => DBConnector.Instance.LoadUserInfo(id));
            await userInfoTask;
            string userResult = userInfoTask.Result;
            if (userResult.Equals("Success"))
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                CloseLoadingPopup();
                ShowAlertPopup($"접속 실패\n\n{userResult}");
                ErrorLogWriter.Instance.WriteErrorLog(userResult, $"error.txt");
            }

        }
        else
        {
            CloseLoadingPopup();
            ShowAlertPopup($"접속 실패\n\n{accountResult}");
            ErrorLogWriter.Instance.WriteErrorLog(accountResult, $"error.txt");
        }
    }

    // Alert 팝업
    private void ShowAlertPopup(string text)
    {
        AlertText.text = text;
        AlertPopup.SetActive(true);
    }
    public void ConfirmAlertPopup()
    {
        AlertPopup.SetActive(false);
    }

    // Loading 팝업
    private void ShowLoadingPopup()
    {
        LoadingPopup.gameObject.SetActive(true);
    }
    private void CloseLoadingPopup()
    {
        LoadingPopup.gameObject.SetActive(false);
    }
}
