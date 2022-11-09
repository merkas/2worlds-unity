using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    Item item;

    public Button clickOnItem;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        clickOnItem.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        clickOnItem.enabled = false;
    }

    public void ClickedOnItem()
    {
        // move object to mouse
        icon.transform.position = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            
        }
    }
}
