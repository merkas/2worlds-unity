using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialoguePart")]
public class DialoguePart : ScriptableObject
{
    public bool reusable;
    public bool playerChoice;
    public int responseNumbers = 0;

    public string[] npcText; // whole array called at beginning of dialogue part

    public Answer[] playerResponse;
    // playerResponse und damit verbundener nextNpcText haben den gleichen Index

    public DialoguePart[] nextNpcText;
    DialoguePart chosenNpcText;

    private void OnValidate()
    {
        responseNumbers = playerResponse.Length;

        if (responseNumbers > 1) playerChoice = true;
        else playerChoice = false;
    }

    public void LoadNextPart(int index)
    {
        chosenNpcText = nextNpcText[index];
    }

    public DialoguePart NextText()
    {
        return chosenNpcText;
    }
}
