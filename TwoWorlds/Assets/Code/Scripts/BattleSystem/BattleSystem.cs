using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState 
{ 
    START, 
    PLAYERTURN, 
    ENEMYTURN, 
    WON, 
    LOST 
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleState state;

    Unit playerUnit;
    Unit enemyUnit;


    public Text dialogueText;
    public Button AttackButton;
    public Button HealButton;
    public Button SkipButton;
    //Später vllt als Panel für die Karten und dass dann disablen

    public BattleHud playerHud;
    public BattleHud enemyHud;

    void Start()
    {
        state = BattleState.START;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject PlayerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = PlayerGO.GetComponent<Unit>();

        GameObject EnemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = EnemyGO.GetComponent<Unit>();

        dialogueText.text = "A smelly " + enemyUnit.unitName + " approaches . . .";

        playerHud.SetHub(playerUnit);
        enemyHud.SetHub(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action: ";
        EnableButton();
    }

    IEnumerator PlayerAttack()
    {

        playerUnit.DrainAP(5);
        playerHud.SetAP(playerUnit.CurrentAP);

        bool isDead = enemyUnit.TakeDamage(10);

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
        //On AttackButton2 ?
        playerUnit.DrainAP(8);
        playerHud.SetAP(playerUnit.CurrentAP);

        bool isDead = enemyUnit.TakeDamage(20);
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
                yield return new WaitForSeconds(1f);

                if (isDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
        }

        if(randomNumber == 1)
        {
            bool isDead = playerUnit.TakeDamage(10);
            playerHud.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

        if (randomNumber == 2)
        {
            bool isDead = playerUnit.TakeDamage(15);
            playerHud.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
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


    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());

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
        HealButton.interactable = false;
        SkipButton.interactable = false;

    }

    public void EnableButton()
    {
        AttackButton.interactable = true;
        HealButton.interactable = true;
        SkipButton.interactable = true;
    }
}