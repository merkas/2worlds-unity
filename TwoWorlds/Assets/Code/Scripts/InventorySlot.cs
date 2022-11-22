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
    InventoryUI inventoryUI;
    public Text infoText; // Prefab
    public Text info;

    bool itemOnMove;
    int numberOfItems;

    bool selectedSlot;
    GameObject otherSlot;
    bool itemActive;

    int newItemNumber;
    Item newItem;

    void Start()
    {
        //InventoryUI.moveInventoryObject += MoveObjectInSlot;
        InventoryUI.inventoryClosed += UiDeactivated;
        // add method to unsubscribe?
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
    }
    

    public void GetSecondObj(GameObject otherObj)
    {
        otherSlot = otherObj;
        MoveObjectInSlot();
    }

    void UiDeactivated()
    {
        selectedSlot = false;
    }

    public void MoveObjectInSlot() // and other things
    {
        
        if (selectedSlot == true)
        {
            //InventorySlot other = otherSlot.GetComponent<InventorySlot>();
            //if (other.item != null) // both slots have an item
            //{
            //    newItem = other.item;
            //    newItemNumber = other.numberOfItems;
            //    // Move object to other selected slot
            //    other.ExchangeItem(item, numberOfItems);

            //    ClearSlot();
            //    AddItem(newItem);
            //    numberOfItems = newItemNumber;
            //}
            //else if (other.item == null)
            //{
            //    other.ExchangeItem(item, numberOfItems);
            //    AddToStack(-numberOfItems);
            //    ClearSlot();
            //}
        }
    }

    public void ExchangeItem(Item newItem, int number)
    {
        //ClearSlot();
        //AddItem(newItem);
        //if (item.isStackable == true)
        //    AddToStack(number);
    }

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on inventory slot
    {

        //inventoryUI.ClickedInInventory(gameObject);
        if (item != null) // slot is filled
        {
            selectedSlot = true;
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


                inventoryUI.ShowActiveItem(item, gameObject);

                itemActive = true;
        }
        //else inventoryUI.EmptySlot(gameObject);
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
        stackNumber.enabled = false;
        itemActive = false;
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
