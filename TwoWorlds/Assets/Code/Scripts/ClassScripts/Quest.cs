using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/Quest")]
public class Quest: ScriptableObject
{
    public string title;
    public string description;
    public GameObject reward; // get from item database later
    public bool completedQuest;
    public string questNpcName;

    public DialoguePart acceptQuestChoice;
    public SingleDialogue questDialogue;

    public int minCorruption;
    public int maxCorruption;
    public int minNumberOfTalks;
    public GameObject wantedObject;
    public GameObject triggerObject;

    public int formerQuestProgress;
    public int questProgress;
    public string[] reactionToProgress;
}
