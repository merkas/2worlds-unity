using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    private BattleSystem bs;
    private BattleSystem Un;
    private CharaManager ChM;
    private BattleHud BH;
    public ButtonNr CurrentButtonNr = ButtonNr.Empty;
    public int CharDmg;
    public int UnitHP;
    public int UnitAP;

    public string Chara;

    public bool buttonisClicked;

    public bool MCTurn;
    // Start is called before the first frame update
    void Start()
    {
        bs = FindObjectOfType<BattleSystem>();
        Un = FindObjectOfType<BattleSystem>();
        ChM = FindObjectOfType<CharaManager>();
        BH = FindObjectOfType<BattleHud>();
        buttonisClicked = false;
        MCTurn = false;
    }


    public void CharaCase()
    {
        ButtonClick(CurrentButtonNr);
        BH.SetAP(UnitAP);
        BH.SetHP(UnitHP);
    }

    public void ButtonClick(ButtonNr buttonnr)
    {


        switch (buttonnr)
        {
            case ButtonNr.MC:
                CurrentButtonNr = ButtonNr.MC;
                CharDmg = ChM.MC.Damage;
                UnitAP = ChM.MC.CurrentAP;
                UnitHP = ChM.MC.currentHP;

                Chara = ChM.MC.unitName;

                MCTurn = true;
                Debug.Log("Charawert" + CharDmg);

                bs.EnemyATK(Chara);
                break;

                case ButtonNr.Companion1:
                CurrentButtonNr = ButtonNr.Companion1;
                CharDmg = ChM.Companion1.Damage;
                UnitAP = ChM.Companion1.CurrentAP;
                UnitHP = ChM.Companion1.currentHP;

                Chara = ChM.Companion1.unitName;

                Debug.Log("Charawert" + CharDmg);

                bs.EnemyATK(Chara);
                break;

                    case ButtonNr.Companion2:
                    CurrentButtonNr = ButtonNr.Companion2;
                    CharDmg = ChM.Companion2.Damage;
                    UnitAP = ChM.Companion2.CurrentAP;
                    UnitHP = ChM.Companion2.currentHP;

                    Chara = ChM.Companion2.unitName;

                    Debug.Log("Charawert" + CharDmg);

                    bs.EnemyATK(Chara);
                    break;
        }

    }
}
