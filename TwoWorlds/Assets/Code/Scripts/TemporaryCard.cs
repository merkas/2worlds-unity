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

    public int _ATKValue;
    [SerializeField] int _APDrain;
    [SerializeField] int _HealHP;
   
    
    

    public string cardName;
    public Sprite cardIcon;

    public CardType cardType;

    public int ActionPointsNeeded;
    public bool isStackable;


    public string info;


    public int ATKValue
    {
        get { return _ATKValue; }
        set { _ATKValue = ATKValue;  }
    }

    public int APDrain
    {
        get { return _APDrain; }
        set { _APDrain = APDrain; }
    }

    public int HealHP
    {
        get { return _HealHP; }
        set { _HealHP = HealHP; }
    }

}
