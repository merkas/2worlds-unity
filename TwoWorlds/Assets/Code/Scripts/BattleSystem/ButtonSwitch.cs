using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    private BattleSystem bs;
    private BattleSystem Un;
    private CharaManager ChM;
    public ButtonNr CurrentButtonNr = ButtonNr.Empty;

    public int CharDmg;

    public bool buttonisClicked;

    // Start is called before the first frame update
    void Start()
    {
        bs = FindObjectOfType<BattleSystem>();
        Un = FindObjectOfType<BattleSystem>();
        ChM = FindObjectOfType<CharaManager>();
        buttonisClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(buttonisClicked == true)
        //{

        //CharDmg += Unit.MCDmg;
        //}
    }

    public void CharaCase()
    {
        ButtonClick(CurrentButtonNr);
    }


    //public void ButtonNeu()
    //{
    //    buttonisClicked = true;
    //        //CharDmg += Unit.MCDmg;
    //    CharDmg = ChM.MC.Damage;
    //        Debug.Log("UnitWert" + ChM.MC.Damage);
        

        //if (gameObject.name == "Chara2")
        //{
        //    CharDmg = Unit.Companion1Dmg;
        //}

    //    Debug.Log("Charawert" + CharDmg);
    //    buttonisClicked = false;
    //}




    public void ButtonClick(ButtonNr buttonnr)
    {


        switch (buttonnr)
        {
            case ButtonNr.MC:
                CurrentButtonNr = ButtonNr.MC;
                //CharDmg = Unit.MCDmg;
                CharDmg = ChM.MC.Damage;
                Debug.Log("Charawert" + CharDmg);
                break;

            case ButtonNr.Companion1:
                CurrentButtonNr = ButtonNr.Companion1;
                CharDmg = ChM.Companion1.Damage;
   
                Debug.Log("Charawert" + CharDmg);
                break;

                //case ButtonNr.Companion2:
                //    CurrentButtonNr = ButtonNr.Companion2;
                //    CharDmg = ChM.Companion2.Damage;

                //    Debug.Log("Charawert" + CharDmg);
                //    break;

                //case ButtonNr.Companion3:
                //    CurrentButtonNr = ButtonNr.Companion3;
                //    CharDmg = ChM.Companion3.Damage;

                //    Debug.Log("Charawert" + CharDmg);
                //    break;
        }

    }
}
