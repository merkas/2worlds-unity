using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{


    public bool hasBeenPlayed;

    public int handindex;

    private BattleSystem bs;

    private void Update()
    {
        if(bs.Cardsplayable == true)
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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnMouseDown()
    {
        //OnMouseDown mit 2D Collider auf den GameObject Karten oder in Button als Funktion
        if(hasBeenPlayed == false)
        {
            bs.Cardsplayable = false;
            transform.position += Vector3.up * 1;
            hasBeenPlayed = true;
            bs.availableCardSlots[handindex] = true;
            Invoke(nameof(MoveToDiscard), 1);

            if (gameObject.name == "TestCard")
            {
                bs.OnAttackButton();
                Debug.Log("Karte1");
            }

            if(gameObject.name == "TestCard (1)")
            {
                bs.OnAttackButton2();
                Debug.Log("Karte2");
            }


            //Abfrage welche Karte - if > z.B. Karte 1 = Atk1 | if > Karte 2 = Atk2 (nach Name vllt)
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
}