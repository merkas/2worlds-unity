using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/CompleteNpcDialogue")]
public class CompleteDialogue : ScriptableObject
{
    public List<SingleDialogue> completeDialogue;

    public bool NpcWithMenu;
    public DialogueMenu dialogueMenu;

    public List<string> Greeting;
    public List<string> AdditionalGreeting;

    public bool GetItem;
    public GameObject Item;
    public bool getCondition;
    public int wantedMinCorruption;
    public int wantedMaxCorruption;
    public int wantedNumberOfTalks;

    public bool questGiver;
    public List<SingleDialogue> questDialogue;
}
