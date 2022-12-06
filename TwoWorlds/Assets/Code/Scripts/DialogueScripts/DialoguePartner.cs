using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialoguePartner : MonoBehaviour
{
    public string npcName;
    //public GameObject nameShield;
    DialogueNpc thisNPC = new DialogueNpc(); //vorerst, wenn fertig, dann bestimmte NPCs aufgerufen
    public CompleteDialogue thisNpcDialogue;
    SingleDialogue activeDialogue;
    public bool getItem;

    int activeIndex = 0;

    public bool changeToMenuNpc = false;
    public bool NpcWithMenu;

    [System.NonSerialized]
    public DialoguePart activeDialoguePart;

    public bool noDialogueLeft;
    public bool corruptionOutOfRange = false;

    public List<Quest> activeQuests; // only of this npc

    public List<Quest> allQuests; // that can be received
    List<Quest> completedQuests;
    List<Quest> abandonedQuests;

    public string chosenGreeting;
    public string additionalGreeting;

    [System.NonSerialized]
    public CompleteItem item;
    [System.NonSerialized]
    public string[] itemText;

    public NpcData thisNpcData;

    private void Start()
    {
        allQuests = thisNpcDialogue.quest;
        //reset quests for new game needed?

        LoadNpcData();

        SceneDataSave.instance.LoadNpcData(thisNpcData, this.gameObject);
    }

    public void UpdateData(NpcData data)
    {
        Debug.Log("Data updated");
        thisNpcData = data;
        
        //check if right number of talks
    }

    void LoadNpcData()
    {
        thisNpcData.npcName = npcName;
        thisNpcData.numberOfTalks = thisNPC.numberOfTalks;
        thisNpcData.npcWithMenu = NpcWithMenu;
        thisNpcData.getItem = getItem;
        // ...
    }


    private void OnDestroy()
    {
        //LoadNpcData();
        SceneDataSave.instance.SaveNewNpcData(thisNpcData); // save new data
    }

    public void ChooseGreeting(int corruptionStat)
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
                        foreach (CompleteItem playerItem in Inventory.instance.items)
                        {
                            if (playerItem.item == greeting.triggerItem) // amount checken
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
                // else 
                if (thisNPC.numberOfTalks >= dialogue.minNumberOfTalks &&
                    thisNPC.numberOfTalks <= dialogue.maxNumberOfTalks && dialogue.maxNumber == true ||
                    thisNPC.numberOfTalks >= dialogue.minNumberOfTalks && dialogue.maxNumber != true)
                {
                    LoadDialogue(dialogue, 0); // starts on beginning of dialogue, so 0
                    break;
                }

                // else this person won´t talk to player anymore

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
                    abandonedQuests.Add(changedQuest);
                }
                else if (questStatus == "triggered")
                {
                    activeQuests.Add(changedQuest);
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
                item = thisNpcDialogue.item;
                itemText = thisNpcDialogue.itemText;
                getItem = false;
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
        Debug.Log(thisNPC.numberOfTalks);
        //LoadNpcData();
        //SceneDataSave.instance.SaveNewNpcData(thisNpcData);
    }
}
