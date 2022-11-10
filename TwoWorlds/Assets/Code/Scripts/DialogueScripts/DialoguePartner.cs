using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePartner : MonoBehaviour
{
    public string npcName;
    public GameObject nameShield;
    DialogueNpc thisNPC = new DialogueNpc(); //vorerst, wenn fertig, dann bestimmte NPCs aufgerufen
    public bool getItem;
    public CompleteDialogue thisNpcDialogue;
    SingleDialogue activeDialogue;

    int activeIndex = 0;

    public bool changeToMenuNpc = false;
    public bool NpcWithMenu;

    [System.NonSerialized]
    public DialoguePart activeDialoguePart;

    public bool noDialogueLeft;
    public bool corruptionOutOfRange = false;
    
    [System.NonSerialized]
    public List<Quest> activeQuests; // only of this npc

    List<Quest> allQuests;
    List<Quest> completedQuests;
    List<Quest> abandonedQuests;
    List<Quest> receivableQuests;

    public string chosenGreeting;
    public string additionalGreeting;

    [System.NonSerialized]
    public Item item;
    [System.NonSerialized]
    public string[] itemText;

    private void Start()
    {
        foreach (Quest quest in thisNpcDialogue.quest)
        {
            //allQuests.Add(quest);
            //allQuests = thisNpcDialogue.quest;
            //reset quests?
            //quest.formerQuestProgress = 0;
            //quest.questProgress = 0;
        }
    }

    public void ChooseGreeting(int corruptionStat)//still empty
    {
        foreach (Greeting greeting in thisNpcDialogue.greeting)
        {
            if (corruptionStat >= greeting.minCorruption && greeting.maxCor == true && corruptionStat <= greeting.maxCorruption ||
                corruptionStat >= greeting.minCorruption && greeting.maxCor == false)
            {
                if (thisNPC.numberOfTalks >= greeting.minNumberOfTalks &&
                    thisNPC.numberOfTalks <= greeting.maxNumberOfTalks && greeting.maxTalks == true ||
                    thisNPC.numberOfTalks >= greeting.minNumberOfTalks && greeting.maxTalks != true)
                {
                    if (greeting.triggerItem != null)
                    {
                        foreach (Item playerItem in Inventory.instance.items)
                        {
                            if (playerItem == greeting.triggerItem)
                            {
                                chosenGreeting = greeting.greetingText;
                                break;
                            }
                        }
                    }
                    else
                    {
                        chosenGreeting = greeting.greetingText;
                        break;
                    }
                }
            }
        }

        //foreach (Greeting addition in thisNpcDialogue.additionalGreeting)
        //{
        //    
        //}
    }

    public void GetData(int corruptionStat) //choose dialogue based on corrupption and numberOfTalks
    {
        foreach (SingleDialogue dialogue in thisNpcDialogue.completeDialogue)
        {

            if (corruptionStat < dialogue.minCorruption || corruptionStat > dialogue.maxCorruption)
            {
                corruptionOutOfRange = true; // if stat goes down again, it still won't work
            }
            else if (corruptionStat >= dialogue.minCorruption && corruptionStat <= dialogue.maxCorruption)
            {
                //if (thisNPC.numberOfTalks < dialogue.minNumberOfTalks ||
                //    dialogue.maxNumber == true && thisNPC.numberOfTalks > dialogue.maxNumberOfTalks)
                //{
                //    //if (dialogue == thisNpcDialogue.completeDialogue[thisNpcDialogue.completeDialogue.Count])
                //    //{
                //        //no dialogue -> no conversation
                //        //noDialogueLeft = true;
                //        //check it in DialogueManager
                //        //break;
                //    //}
                //}

                /*else*/ if (thisNPC.numberOfTalks >= dialogue.minNumberOfTalks &&
                    thisNPC.numberOfTalks <= dialogue.maxNumberOfTalks && dialogue.maxNumber == true ||
                    thisNPC.numberOfTalks >= dialogue.minNumberOfTalks && dialogue.maxNumber != true)
                {
                    LoadDialogue(dialogue, 0); // starts on beginning of dialogue, so 0
                    break;
                }

                else
                {
                    Debug.Log("No dialogue"); //not called
                    // This person won´t talk to you anymore
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
        if (activeDialoguePart.playerResponse[chosenIndex].endConversation == true) noDialogueLeft = true;
        else noDialogueLeft = false;
        return noDialogueLeft;
    }

    public void LoadNextDialoguePart(int indexOfChosenResponse)
    {
        activeDialoguePart = activeDialogue.dialogueParts[activeIndex].nextNpcText[indexOfChosenResponse];
        int index = 0;
        foreach (DialoguePart dialoguePart in activeDialogue.dialogueParts)
        {
            if (dialoguePart == activeDialoguePart)
            {
                activeIndex = index;
                break;
            }
            else index++;
        }
    }

    public void ChangedQuestStatus(Quest changedQuest, string questStatus)
    {
        foreach (Quest quest in thisNpcDialogue.quest)
        {
            if (quest == changedQuest)
            {
                changedQuest = quest;
                if (questStatus == "abandoned")
                {
                    //receivableQuests.Remove(changedQuest);
                    abandonedQuests.Add(changedQuest);
                }
                else if (questStatus == "triggered")
                {
                    //activeQuests.Add(changedQuest); // not working
                }
                else if (questStatus == "completed")
                {
                    activeQuests.Remove(changedQuest);
                    completedQuests.Add(changedQuest);
                }
                break;
            }
        }
    }

    public bool CheckForItemConditions(int corruptionStat) //item given at beginning of conversation
    {
        if (corruptionStat >= thisNpcDialogue.wantedMinCorruption && corruptionStat <= thisNpcDialogue.wantedMaxCorruption)
        {
            if (thisNPC.numberOfTalks == thisNpcDialogue.wantedNumberOfTalks)
            {
                //get item
                item = thisNpcDialogue.item;
                itemText = thisNpcDialogue.itemText;
                getItem = false; //check
                return true;
            }
            else return false;
        }
        else return false;
    }


    public void LoadQuestDialogue() // menu npc
    {
        //check for active quest
        //check for questprogress
        //check for item to activate / move progress of quest
    }

    public void SaveNewInfo()
    {
        thisNPC.numberOfTalks++;
        if (changeToMenuNpc == true)
        {
            NpcWithMenu = true;
        }
        //QuestInfo + used onetime and reusable DialogueParts
        //used dialogue
    }
}
