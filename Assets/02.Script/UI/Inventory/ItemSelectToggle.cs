using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectToggle : MonoBehaviour
{
    public int IndexInContentView;
    public Image ItemImage;
    public Text ItemName;
    public ItemData itemData;

    public void ItemSelected(bool selected)
    {
        if (!selected)
            return;
    }
}
