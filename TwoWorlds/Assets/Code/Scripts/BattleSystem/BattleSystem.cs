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
    public GameObject enemyPrefab;

    public GameObject CharaSelectPanel;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleState state;

    Unit playerUnit;
    Unit enemyUnit;

    [SerializeField]
    public TemporaryCard Karte1;
    [SerializeField]
    public TemporaryCard Karte2;
    //In Inspector ziehen

    [SerializeField]
    public Unit Companion1;


    public bool Cardsplayable;

    public Text discardText;
    public Text deckText;
    public Text dialogueText;
    public Button AttackButton;
    public Button AttackButton2;
    public Button AttackButton3;
    public Button HealButton;
    public Button SkipButton;
    //Später vllt als Panel für die Karten und dass dann disablen

    public BattleHud playerHud;
    public BattleHud enemyHud;

    public List<Card> deck = new List<Card>();
    public List<Card> discarded = new List<Card>();
    //public List<TemporaryCard> deck = new List<TemporaryCard>(); ?

    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    //int index = Card.IndexOf(Inventory);

    void Start()
    {
        CharaSelectPanel.gameObject.SetActive(false);
        Cardsplayable = false;
        state = BattleState.START;


        StartCoroutine(SetupBattle());

        //Inventory.instance.deckCards
        //liste = inventory
        //for each
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

        dialogueText.text = "A smelly " + enemyUnit.unitName + " approaches . . .";

        //dialogueText.text = Karte.ATKValue + ""; ATK Value anzeigen lassen

        playerHud.SetHub(playerUnit);
        enemyHud.SetHub(enemyUnit);

        yield return new WaitForSeconds(0.5f);

        dialogueText.text = "Here are your Cards";
        //Cardpull();
        InvokeRepeating(nameof(Cardpull), 0.3f, 0.3f);


        yield return new WaitForSeconds(4f);
        state = BattleState.CHARSELECT;
        CharaSelect();

        //state = BattleState.PLAYERTURN;
        //PlayerTurn();
    }

    void CharaSelect()
    {
        CharaSelectPanel.gameObject.SetActive(true);
        //Auf Charawahl warten per Button
    }

    public void CardSelect()
    {
        CharaSelectPanel.gameObject.SetActive(false);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void Cardpull()
    {
        if(deck.Count >= 1)
        {
            Card randomCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if(availableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handindex = i;
                    randomCard.transform.position = cardSlots[i].position;
                    randomCard.hasBeenPlayed = false;

                    availableCardSlots[i] = false;
                    deck.Remove(randomCard);
                    Debug.Log("Card" + i);
                    return;
                }       
            }
        }

    }

    void Shuffle()
    {
        if(discarded.Count >= 5)
        {
            foreach (Card card in discarded)
            {
                deck.Add(card);
            }
            discarded.Clear();
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Pick a Card!";
        EnableButton();
    }

    IEnumerator PlayerAttack() //Karte 1
    {

        playerUnit.DrainAP(Karte1.APDrain);
        playerHud.SetAP(playerUnit.CurrentAP);

        bool isDead = enemyUnit.TakeDamage(Karte1.ATKValue); //Companion1.Damage

        //bool isDead = enemyUnit.TakeDamage(10 + playerUnit.damage); playerUnit = Companion ATK (set dmg) + Card Dmg = Dmg
        //or
        //bool isDead = enemyUnit.TakeDamage(playerUnit.damage); Dmg number from the Inspector

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

    IEnumerator PlayerAttack2()
    {

        playerUnit.DrainAP(Karte2.APDrain);
        playerHud.SetAP(playerUnit.CurrentAP);


        bool isDead = enemyUnit.TakeDamage(Karte2.ATKValue);
        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Attack 2: ...";


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

    IEnumerator PlayerAttack3()
    {

        playerUnit.DrainAP(8);
        playerHud.SetAP(playerUnit.CurrentAP);

        bool isDead = enemyUnit.TakeDamage(12);
        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Attack 3: ...";


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

        playerUnit.DrainAP(10);
        playerHud.SetAP(playerUnit.CurrentAP);


        playerUnit.Heal(5);
        playerHud.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator SkipMove()
    {

        playerUnit.GetAP(5);
        playerHud.SetAP(playerUnit.CurrentAP);

        dialogueText.text = "You skip a move and regain some AP";
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);


        //Randomized Enemy Attack, 1 of 3
        int randomNumber;
        randomNumber = Random.Range(0, 3);

        //dialogueText.text = "Number: " + randomNumber;

        if (randomNumber == 0)
            {
                bool isDead = playerUnit.TakeDamage(5);
                playerHud.SetHP(playerUnit.currentHP);
                yield return new WaitForSeconds(2f);

                if (isDead)
                {
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

        if(randomNumber == 1)
        {
            bool isDead = playerUnit.TakeDamage(10);
            playerHud.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(4f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                Shuffle();
                yield return new WaitForSeconds(4f); //Time before Character selection starts
                state = BattleState.PLAYERTURN;
                CharaSelect();
            }
        }

        if (randomNumber == 2)
        {
            bool isDead = playerUnit.TakeDamage(15);
            playerHud.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                Shuffle();
                yield return new WaitForSeconds(2f); //Time before Character selection starts
                state = BattleState.PLAYERTURN;
                CharaSelect();
            }
        }


        //Randomize();


        //bool isDead = playerUnit.TakeDamage(10);
        //bool isDead = playerUnit.TakeDamage(enemyUnit.damage); dmg numbers from Inspector
        //playerHud.SetHP(playerUnit.currentHP);

        //yield return new WaitForSeconds(1f);

        //if (isDead)
        //{
        //    state = BattleState.LOST;
        //    EndBattle();
        //}
        //else
        //{
        //    state = BattleState.PLAYERTURN;
        //    PlayerTurn();
        //}
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
        CharaSelectPanel.gameObject.SetActive(false);
        //Atk Button enable oder SetActive
        //Karten als Sprite an Position und Panel mit jeweiligen Button wird aktiviert
        Cardsplayable = true;
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }




    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());

        DisableButtonOnClick();
    }

    public void OnAttackButton2()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack2());

        DisableButtonOnClick();
    }

    public void OnAttackButton3()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack3());

        DisableButtonOnClick();
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

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
        AttackButton.interactable = false;
        AttackButton2.interactable = false;
        AttackButton3.interactable = false;
        HealButton.interactable = false;
        SkipButton.interactable = false;

    }

    public void EnableButton()
    {
        AttackButton.interactable = true;
        AttackButton2.interactable = true;
        AttackButton3.interactable = true;
        HealButton.interactable = true;
        SkipButton.interactable = true;
    }
}