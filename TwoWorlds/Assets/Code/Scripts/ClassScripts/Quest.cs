using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public GameObject reward; // get from item database later

    public string questNpcName;

    public DialoguePart acceptQuestChoice;
    public SingleDialogue questDialogue;

    public int minCorruption;
    public int maxCorruption;
    public int minNumberOfTalks;
    public GameObject wantedObject;
    public GameObject triggerObject;

    public int questProgress;
    public string[] reactionToProgress;
}
