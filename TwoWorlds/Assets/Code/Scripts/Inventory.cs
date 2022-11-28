using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<CompleteItem> items = new List<CompleteItem>();
    public List<CompleteCard> cards = new List<CompleteCard>();
    public List<CompleteCard> deckCards = new List<CompleteCard>();
    public List<CompleteCard> temporaryDeckCards = new List<CompleteCard>();

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

    public bool AddItem(CompleteItem item)
    {
        addedToStack = false;

        if (item.item.isStackable == true)
        {
            int itemNumber = 0;
            foreach (CompleteItem obj in items) // check if same stackable item is already in inventory
            {
                if (item.item == obj.item)
                {
                    obj.amount += item.amount;
                    addedToStack = true;
                    break;
                }
                itemNumber += 1;
            }
        }
        
        if (addedToStack != true)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(item);
        }
        
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public bool AddCard(CompleteCard card)
    {
        if (cards.Count < deckSpace) // check if deck is full, if not add card
        {
            deckCards.Add(card);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();

            return true;
        }

        addedToStack = false;
        if (card.card.isStackable == true)
        {
            int cardNumber = 0;
            foreach (CompleteCard obj in cards) // check if same stackable card is already in card inventory
            {
                if (card.card == obj.card)
                {
                    //obj.amount += card.amount;
                    obj.amount += 1; // ändern zu oben, wenn Karten doppelt oder mehrfach auf einmal zu finden sind
                    addedToStack = true;
                    return true;
                }
                cardNumber += 1;
            }
        }
        else if (cards.Count >= cardSpace) // check if card inventory is full, if true can't pick up
        {
            Debug.Log("Not enough room");
            return false;
        }
        cards.Add(card); // if nothing of above add card in card inventory

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public void MoveCardToDeck(CompleteCard card, bool doubleClick, CompleteCard deckCard = null)
    {
        if (deckCards.Count < deckSpace && doubleClick == false)
        {
            if (card.card.isStackable == true)
            {
                if (card.amount > 1)
                {
                    card.amount -= 1;
                }
                else cards.Remove(card);
            }
            else cards.Remove(card);

            deckCards.Add(card);
        }
        else if (doubleClick == true && deckCard != null) // exchange cards with a double click on both
        {
            cards.Remove(card);
            deckCards.Remove(deckCard);

            cards.Add(deckCard);
            deckCards.Add(card);
        }
    }

    public void MoveCardToInventory(CompleteCard deckCard, bool doubleClick, CompleteCard card = null)
    {
        if (cards.Count < space && doubleClick == false)
        {
            deckCards.Remove(deckCard);
            if (deckCard.card.isStackable == true)
            {
                bool isInInventory = false;
                foreach (CompleteCard c in cards)
                {
                    if (c.card == deckCard.card)
                    {
                        c.amount += 1;
                        isInInventory = true;
                        break;
                    }
                }
                if (isInInventory == false) cards.Add(deckCard);
            }
            else cards.Add(deckCard);
        }
        else if (doubleClick == true && card != null) // exchange cards with a double click on both
        {
            deckCards.Remove(deckCard);
            cards.Remove(card);

            deckCards.Add(card);
            cards.Add(deckCard); // check for stack is in method
        }
    }

    public void UseItem(CompleteItem item)
    {
        item.amount -= 1;
        if (item.amount == 0) RemoveItem(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveCard(CompleteCard card)
    {
        cards.Remove(card);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(CompleteItem item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
