using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NpcDialogueMenu")]
public class DialogueMenu : ScriptableObject
{
    public string npcMainText;

    public int index = 0;

    public DialogueMenuOption[] menuOption;

    private void OnValidate()
    {
        index = menuOption.Length;
    }
}
