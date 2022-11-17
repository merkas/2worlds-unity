using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryWindow;
    public GameObject cardInventoryWindow;
    public Transform cardParent;
    Inventory inventory;

    InventorySlot[] slots;

    CardSlot[] cardSlots;
    CardDeckSlot[] cardDeckSlots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; //?

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        cardSlots = cardParent.GetComponentsInChildren<CardSlot>(); // also normal card slots
        cardDeckSlots = cardParent.GetComponentsInChildren<CardDeckSlot>();
        inventoryWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inventoryWindow.SetActive(true);
            // cardInventoryWindow.SetActive(true);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) // item inventory
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

        for (int i = 0; i < cardSlots.Length; i++) // card inventory
        {
            if (i < inventory.cards.Count) // change
            {
                cardSlots[i].AddCard(inventory.cards[i]); // add cards first to if =< 15 cardDeckSlots
            }
            else
            {
                cardSlots[i].ClearSlot();
            }
        }

        for (int i = 0; i < cardDeckSlots.Length; i++) // card deck (part of card inventory)
        {
            if (i < inventory.deckCards.Count) // change
            {
                cardDeckSlots[i].AddCard(inventory.deckCards[i]); // add cards first to if =< 15 cardDeckSlots
            }
            else
            {
                cardDeckSlots[i].ClearSlot();
            }
        }
    }

    public void CloseInventory()
    {
        inventoryWindow.SetActive(false);
        cardInventoryWindow.SetActive(false);
    }
}
