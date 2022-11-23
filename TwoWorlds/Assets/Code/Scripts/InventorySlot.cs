using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Text stackNumber;
    CompleteItem comItem;
    Item item;
    InventoryUI inventoryUI;
    public Text infoText; // Prefab
    public Text info;

    public GameObject miniMenu; // Prefab
    GameObject mMenu;
    Button[] mMenuButtons;

    int numberOfItems;

    bool selectedSlot;
    GameObject otherSlot;
    bool itemActive;

    //int newItemNumber;
    //Item newItem;

    void Start()
    {
        //InventoryUI.moveInventoryObject += MoveObjectInSlot;
        InventoryUI.inventoryClosed += UiDeactivated;
        // add method to unsubscribe?
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
    }

    public bool ItemActive(bool active)
    {
        itemActive = active;
        return itemActive;
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

    //public void ExchangeItem(Item newItem, int number)
    //{
    //    //ClearSlot();
    //    //AddItem(newItem);
    //    //if (item.isStackable == true)
    //    //    AddToStack(number);
    //}

    public void OnPointerDown(PointerEventData pointerEventData) // player clicks on inventory slot
    {

        //inventoryUI.ClickedInInventory(gameObject);
        if (item != null) // slot is filled
        {
            if (Input.GetMouseButtonDown(0)) // Linksklick
            {
                if (mMenu != null) Destroy(mMenu); // auch wenn wo anders auf der UI geklickt wird einfügen!

                selectedSlot = true;
                inventoryUI.ShowActiveItem(item, gameObject);
                itemActive = true;
            }
            else if(Input.GetMouseButtonDown(1)) // Rechtsklick
            {
                // open menu
                mMenu = Instantiate(miniMenu, inventoryUI.transform); // not hidden behind other slots with this
                mMenu.transform.position = gameObject.transform.position; // position to slot position
                mMenu.transform.position += new Vector3(50, 50, 0); // offset

                // Instantiate Button
                mMenuButtons = mMenu.GetComponentsInChildren<Button>();
                mMenuButtons[0].onClick.AddListener(UseItem);
                mMenuButtons[1].onClick.AddListener(BackButton);

                if (item.isConsumable)
                {
                    // enable UseButton
                    mMenuButtons[0].enabled = true;
                }
                else mMenuButtons[0].enabled = false;
            }
                
        }
        else
        {
            Destroy(mMenu); // Destroy miniMenu, when clicked on empty slot, remove if overall check is implemented
        }
    }

    public void UseItem() // setzt voraus, dass Item consumable ist, auf miniMenu Button
    {
        if (item.isStackable)
        {
            Inventory.instance.UseItem(comItem);
        }
    }

    public void BackButton()
    {
        Destroy(mMenu);
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

    public void AddItem(CompleteItem newItem)
    {
        comItem = newItem;
        item = newItem.item;
        numberOfItems = newItem.amount;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        if (itemActive == true) inventoryUI.HideInfoWindow();
        if (mMenu != null) Destroy(mMenu);

        comItem = null;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        numberOfItems = 0;
        stackNumber.enabled = false;
        itemActive = false;
    }

    public void UpdateStack(int newAmount)
    {
        numberOfItems = newAmount;
        if (numberOfItems > 1)
        {
            stackNumber.text = numberOfItems.ToString();
            stackNumber.enabled = true;
        }
        else stackNumber.enabled = false;
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
