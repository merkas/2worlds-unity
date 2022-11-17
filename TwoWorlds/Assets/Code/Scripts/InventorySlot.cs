using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
     IPointerMoveHandler
{
    public Image icon;
    public Text stackNumber;
    Item item;

    public Text infoText; // Prefab
    public Text info;

    int numberOfItems;

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on inventory slot
    {
        if (item != null) // slot is filled
        {
            if (item.isConsumable)
            {
                if (item.isStackable)
                {
                    if (numberOfItems > 1)
                    {
                        numberOfItems -= 1;
                        if (numberOfItems < 2) stackNumber.enabled = false;

                        return;
                    }
                }
                ClearSlot();
            }
        }
    }
    public void OnPointerMove(PointerEventData pointerEventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (item != null)
        {
            info = Instantiate(infoText, this.gameObject.transform);
            info.transform.position += new Vector3(0, 50, 0);
            info.text = item.info;
            info.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (item != null)
        {
            info.enabled = false;
            Destroy(info);
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void AddToStack(int added)
    {
        numberOfItems += added;
        if (numberOfItems > 1)
        {
            stackNumber.text = numberOfItems.ToString();
            stackNumber.enabled = true;
        }
    }
}
