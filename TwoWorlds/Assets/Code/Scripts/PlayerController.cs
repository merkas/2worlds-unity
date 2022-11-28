using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f; 

    private Vector2 moveInput;

    private Rigidbody2D rb;

    public int corruptionStat;
    public DialogueManager dialogueManager;

    bool canStartConversation = false;
    GameObject npc;

    public List<Quest> activeQuests;
    // List with completed and List with failed quests

    bool canTakeItem;
    GameObject otherObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        corruptionStat = 0; // load old stat, when necessary, instead
    }



    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        if (canStartConversation == true && Input.GetKey(KeyCode.E))
        {
            dialogueManager.NewConversation(npc, this.gameObject);
            dialogueManager.StartConversation();
            canStartConversation = false;
        }
        if (canTakeItem == true && Input.GetKey(KeyCode.E))
            Pickup();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "DialogueNpc")
        {
            npc = other.gameObject;
            canStartConversation = true;
        }
        if (other.tag == "PickUp")
        {
            otherObject = other.gameObject;
            canTakeItem = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "DialogueNpc") canStartConversation = false;

        if (other.tag == "PickUp") canTakeItem = false;

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void ChangeQuestProgress(Quest quest, int progress)
    {
        quest.questProgress += progress;
    }

    public void ConversationEnded()
    {
        canStartConversation = true;
    }

    void AddTemporaryCompanion()
    {
        // get temporary cards
        // add temporary cards to inventory
        // add to companion tab
    }

    void Pickup()
    {
        bool pickedUp;
        if (otherObject.GetComponent<ItemPickup>().item.item != null)
            pickedUp = Inventory.instance.AddItem(otherObject.GetComponent<ItemPickup>().item);
        else pickedUp = Inventory.instance.AddCard(otherObject.GetComponent<ItemPickup>().card);

        if (pickedUp)
            Destroy(otherObject);
    }
}
