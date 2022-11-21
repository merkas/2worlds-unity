using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    DEFENSE,
    ATTACK,
    BUFF,
    SPECIAL
}

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class TemporaryCard: ScriptableObject
{
    public string cardName;
    public Sprite cardIcon;

    public CardType cardType;

    public int ActionPointsNeeded;

    public string info;
}
