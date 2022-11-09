using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    bool canTakeItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canTakeItem = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canTakeItem = false;
        }
    }

    private void Update()
    {
        if (canTakeItem == true && Input.GetKey(KeyCode.E))
            Pickup();
    }

    // method interact of interactable base with Pickup() added; or event?

    void Pickup()
    {
        bool pickedUp = Inventory.instance.AddItem(item);

        if(pickedUp)
            Destroy(gameObject);
    }
}
