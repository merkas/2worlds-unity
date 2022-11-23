using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer
{
    public string answer;

    public bool endConversation;
    public int corruptionStatChange;

    public bool questTrigger;
    public bool abandonedQuest;
    public bool questComplete;
    public Quest changedQuest;
    public string questTitle;
    public int questProgress;
    public bool startAFight;

    //public bool itemExchanged;
    //public bool itemCondition;
    public bool getAnItem;
    public bool giveAnItem;
    public CompleteItem getItem;
    public CompleteItem giveItem;

    public bool changeToMenuInNextConversation;
    
}
