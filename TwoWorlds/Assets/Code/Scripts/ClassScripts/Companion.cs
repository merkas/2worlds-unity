using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : ScriptableObject
{
    // this is a script for companions already following the player

    public string companionName;

    // fight stats and attack funtions

    public List<SingleDialogue> possibleDialogues;

    void CheckForDialogueActivation()
    {
        // dialogue while being with player on certain conditions
    }

    void PlayDialogue()
    {
        // give player always the option to ignore the dialogue
    }

    void LeaveParty()
    {
        // show dialogue and cutscene
    }

    void JoinParty()
    {
        // Add to player companion list and UI
    }
}
