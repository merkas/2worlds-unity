using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<TemporaryCard> cards = new List<TemporaryCard>();
    public List<TemporaryCard> deckCards = new List<TemporaryCard>();

    List<int> itemAmount = new List<int>();
    List<int> numberOfItems = new List<int>();
    //int[] numberOfItems;
    //int allItems;
    //int itemNumber;
    bool addedToStack;

    public int space = 10;
    public int cardSpace = 20;

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
                numberOfItems[itemNumber] += obj.stack;
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

            numberOfItems.Add(item.stack);
            items.Add(item);
        }
        

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public bool AddCard(TemporaryCard card)
    {
        if (cards.Count >= cardSpace)
        {
            Debug.Log("Not enough room");
            return false;
        }
        cards.Add(card);

        // event

        return true;
    }

    public void RemoveCard(TemporaryCard card)
    {
        cards.Remove(card);

        // event
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
