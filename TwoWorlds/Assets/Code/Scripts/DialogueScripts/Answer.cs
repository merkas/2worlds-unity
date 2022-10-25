using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Answer")]
//[System.Serializable]
public class Answer : ScriptableObject
{
    public string answer;

    public bool endConversation;
    public int corruptionStatChange;

    public bool questTrigger;
    //public string quest;
    public int questProgress;
    public bool questComplete;

    //public bool itemExchanged;
    //public bool itemCondition;
    public bool getAnItem;
    public bool giveAnItem;
    //public GameObject getItem;
    //public string getItemName;
    //public GameObject giveItem;
    //public string giveItemName;

    //public bool answerCondition;
    //public string neededItemForAnswer;
    //public int neededCorruptionStat;
}
