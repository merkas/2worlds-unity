using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    DEFENSE,
    ATTACK,
    SONDER,
    SPECIAL,
    SUPPORT
}

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class TemporaryCard: ScriptableObject
{
    public string cardName;
    public Sprite cardIcon;

    public CardType cardType;

    public int ActionPointsNeeded;
    public bool isStackable;

    public string info;
}
