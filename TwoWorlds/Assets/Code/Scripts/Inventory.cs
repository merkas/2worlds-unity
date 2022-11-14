using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    List<int> itemAmount = new List<int>();
    List<int> numberOfItems = new List<int>();
    //int[] numberOfItems;
    int allItems;
    int itemNumber;
    bool addedToStack;

    public int space = 10;

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
        foreach (Item obj in items)
        {
            if (item == obj && item.isStackable == true)
            {
                Debug.Log("Have: " + item.stack + ", new: " + obj.stack);
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

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
