using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/Quest")]
public class Quest: ScriptableObject
{
    public string title;
    public string description;
    public CompleteItem reward;
    public CompleteCard cardReward;

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
    public int maxProgress; // if questProgress = maxProgress -> Quest completed
    public string[] reactionToProgress;
}
