using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour // enemy/player object
{
    public string unitName;
    public int unitLevel;
    [SerializeField] int _Damage;

    public int maxHP;
    public int currentHP;
    public int maxAP;
    public int CurrentAP;

    public int Damage
    {
        get { return _Damage; }
        set { _Damage = Damage; }
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        Debug.Log("Schaden" + dmg);

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }


    public void DrainAP(int amount)
    {
        Debug.Log("Amount" + amount);
        CurrentAP -= amount;
    }

    public void GetAP(int amount)
    {
        CurrentAP += amount;

        if (CurrentAP > maxAP)
            CurrentAP = maxAP;
    }


}

