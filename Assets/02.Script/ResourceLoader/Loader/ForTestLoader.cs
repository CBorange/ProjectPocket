using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTestLoader : MonoBehaviour
{
    public bool isTest;
    public string TestAccount;
    private void Start()
    {
        if (isTest)
            DBConnector.Instance.LoadUserInfo(TestAccount);
    }
}
