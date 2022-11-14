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

    public GameObject InteractBox;
    Text interactText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        corruptionStat = 0; // load old stat, when necessary, instead
        interactText = InteractBox.GetComponentInChildren<Text>();
        InteractBox.SetActive(false);
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
            interactText.text = "Start Conversation with E";
            canStartConversation = true;
            InteractBox.SetActive(true);
        }
        if (other.tag == "PickUp")
        {
            interactText.text = "Pick up object with E";
            otherObject = other.gameObject;
            canTakeItem = true;
            InteractBox.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractBox.SetActive(false);
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

    void Pickup()
    {
        bool pickedUp = Inventory.instance.AddItem(otherObject.GetComponent<ItemPickup>().item);

        if (pickedUp)
            Destroy(otherObject);
    }
}
