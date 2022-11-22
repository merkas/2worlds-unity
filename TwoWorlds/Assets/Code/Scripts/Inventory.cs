using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<TemporaryCard> cards = new List<TemporaryCard>();
    public List<TemporaryCard> deckCards = new List<TemporaryCard>();

    //List<int> itemAmount = new List<int>();
    List<int> numberOfItems = new List<int>();
    List<int> numberOfCards = new List<int>();
    //int[] numberOfItems;
    //int allItems;
    //int itemNumber;
    bool addedToStack;

    public int space = 10;
    public int cardSpace = 20;
    public int deckSpace = 20;

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            //return;
        }
        else instance = this;
    }
    #endregion

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallback;

    public bool AddItem(Item item)
    {
        addedToStack = false;
        
        int itemNumber = 0;
        foreach (Item obj in items) // check if same stackable item is already in inventory
        {
            if (item == obj && item.isStackable == true)
            {
                //numberOfItems[itemNumber] += obj.stack;
                addedToStack = true;
                break;
            }
            itemNumber += 1;
        }
        
        if (addedToStack != true)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }

            //numberOfItems.Add(item.stack);
            items.Add(item);
        }
        

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public bool AddCard(TemporaryCard card)
    {
        addedToStack = false;

        int cardNumber = 0;
        foreach (TemporaryCard obj in cards) // check if same stackable card is already in card inventory
        {
            if (card == obj && card.isStackable == true)
            {
                numberOfCards[cardNumber] ++; // nur einzelne Karten zu finden, nicht in stacks
                addedToStack = true;
                break;
            }
            cardNumber += 1;
        }

        if (addedToStack != true)
        {
            if (cards.Count < deckSpace) // check if deck is full, if not add card
            {
                deckCards.Add(card);

                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();

                return true;
            }
            if (cards.Count >= cardSpace) // check if card inventory is full, if true can't pick up
            {
                Debug.Log("Not enough room");
                return false;
            }
            cards.Add(card); // if nothing of above add card in card inventory
            //numberOfCards.Add()
        }
        

        // event
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public void MoveCardToDeck(TemporaryCard card, bool doubleClick, TemporaryCard deckCard = null)
    {
        if (deckCards.Count < deckSpace && doubleClick == false)
        {
            cards.Remove(card);
            deckCards.Add(card);
        }
        else if (deckCards.Count == deckSpace && doubleClick == true && deckCard != null) 
            // deck full, exchange cards with a double click on both
        {
            cards.Remove(card);
            deckCards.Remove(deckCard);

            cards.Add(deckCard);
            deckCards.Add(card);
        }
        
        //if (onItemChangedCallback != null)
        //    onItemChangedCallback.Invoke();
    }

    public void MoveCardToInventory(TemporaryCard deckCard, bool doubleClick, TemporaryCard card = null)
    {
        if (cards.Count < space && doubleClick == false)
        {
            deckCards.Remove(deckCard);
            cards.Add(deckCard);
        }
        else if (cards.Count == space && doubleClick == true && card != null)
        // deck full, exchange cards with a double click on both
        {
            deckCards.Remove(deckCard);
            cards.Remove(card);

            deckCards.Add(card);
            cards.Add(deckCard); // check for stack is in method
        }
        
        //if (onItemChangedCallback != null)
        //    onItemChangedCallback.Invoke();
    }

    public void RemoveCard(TemporaryCard card)
    {
        cards.Remove(card);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        // event
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
