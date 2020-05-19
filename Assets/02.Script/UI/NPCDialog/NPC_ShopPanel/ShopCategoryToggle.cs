using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopCategoryToggle : MonoBehaviour
{
    // UI
    public Text CategoryNameText;

    // Data
    private string categoryType;
    public Action<string> selectCategoryCallback;

    public void Initialize(Action<string> selectCallback)
    {
        selectCategoryCallback = selectCallback;
    }

    public void Refresh(string category)
    {
        categoryType = category;
        CategoryNameText.text = UIText_Util.Instance.GetKorItemTypeByEng(category);
        gameObject.SetActive(true);
    }

    public void SelectCategory(bool select)
    {
        if (!select)
            return;
        selectCategoryCallback(categoryType);
    }
}
