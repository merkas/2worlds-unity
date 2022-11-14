using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public bool isConsumable;
    public bool isQuestItem;
    public bool isStackable;

    public int stack;
}
