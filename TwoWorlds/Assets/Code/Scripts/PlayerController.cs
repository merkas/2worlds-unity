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
    bool optionalDialogue;
    bool automaticListening = false;
    GameObject npc;

    public List<Quest> activeQuests;
    // List with completed and List with failed quests
    List<Companion> companions;

    bool canTakeItem;
    GameObject otherObject;

    public GameObject InteractBox;
    Text interactText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        corruptionStat = 0; // load old stat, when necessary, instead
        if (InteractBox != null)
        {
            interactText = InteractBox.GetComponentInChildren<Text>();
            InteractBox.SetActive(false);
        }
        
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

        if (optionalDialogue == true && Input.GetKey(KeyCode.E))
        {
            if (otherObject.GetComponent<NpcChatter>() != null)
                otherObject.GetComponent<NpcChatter>().ChooseConversation();
            if (otherObject.GetComponent<NpcComment>() != null)
                otherObject.GetComponent<NpcComment>().ChooseComment();

            optionalDialogue = false;
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "DialogueNpc")
        {
            npc = other.gameObject;
            //interactText.text = "Start Conversation with E";
            canStartConversation = true;
            //InteractBox.SetActive(true);
        }
        else if (other.tag == "PickUp")
        {
            //interactText.text = "Pick up object with E";
            otherObject = other.gameObject;
            canTakeItem = true;
            //InteractBox.SetActive(true);
        }
        else if (other.tag == "Chatter")
        {
            //optionalDialogue = true;
            otherObject = other.gameObject;

            if (otherObject.GetComponent<NpcChatter>() != null && otherObject.GetComponent<NpcChatter>().automatic == true)
            {
                otherObject.GetComponent<NpcChatter>().ChooseConversation();
                automaticListening = true;
            }
            else if (otherObject.GetComponent<NpcComment>() != null && otherObject.GetComponent<NpcComment>().automatic == true)  
            {
                otherObject.GetComponent<NpcComment>().ChooseComment();
                automaticListening = true;
            }
            else optionalDialogue = true;
            // show notif, if not automatic and conversation not empty
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //InteractBox.SetActive(false);
        if (other.tag == "DialogueNpc") canStartConversation = false;

        else if (other.tag == "PickUp") canTakeItem = false;

        else if (other.tag == "Chatter") 
        {
            //optionalDialogue = false;
            if (otherObject.GetComponent<NpcChatter>() != null)
                otherObject.GetComponent<NpcChatter>().HideComment();
            if (otherObject.GetComponent<NpcComment>() != null)
                otherObject.GetComponent<NpcComment>().HideComment();
            automaticListening = false;
        }
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

    void Pickup()
    {
        bool pickedUp = Inventory.instance.AddItem(otherObject.GetComponent<ItemPickup>().item);

        if (pickedUp)
            Destroy(otherObject);
    }
}
