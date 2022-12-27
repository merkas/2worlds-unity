using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{


    public bool hasBeenPlayed;

    public int handindex;

    private BattleSystem bs;
    private CardManager CM;
    public CardNr CurrentCardNr = CardNr.Empty;
    public int CardDmg;
    public int HP;
    public int AP;


    //[SerializeField]
    //public TemporaryCard Card1;
    //[SerializeField]
    //public TemporaryCard Card2;
    //[SerializeField]
    //public TemporaryCard Card3;



    private void Update()
    {
        if (bs.Cardsplayable == true)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void Start()
    {
        bs = FindObjectOfType<BattleSystem>();
        CM = FindObjectOfType<CardManager>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnMouseDown()
    {
        //OnMouseDown mit 2D Collider auf den GameObject Karten oder in Button als Funktion
        if (hasBeenPlayed == false)
        {

            if(gameObject.name == "Card6")
            {
                CardClick(CurrentCardNr);
                bs.OnHealButton(HP, AP);

                bs.Cardsplayable = false;
                transform.position += Vector3.up * 1;
                hasBeenPlayed = true;
                bs.availableCardSlots[handindex] = true;
                Invoke(nameof(MoveToDiscard), 1);
            }


            else
            {
                CardClick(CurrentCardNr);
                Debug.Log("ATK Wert" + CardDmg);
                bs.OnAttackButton(CardDmg, AP);

                bs.Cardsplayable = false;
                transform.position += Vector3.up * 1;
                hasBeenPlayed = true;
                bs.availableCardSlots[handindex] = true;
                Invoke(nameof(MoveToDiscard), 1);
            }
        }
    }




    void MoveToDiscard()
    {
        bs.discarded.Add(this);
        gameObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        transform.position += Vector3.up * 0.2f;
        transform.position += Vector3.right * 0.1f;
    }

    void OnMouseExit()
    {
        transform.position += Vector3.up * -0.2f;
        transform.position += Vector3.right * -0.1f;
    }


    public void CardClickStart()
    {
        CardClick(CurrentCardNr);
    }
    public void CardClick(CardNr cardnr)
    {


        switch (cardnr)
        {
                case CardNr.Card1:
                CurrentCardNr = CardNr.Card1;
                CardDmg = CM.Card1.ATKValue;

                AP = CM.Card1.APDrain;
                break;

                case CardNr.Card2:
                CurrentCardNr = CardNr.Card2;
                CardDmg = CM.Card2.ATKValue;

                AP = CM.Card2.APDrain;
                break;

                case CardNr.Card3:
                CurrentCardNr = CardNr.Card3;
                CardDmg = CM.Card3.ATKValue;

                AP = CM.Card3.APDrain;
                break;

                case CardNr.Card4:
                CurrentCardNr = CardNr.Card4;
                CardDmg = CM.Card4.ATKValue;

                AP = CM.Card4.APDrain;
                break;

                case CardNr.Card5:
                CurrentCardNr = CardNr.Card5;
                CardDmg = CM.Card5.ATKValue;

                AP = CM.Card5.APDrain;
                break;

                case CardNr.Card7:
                CurrentCardNr = CardNr.Card7;
                CardDmg = CM.Card7.ATKValue;

                AP = CM.Card7.APDrain;
                break;

                case CardNr.Card6:
                HP = CM.Card6.HealHP;

                AP = CM.Card6.APDrain;
                break;

                case CardNr.Card8:
                CurrentCardNr = CardNr.Card8;
                CardDmg = CM.Card8.ATKValue;

                AP = CM.Card8.APDrain;
                break;

                case CardNr.Card9:
                CurrentCardNr = CardNr.Card9;
                CardDmg = CM.Card9.ATKValue;

                AP = CM.Card9.APDrain;
                break;

                case CardNr.Card10:
                CurrentCardNr = CardNr.Card10;
                CardDmg = CM.Card10.ATKValue;

                AP = CM.Card10.APDrain;
                break;
        }


    }

}