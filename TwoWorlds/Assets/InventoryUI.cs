using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryWindow;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; //?

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventoryWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inventoryWindow.SetActive(true);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
                if (inventory.items[i].isStackable)
                {
                    slots[i].AddToStack(inventory.items[i].stack);
                }
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void CloseInventory()
    {
        inventoryWindow.SetActive(false);
    }
}
