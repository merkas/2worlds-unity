using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePartner : MonoBehaviour
{
    public string npcName;
    public GameObject nameShield;
    DialogueNpc thisNPC = new DialogueNpc(); //vorerst, wenn fertig, dann bestimmte NPCs aufgerufen

    public CompleteDialogue thisNpcDialogue;
    SingleDialogue activeDialogue;

    int activeIndex = 0;
    public DialoguePart activeDialoguePart;
    public bool noDialogueLeft;

    private void Start()
    {
        //thisNPC.name = npcName;
    }
    
    public void ChooseGreeting(int corruptionStat)//still empty
    {
        //thisNPC.ChooseGreeting();
        //thisNPC.AdditionalGreeting(); // after greeting
    }

    public void GetData(int corruptionStat) //chooses dialogue based on corrupption and numberOfTalks
    {

        foreach (SingleDialogue dialogue in thisNpcDialogue.completeDialogue)
        {
            Debug.Log("Dialog: " + dialogue); // second dialogue doesn´t get called!

            if (corruptionStat < dialogue.minCorruption || corruptionStat > dialogue.maxCorruption)
            {
                return;
            }
            else if (corruptionStat >= dialogue.minCorruption && corruptionStat <= dialogue.maxCorruption)
            {
                if (thisNPC.numberOfTalks < dialogue.minNumberOfTalks ||
                    dialogue.maxNumber == true && thisNPC.numberOfTalks > dialogue.maxNumberOfTalks)
                {
                    return;
                }

                else if (thisNPC.numberOfTalks >= dialogue.minNumberOfTalks &&
                    thisNPC.numberOfTalks <= dialogue.maxNumberOfTalks && dialogue.maxNumber == true ||
                    thisNPC.numberOfTalks >= dialogue.minNumberOfTalks && dialogue.maxNumber == false)
                {
                    LoadDialogue(dialogue, 0); // starts on beginning of dialogue, so 0
                    break;
                }

                else
                {
                    return;
                }
            }
        }
    }

    public void LoadDialogue(SingleDialogue dialogue, int index) //load chosen dialogue
    {
        activeDialogue = dialogue;
        activeIndex = index;
        activeDialoguePart = activeDialogue.dialogueParts[index]; //load first dialogue part
    }

    public bool CheckIfConversationEnded(int chosenIndex)
    {
        noDialogueLeft = activeDialoguePart.playerResponse[chosenIndex].endConversation;
        return noDialogueLeft;
    }

    public void CheckForItemConditions(int corruptionStat)
    {
        if (corruptionStat >= thisNpcDialogue.wantedMinCorruption && corruptionStat <= thisNpcDialogue.wantedMaxCorruption)
        {
            if (thisNPC.numberOfTalks == thisNpcDialogue.wantedNumberOfTalks)
            {
                //get item
                thisNpcDialogue.GetItem = false;
            }
        }
    }

    public void CheckReactionToPlayerResponse(int index) //questtrigger / progress
    {
        if (activeDialoguePart.playerResponse[index].getAnItem == true)
        {

        }
        if (activeDialoguePart.playerResponse[index].questTrigger == true)
        {

        }
        if (activeDialoguePart.playerResponse[index].questComplete == true)
        {

        }
    }

    public void LoadNextDialoguePart(int indexOfChosenResponse)
    {
        //LoadDialogue(activeDialogue, activeIndex + 1); //for a linear dialogue without choices

        activeDialogue.dialogueParts[activeIndex].LoadNextPart(indexOfChosenResponse); //choose next DialoguePart
        activeDialoguePart = activeDialogue.dialogueParts[activeIndex].NextText(); //Get next dialoguePart
    }

    public void LoadQuestDialogue()
    {
        //check for active quest
        //check for questprogress
        //check for item to activate / move progress of quest
    }

    public void SaveNewInfo()
    {
        thisNPC.numberOfTalks++;

        //QuestInfo + used onetime and reusable DialogueParts
        //used dialogue
    }
}
