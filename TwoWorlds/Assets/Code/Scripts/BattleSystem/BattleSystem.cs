using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState 
{ 
    START, 
    CHARSELECT,
    CARDSELECT,
    PLAYERTURN, 
    ENEMYTURN, 
    WON, 
    LOST 
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject CompanionPrefab;
    public GameObject Companion2Prefab;
    public GameObject enemyPrefab;

    public GameObject CharaSelectPanel;

    public Transform playerBattleStation;
    public Transform CompanionBattleStation;
    public Transform Companion2BattleStation;
    public Transform enemyBattleStation;

    public BattleState state;

    Unit playerUnit;
    Unit Companion1Unit;
    Unit Companion2Unit;
    Unit enemyUnit;

    private Card Cd;
    private ButtonSwitch BSw;
    private Unit Un;

    private int DmgDealt;
    public int APCost;
    private int Heals;
    public Unit ChosenChara;
    private BattleHud ChosenHud;

    public bool Cardsplayable;
    public bool NoAPLeft;
    bool MCDead = false;
    bool Comp1Dead = false;
    bool Comp2Dead = false;

    public Text discardText;
    public Text deckText;
    public Text dialogueText;
    public Button AttackButton;
    public Button HealButton;
    public Button SkipButton;
    public Button playerButton;
    public Button Comp1Button;
    public Button Comp2Button;

    public BattleHud playerHud;
    public BattleHud Companion1Hud;
    public BattleHud Companion2Hud;
    public BattleHud enemyHud;

    public List<Card> deck = new List<Card>();
    public List<Card> discarded = new List<Card>();
    //public List<TemporaryCard> deck = new List<TemporaryCard>(); ?

    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    //int index = Card.IndexOf(Inventory);

    void Start()
    {
        Cd = FindObjectOfType<Card>();
        BSw = FindObjectOfType<ButtonSwitch>();
        Un = FindObjectOfType<Unit>();
        CharaSelectPanel.gameObject.SetActive(false);
        Cardsplayable = false;

        state = BattleState.START;


        StartCoroutine(SetupBattle());
        DisableButtonOnClick();
    }

    private void Update()
    {
        deckText.text = deck.Count.ToString();
        discardText.text = discarded.Count.ToString();
    }


    IEnumerator SetupBattle()
    {
        GameObject PlayerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = PlayerGO.GetComponent<Unit>();

        GameObject EnemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = EnemyGO.GetComponent<Unit>();

        GameObject PlayerGO1 = Instantiate(CompanionPrefab, CompanionBattleStation);
        Companion1Unit = PlayerGO1.GetComponent<Unit>();

        GameObject PlayerGO2 = Instantiate(Companion2Prefab, Companion2BattleStation);
        Companion2Unit = PlayerGO2.GetComponent<Unit>();


        dialogueText.text = "A smelly " + enemyUnit.unitName + " approaches . . .";


        playerHud.SetHub(playerUnit);
        enemyHud.SetHub(enemyUnit);
        Companion1Hud.SetHub(Companion1Unit);
        Companion2Hud.SetHub(Companion2Unit);

        yield return new WaitForSeconds(0.5f);

        dialogueText.text = "Here are your Cards";
        //Cardpull();
        InvokeRepeating(nameof(Cardpull), 1f, 0.3f);



       
       
           // StartCoroutine(MyCounter(5));
        



        yield return new WaitForSeconds(4f);
        state = BattleState.CHARSELECT;
        CharaSelect();
        
        //CancelInvoke();
        //state = BattleState.PLAYERTURN;
        //PlayerTurn();
    }

    void CharaSelect()
    {

        if (MCDead == true)
        {
            playerButton.interactable = false;
        }

        if (Comp1Dead == true)
        {
            Comp1Button.interactable = false;
        }

        if (Comp2Dead == true)
        {
            Comp2Button.interactable = false;
        }
        



        CharaSelectPanel.gameObject.SetActive(true);
        //Auf Charawahl warten per Button

        //if(deck.Count >= 5)
        //{
        //    InvokeRepeating(nameof(Cardpull), 0.3f, 0.3f);
        //}
    }

        //IEnumerator MyCounter(int number)
        //{
        //    int i = 0;
        //    while (i < number)
        //    {
        //    Cardpull();
        //    //yield return 0; //Wait 1 Frame
        //    yield return new WaitForSeconds(1.0f); //wait 1 second per interval
        //    }
        //}

    void Cardpull()
    {
        if (deck.Count >= 1)
        {
            Card randomCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    //if (Cd.CardSet != true && CardSet2 != true)
                    //{ 

                    //}

                        randomCard.gameObject.SetActive(true);
                    //bool CardSet2 = true;


                        randomCard.handindex = i;
                        randomCard.transform.position = cardSlots[i].position;
                        randomCard.hasBeenPlayed = false;

                        availableCardSlots[i] = false;
                        deck.Remove(randomCard);
                        return;

                    // Debug.Log("Card" + i);
                }
            }
        }

    }

    void Shuffle()
    {
        if (discarded.Count >= 5)
        {
            foreach (Card card in discarded)
            {
                deck.Add(card);
            }
            discarded.Clear();
       InvokeRepeating(nameof(Cardpull), 0.3f, 0.3f);
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Pick a Card!";
        EnableButton();
    }

    IEnumerator PlayerAttack()
    {
        ChosenChara.DrainAP(APCost);
        ChosenHud.SetAP(ChosenChara.CurrentAP);


        DmgDealt += BSw.CharDmg;

        bool isDead = enemyUnit.TakeDamage(DmgDealt);

        Debug.Log("Dmg zsm: " + DmgDealt);

        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Attack 1: ...";


        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());

        }

    }


    IEnumerator PlayerHeal()
    {
        Debug.Log("HealingRec:" + Heals);
        ChosenChara.DrainAP(APCost);
        ChosenHud.SetAP(ChosenChara.CurrentAP);

        ChosenChara.Heal(Heals);
        ChosenHud.SetHP(ChosenChara.currentHP);

        dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator SkipMove()
    {

        ChosenChara.GetAP(10);
        ChosenHud.SetAP(ChosenChara.CurrentAP);

        Cardsplayable = false;

        dialogueText.text = "You skip a move and regain some AP";
        yield return new WaitForSeconds(2f);


        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }



    public void EnemyATK(string Char)
    {  
        if (Char == "MC")
        {
            //Setzt je nach gewähltem Charakter, das Hud oder Unit auf die zutreffende Unit oder Hud

            ChosenChara = playerUnit;
            ChosenHud = playerHud;
           
            
            Debug.Log("Unit =" + ChosenChara);
        }

        if (Char == "Companion1")
        {
            ChosenChara = Companion1Unit;
            ChosenHud = Companion1Hud;

           // NoAPLeft = true;
            Debug.Log("Unit =" + ChosenChara);
        }

        if (Char == "Companion2")
        {
            ChosenChara = Companion2Unit;
            ChosenHud = Companion2Hud;

           // NoAPLeft = true;
            Debug.Log("Unit =" + ChosenChara);
        }
            //if(ChosenChara.CurrentAP <= 0)
            //{

            //    NoAPLeft = true;

            //}
   
    }


    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);


        //Randomized Enemy Attack, 1 of 3
        int randomNumber;
        randomNumber = Random.Range(0, 3);

        //dialogueText.text = "Number: " + randomNumber;

        NoAPLeft = false;


        if (randomNumber == 0)
        {

            bool isDead = ChosenChara.TakeDamage(5);
            ChosenHud.SetHP(ChosenChara.currentHP);
            Debug.Log("Dmg1 to: " + ChosenChara);


            yield return new WaitForSeconds(2f);

            if (isDead)
            {

                if (ChosenChara == playerUnit)
                {
                    MCDead = true;
                    Debug.Log("MCDead");
                }

                if (ChosenChara == Companion1Unit)
                {
                    Comp1Dead = true;
                    Debug.Log("CompanionDead");
                }

                if (ChosenChara == Companion2Unit)
                {
                    Comp2Dead = true;
                    Debug.Log("Companion2Dead");
                }


                dialogueText.text = "Your Character is down!";

                if (MCDead == true && Comp1Dead == true && Comp2Dead == true)
                {
                    Debug.Log("LOST");
                    state = BattleState.LOST;
                    EndBattle();
                }

                else
                {
                    Shuffle();
                    yield return new WaitForSeconds(4f); //Time before Character selection starts
                    state = BattleState.CHARSELECT;
                    CharaSelect();
                }
            }
            else
            {
                Shuffle();
                yield return new WaitForSeconds(4f); //Time before Character selection starts
                state = BattleState.CHARSELECT;
                CharaSelect();
            }
        }

        if (randomNumber == 1)
        {
            bool isDead = ChosenChara.TakeDamage(10);
            ChosenHud.SetHP(ChosenChara.currentHP);

            Debug.Log("Dmg2 to: " + ChosenChara);

            yield return new WaitForSeconds(2f);

            if (isDead)
            {

                if (ChosenChara == playerUnit)
                {
                    MCDead = true;
                    Debug.Log("MCDead");
                }

                if (ChosenChara == Companion1Unit)
                {
                    Comp1Dead = true;
                    Debug.Log("CompanionDead");
                }

                if (ChosenChara == Companion2Unit)
                {
                    Comp2Dead = true;
                    Debug.Log("Companion2Dead");
                }

                dialogueText.text = "Your Character is down!";

                if (MCDead == true && Comp1Dead == true && Comp2Dead == true)
                {
                    Debug.Log("LOST");
                    state = BattleState.LOST;
                    EndBattle();
                }

                else
                {
                    Shuffle();
                    yield return new WaitForSeconds(4f); //Time before Character selection starts
                    state = BattleState.CHARSELECT;
                    CharaSelect();
                }
            }
            else
            {
                Shuffle();
                yield return new WaitForSeconds(2f); //Time before Character selection starts
                state = BattleState.PLAYERTURN;
                CharaSelect();
            }
        }

        if (randomNumber == 2)
        {
            bool isDead = ChosenChara.TakeDamage(15);
            ChosenHud.SetHP(ChosenChara.currentHP);

            Debug.Log("Dmg3 to: " + ChosenChara);

            yield return new WaitForSeconds(2f);

            if (isDead)
            {

                if (ChosenChara == playerUnit)
                {
                    MCDead = true;
                    Debug.Log("MCDead");
                }

                if (ChosenChara == Companion1Unit)
                {
                    Comp1Dead = true;
                    Debug.Log("CompanionDead");
                }

                if (ChosenChara == Companion2Unit)
                {
                    Comp2Dead = true;
                    Debug.Log("Companion2Dead");
                }

                dialogueText.text = "Your Character is down!";

                if (MCDead == true && Comp1Dead == true && Comp2Dead == true)
                {
                    Debug.Log("LOST");
                    state = BattleState.LOST;
                    EndBattle();
                }

                else
                {
                    Shuffle();
                    yield return new WaitForSeconds(4f); //Time before Character selection starts
                    state = BattleState.CHARSELECT;
                    CharaSelect();
                }
            }
            else
            {
                Shuffle();
                yield return new WaitForSeconds(2f); //Time before Character selection starts
                state = BattleState.PLAYERTURN;
                CharaSelect();
            }
        }

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }


    public void OnCharaSelect()
    {
        CancelInvoke();
        CharaSelectPanel.gameObject.SetActive(false);
        //Atk Button enable oder SetActive
        //Karten als Sprite an Position und Panel mit jeweiligen Button wird aktiviert

        Cardsplayable = true;



        //Cd.NOAP = false;
        //Cd.CardAvailable();
        //if (ChosenChara.CurrentAP < 0)
        //{
        //    Cardsplayable = false;
        //}


        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void OnAttackButton(int CaaardDmg, int APStat)
    {
        if (state != BattleState.PLAYERTURN)
            return;
        DmgDealt = CaaardDmg;
        APCost = APStat;
        Debug.Log("AP" + APCost);


        StartCoroutine(PlayerAttack());

        DisableButtonOnClick();
    }


    public void OnHealButton(int Healing, int APStat)
    {
        if (state != BattleState.PLAYERTURN)
            return;
        Heals = Healing;
        APCost = APStat;
        StartCoroutine(PlayerHeal());

        DisableButtonOnClick();
    }

    public void OnSkipMoveButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(SkipMove());

        DisableButtonOnClick();
    }


    public void DisableButtonOnClick()
    { 
        HealButton.interactable = false;
        SkipButton.interactable = false;
    }

    public void EnableButton()
    {
        HealButton.interactable = true;
        SkipButton.interactable = true;
    }
}