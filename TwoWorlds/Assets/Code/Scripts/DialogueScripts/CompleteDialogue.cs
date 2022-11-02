using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/CompleteNpcDialogue")]
public class CompleteDialogue : ScriptableObject
{
    public List<SingleDialogue> completeDialogue;

    public bool NpcWithMenu;
    public DialogueMenu dialogueMenu;

    public List<Greeting> greeting;
    //public List<Greeting> additionalGreeting;

    //public GameObject Item;
    //ändern, wenn Item script eingefügt wird
    public Sprite itemSprite;
    public string[] itemText;
    public bool getCondition;
    public int wantedMinCorruption;
    public int wantedMaxCorruption;
    public int wantedNumberOfTalks;

    public bool questGiver;
    public List<Quest> quest;
    public bool questOptionNotEmpty;
    public string[] questOption;
    //public List<SingleDialogue> questDialogue; // nur bei menu npc benötigt?
}
