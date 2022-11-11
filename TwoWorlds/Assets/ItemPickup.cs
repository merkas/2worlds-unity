using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public Quest quest;
    public int questProgress;

    // method interact of interactable base with Pickup() added; or event?

    //void Pickup()
    //{
    //    bool pickedUp = Inventory.instance.AddItem(item);

    //    if(pickedUp)
    //        Destroy(gameObject);
    //}
}
