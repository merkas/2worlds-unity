using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIn : MonoBehaviour
{
    public int corruptionStat;
    public DialogueManager dialogueManager;
    //GameObject[] Items(string name, bool questItem);
    bool canStartConversation = false;
    GameObject npc;

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
    }

    public void ConversationEnded()
    {
        canStartConversation = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DialogueNpc")
        {
            ;
            npc = other.gameObject;
            canStartConversation = true;
            //show animated sign, that you can interact
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canStartConversation = false;
    }
}
