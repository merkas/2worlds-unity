using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour // = Battle UI
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Slider APSlider;

    public void SetHub(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        APSlider.maxValue = unit.maxAP;
        APSlider.value = unit.CurrentAP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetAP(int ap)
    {
        APSlider.value = ap;
    }
}
