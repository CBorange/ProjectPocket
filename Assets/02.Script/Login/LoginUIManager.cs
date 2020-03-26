using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    // 계정 변수
    public InputField ID_AccountInput;
    public InputField PW_AccountInput;

    // 팝업
    public GameObject AlertPopup;
    public Text PopupText;

    private void Start()
    {
    }
    public void Login()
    {
        string id = ID_AccountInput.text;
        string pw = PW_AccountInput.text;

        string accountResult = DBConnector.Instance.ValiadeAccountOnDB(id, pw);
        if (accountResult.Equals("Success"))
        {
            string userResult = DBConnector.Instance.LoadUserInfo(id);
            if (userResult.Equals("Success"))
            {
                SceneManager.LoadScene("GameScene");
            }
            else
                ShowPopup($"접속할 수 없습니다.\n\n{userResult}");
        }
        else
            ShowPopup($"접속할 수 없습니다.\n\n{accountResult}");
    }

    // 팝업
    private void ShowPopup(string text)
    {
        PopupText.text = text;
        AlertPopup.SetActive(true);
    }
    public void ConfirmPopup()
    {
        AlertPopup.SetActive(false);
    }
}
