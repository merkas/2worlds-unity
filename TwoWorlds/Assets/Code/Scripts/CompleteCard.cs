using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteCard
{
    public TemporaryCard card;
    public int amount; // only called if card is stackable
}
