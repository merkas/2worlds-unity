using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/SingleDialogue")]
public class SingleDialogue : ScriptableObject
{
    public int minCorruption;
    public int maxCorruption;
    public int minNumberOfTalks;
    public bool maxNumber;
    public int maxNumberOfTalks;

    public List<DialoguePart> dialogueParts;
}
