using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIn : MonoBehaviour // delete script, if not needed anymore
{
    public int corruptionStat;
    public DialogueManager dialogueManager;
    
    bool canStartConversation = false;
    GameObject npc;

    public List<Quest> activeQuests;
    // List with completed and List with failed quests

    bool canTakeItem;
    GameObject otherObject;

    void Start()
    {
        corruptionStat = 0;
    }

    void Update()
    {
        if (canStartConversation == true && Input.GetKey(KeyCode.E))
        {
            dialogueManager.NewConversation(npc, this.gameObject);
            dialogueManager.StartConversation();
            canStartConversation = false;
        }
        if (canTakeItem == true && Input.GetKey(KeyCode.E))
            Pickup();
    }

    public void ChangeQuestProgress(Quest quest, int progress)
    {
        quest.questProgress += progress;
    }

    public void ConversationEnded()
    {
        canStartConversation = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DialogueNpc")
        {
            npc = other.gameObject;
            canStartConversation = true;
            //show animated sign, that you can interact
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

    void Pickup()
    {
        bool pickedUp = Inventory.instance.AddItem(otherObject.GetComponent<ItemPickup>().item);

        if (pickedUp)
            Destroy(otherObject);
    }
}
