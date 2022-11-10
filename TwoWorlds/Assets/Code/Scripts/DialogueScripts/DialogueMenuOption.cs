using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NpcDialogueMenuOption")]
public class DialogueMenuOption : ScriptableObject
{
    public string playerQuestion;
    public string npcAnswer;

    public bool oneTime;
    public bool endConversation;

    //Color unused = Color.white;
    //Color used = Color.gray;
}
