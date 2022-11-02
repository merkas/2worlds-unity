using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Dialogue/Answer")]
[System.Serializable]
public class Answer // check if problems appear
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
    public Sprite getItem;
    public string getItemName;
    public Sprite giveItem;
    public string giveItemName;

    //public bool answerCondition;
    //public string neededItemForAnswer;
    //public int neededCorruptionStat;
}
