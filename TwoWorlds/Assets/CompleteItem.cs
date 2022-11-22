using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteItem
{
    public Item item;
    public int amount; // only called if item is stackable
}
