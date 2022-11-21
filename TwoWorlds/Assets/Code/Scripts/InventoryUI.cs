using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryWindow;
    public GameObject cardInventoryWindow;
    public Transform cardParent;

    public Button AddToInventoryButton;
    public Button AddToDeckButton;

    Inventory inventory;
    TemporaryCard activeCard;
    public GameObject cardWindow;
    public Text cardTitleText;
    public Text cardInfoText;

    InventorySlot[] slots;

    CardSlot[] cardSlots;
    CardDeckSlot[] cardDeckSlots;

    GameObject selectedObject;

    public delegate void MovedObject();
    public static event MovedObject movedObject;

    public delegate void ClosingInventory();
    public static event ClosingInventory inventoryClosed;

    GameObject obj1;
    GameObject obj2;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        cardSlots = cardParent.GetComponentsInChildren<CardSlot>(); // also normal card slots
        cardDeckSlots = cardParent.GetComponentsInChildren<CardDeckSlot>();
        inventoryWindow.SetActive(false);
        cardInventoryWindow.SetActive(false);
        cardWindow.SetActive(false);

        //movedInventoryObject += ChangeSlots;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inventoryWindow.SetActive(true);
            cardInventoryWindow.SetActive(true);
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

    public void ClickedInInventory(GameObject obj) // = clicked on slot
    {
        if (obj1 == null)
            obj1 = obj;
        else if (obj1 != null && obj2 == null)
        {
            if (obj1 != obj)
            {
                obj2 = obj;

                //if (moveInventoryObject != null)
                //    moveInventoryObject.Invoke();
            }
        }
        
    }

    public void ShowActiveCard(TemporaryCard card, GameObject slot, bool doubleClick = false)
    {
        if (selectedObject != null) // object activated before
        {
            if (selectedObject.GetComponent<CardSlot>() != null)
            {
                selectedObject.GetComponent<CardSlot>().CardActive(false);
            }
            else selectedObject.GetComponent<CardDeckSlot>().CardActive(false);
        }
        selectedObject = slot;
        activeCard = card;

        cardWindow.GetComponentInChildren<Image>().sprite = card.cardIcon;
        cardTitleText.GetComponent<Text>().text = card.cardName;
        cardInfoText.GetComponent<Text>().text = card.info;

        if (selectedObject.GetComponent<CardSlot>() == true)
        {
            AddToDeckButton.gameObject.SetActive(true);
            AddToInventoryButton.gameObject.SetActive(false);
        }
        else
        {
            AddToInventoryButton.gameObject.SetActive(true);
            AddToDeckButton.gameObject.SetActive(false);
        }
        cardWindow.SetActive(true);
    }

    public void MoveCardToDeck() // move card to deck, needed in UI because of click and button methods
    {
        //bool doubleClick; statt true
        
        selectedObject.GetComponent<CardSlot>().ClearSlot();
        
        cardDeckSlots[inventory.deckCards.Count].GetComponent<CardDeckSlot>().AddCard(activeCard);
        Inventory.instance.MoveCardToDeck(activeCard, false);

        AddToDeckButton.gameObject.SetActive(false);
        AddToInventoryButton.gameObject.SetActive(true);

        selectedObject = cardDeckSlots[inventory.deckCards.Count].gameObject;
        selectedObject.GetComponent<CardDeckSlot>().CardActive(true);
        //UpdateUI();
    }

    public void MoveCardToInventory() //remove card from deck
    {
        //bool doubleClick; statt true

        selectedObject.GetComponent<CardDeckSlot>().ClearSlot();
        cardSlots[inventory.cards.Count].GetComponent<CardSlot>().AddCard(activeCard);
        Inventory.instance.MoveCardToInventory(activeCard, false);

        AddToDeckButton.gameObject.SetActive(true);
        AddToInventoryButton.gameObject.SetActive(false);

        selectedObject = cardSlots[inventory.cards.Count].gameObject;
        selectedObject.GetComponent<CardSlot>().CardActive(true);
        //UpdateUI();
    }

    //public void EmptySlot(GameObject obj)
    //{
    //    if (obj == obj1) obj1 = null;
    //}

    void ChangeSlots()
    {
        //obj1.SendMessage("GetSecondObj", obj2);
        
        //obj1 = null;
        //obj2 = null;
        //UpdateUI();
    }

}
