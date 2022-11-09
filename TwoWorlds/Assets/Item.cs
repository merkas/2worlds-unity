using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string name;
    public Sprite icon;
    //public bool isDefaultItem;

    public bool isConsumable;
    public bool isQuestItem;
    public bool isStackable;
}
