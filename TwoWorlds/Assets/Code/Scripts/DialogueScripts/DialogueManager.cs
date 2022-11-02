using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum DialogueState
{
    NODIALOGUE,
    NPCTALKING,
    PLAYERTURN,
    PLAYERRESPONSE,
    AUTOPLAYERRESPONSE,
    SYSTEMMESSAGE,
    ENDDIALOGUE
}

public class DialogueManager : MonoBehaviour
{
    DialogueState state;

    GameObject npc;
    GameObject playerObject;

    DialoguePartner dialoguePartner;
    PlayerStandIn player;

    //everything needed of the UI
    public GameObject dialogueWindow;
    public Text npcNameText;
    public Text dialogueText;
    public GameObject responseField;
    public Image itemImage;

    public Button ContinueButton;

    public GameObject[] response; //Button-Prefabs
    GameObject ButtonPrefab;
    Button[] choices;
    int chosenIndex;
    Queue<string> npcSentences;
    string[] message = new string[1];

    bool messageBeforeNpc;

    private void Start()
    {
        state = DialogueState.NODIALOGUE;
    }

    public void NewConversation(GameObject Npc, GameObject Player)
    {
        npc = Npc;
        playerObject = Player;
    }

    public void StartConversation()//called by player
    {
        dialoguePartner = npc.GetComponent<DialoguePartner>();
        player = playerObject.GetComponent<PlayerStandIn>();
        npcSentences = new Queue<string>();

        // check for quest item in player inventory
        // check for active Quests
        //if (dialoguePartner.activeQuests.Count != 0)
        //{
        //    foreach(Quest activeQuest in dialoguePartner.activeQuests)
        //    {
        //        if (activeQuest.questNpcName == dialoguePartner.npcName)
        //        {
        //            //check progress and/or activeQuest.wantedItem
        //        }
        //    }
        //}

        Greeting();
        CheckQuestProgress();
        if (dialoguePartner.thisNpcDialogue.NpcWithMenu == true)
        {
            CreateMenu();
            NpcMenu();
        }
        else
        {
            dialoguePartner.GetData(player.corruptionStat); // get dialogue
            if (dialoguePartner.corruptionOutOfRange == true)
            {
                state = DialogueState.NODIALOGUE; //falls Probleme, ConversationEnd();
            }
            else if (dialoguePartner.corruptionOutOfRange != true && dialoguePartner.getItem == true) GetItemCheck(); 
            else NpcTalking();
        }

        OpenDialogueWindow();
        
    }

    void GetItemCheck()
    {
        if (dialoguePartner.CheckForItemConditions(player.corruptionStat) == true)
        {
            Sprite sprite = dialoguePartner.itemSprite;
            string[] message = dialoguePartner.itemText;
            SystemMessage(dialoguePartner.itemText, dialoguePartner.itemSprite);
            //get inventory of player and add item
        }
        else NpcTalking();
    }

    void CheckQuestProgress()
    {
        // check for wanted item to trigger or end a quest

        foreach (Quest playerQuest in player.activeQuests)
        {
            foreach (Quest npcQuest in dialoguePartner.activeQuests)
            {
                if (playerQuest == npcQuest)
                {
                    if (playerQuest.questProgress >= playerQuest.formerQuestProgress)
                    {
                        playerQuest.questProgress = npcQuest.questProgress;
                        playerQuest.formerQuestProgress = npcQuest.formerQuestProgress;
                        npcSentences.Enqueue(npcQuest.reactionToProgress[npcQuest.questProgress]);
                        NextText();
                    }
                    if (npcQuest.completedQuest == true)
                    {
                        int index = npcQuest.reactionToProgress.Length;
                        npcSentences.Enqueue(npcQuest.reactionToProgress[index]);
                        NextText();

                        // get reward + check

                        message[0] = dialoguePartner.npcName + " gave you " + 
                            dialoguePartner.activeDialoguePart.playerResponse[index].getItemName + "!";
                        SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].getItem);
                    }
                }
            }
        }
    }

    void OpenDialogueWindow()
    {
        npcNameText.text = dialoguePartner.npcName;
        dialogueWindow.SetActive(true);
    }

    private void Update() //UI (de)activation
    {
        if (state == DialogueState.PLAYERTURN)
            ContinueButton.interactable = false;
        else
            ContinueButton.interactable = true;

        if (state != DialogueState.PLAYERTURN || state != DialogueState.PLAYERRESPONSE)
        {
            responseField.SetActive(false);
            dialogueText.enabled = true;
        }

        if (state == DialogueState.PLAYERTURN || state == DialogueState.PLAYERRESPONSE)
        {
            responseField.SetActive(true);
            dialogueText.enabled = false;
            //itemImage.enabled = false;
        }

        if (state == DialogueState.ENDDIALOGUE || state == DialogueState.NODIALOGUE)
        {
            dialogueWindow.SetActive(false);
            //itemImage.enabled = false;
        }

        if (state != DialogueState.SYSTEMMESSAGE)
            itemImage.enabled = false;
    }

    void SystemMessage(string[] message, Sprite item = default)
    {
        state = DialogueState.SYSTEMMESSAGE;
        foreach (string text in message)
        {
            npcSentences.Enqueue(text);
        }
        NextText();
        
        //show animation of Sprite moving to screen center?
        itemImage.sprite = item;
        itemImage.enabled = true;
    }

    void Greeting()
    {
        dialoguePartner.ChooseGreeting(player.corruptionStat);
        
        npcSentences.Enqueue(dialoguePartner.chosenGreeting);
        NextText();
    }

    void NpcTalking(int index = default)
    {
        state = DialogueState.NPCTALKING;
        messageBeforeNpc = false;
        if (dialoguePartner.thisNpcDialogue.NpcWithMenu == true)
        {
            responseField.SetActive(false);
            npcSentences.Enqueue(dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption[index].npcAnswer);
            NextText();
        }
        else if (dialoguePartner.thisNpcDialogue.NpcWithMenu != true)
        {
            foreach (string text in dialoguePartner.activeDialoguePart.npcText)
            {
                npcSentences.Enqueue(text);
            }

            NextText();
        }
    }

    void NpcMenu()
    {
        state = DialogueState.PLAYERTURN;
        PlayerTurn();
    }

    public void NextText() //ContinueButton
    {
        if (state == DialogueState.NPCTALKING || state == DialogueState.SYSTEMMESSAGE)
        {
            if (npcSentences.Count == 0)
            {
                npcSentences.Clear();
                if (state == DialogueState.NPCTALKING) PlayerTurn();
                else if (state == DialogueState.SYSTEMMESSAGE) NpcTalking();
                return;
            }

            string sentence = npcSentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }
        else if (state == DialogueState.AUTOPLAYERRESPONSE)
            SelectedResponse();
    }

    void PlayerTurn()
    {
        state = DialogueState.PLAYERTURN;

        if (dialoguePartner.thisNpcDialogue.NpcWithMenu == true) // menu
        {
            state = DialogueState.PLAYERTURN;
            responseField.SetActive(true);
        }
        if (dialoguePartner.thisNpcDialogue.NpcWithMenu != true
            && dialoguePartner.activeDialoguePart.playerChoice != true) // automatic response
        {
            state = DialogueState.AUTOPLAYERRESPONSE;
            if (dialoguePartner.activeDialoguePart.playerResponse.Length == 0) ConversationEnd();
            else
            {
                npcSentences.Enqueue(dialoguePartner.activeDialoguePart.playerResponse[0].answer);
                NextText();
            }
        }
        else if (dialoguePartner.thisNpcDialogue.NpcWithMenu != true
            && dialoguePartner.activeDialoguePart.playerChoice == true)// choice
        {
            state = DialogueState.PLAYERRESPONSE;
            responseField.SetActive(true);
            CreateResponseUI(dialoguePartner.activeDialoguePart.responseNumbers);

            choices = responseField.GetComponentsInChildren<Button>();
            int selection = 0;

            foreach (Answer response in dialoguePartner.activeDialoguePart.playerResponse)
            {
                int a = selection;
                choices[selection].GetComponentInChildren<Text>().text = response.answer;
                choices[selection].GetComponentInChildren<Button>().onClick.AddListener(delegate { ReturnButtonIndex(a); });
                choices[selection].GetComponentInChildren<Button>().onClick.AddListener(ResponseButton);

                selection++;
            }
        }
    }

    int ReturnButtonIndex(int i) // get index of chosen answer
    {
        chosenIndex = i;
        return chosenIndex;
    }

    public void ResponseButton() //get method on Prefab or delete prefab
    {
        SelectedResponse();
        if (dialoguePartner.thisNpcDialogue.NpcWithMenu != true)
            Destroy(ButtonPrefab);
    }

    void SelectedResponse() //response chosen, NPC reaction awaited
    {
        if (dialoguePartner.thisNpcDialogue.NpcWithMenu != true) // choice
        {
            player.corruptionStat +=
                    dialoguePartner.activeDialoguePart.playerResponse[chosenIndex].corruptionStatChange;
            // visual reaction

            CheckReactionToPlayerResponse(chosenIndex);

            if (dialoguePartner.activeDialoguePart.playerResponse[chosenIndex].startAFight == true)
            {
                ConversationEnd();
                //change to fight scene
                Debug.Log("Change to fight scene");
            }
            else
            {
                dialoguePartner.CheckIfConversationEnded(chosenIndex);

                if (dialoguePartner.noDialogueLeft != true) // = Dialogue continues
                {
                    state = DialogueState.NPCTALKING;
                    //CheckReactionToPlayerResponse(chosenIndex);

                    dialoguePartner.LoadNextDialoguePart(chosenIndex);
                    if (dialoguePartner.activeDialoguePart == null) ConversationEnd();
                    if (messageBeforeNpc == false)
                    {
                        NpcTalking();
                    }
                }
            }
        }

        else if (dialoguePartner.thisNpcDialogue.NpcWithMenu == true) // menu
        {
            if (dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption[chosenIndex].endConversation == true)
                ConversationEnd();
            else
                NpcTalking(chosenIndex);

            //dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption[chosenIndex].usedQuestion = true;
        }

        if (dialoguePartner.noDialogueLeft == true)
        {
            ConversationEnd();
        }
    }

    void CheckReactionToPlayerResponse(int index) //questtrigger / progress / abandonded
    {
        messageBeforeNpc = false;
        if (dialoguePartner.activeDialoguePart.playerResponse[index].getAnItem == true)
        {
            message[0] = dialoguePartner.npcName + " gave you " +
                dialoguePartner.activeDialoguePart.playerResponse[index].getItemName + "!";
            SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].getItem);
            messageBeforeNpc = true;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].giveAnItem == true)
        {
            Debug.Log("player gives an item");
            message[0] = "You gave " + dialoguePartner.npcName + " " +
                dialoguePartner.activeDialoguePart.playerResponse[index].giveItemName + ".";
            SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].giveItem);
            // check if quest gets triggered
            // get item out of inventory
            messageBeforeNpc = true;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questTrigger == true) // = accept quest
        {
            player.activeQuests.Add(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest);
            dialoguePartner.ChangedQuestStatus(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest, "triggered");
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].abandonedQuest == true)
        {
            dialoguePartner.ChangedQuestStatus(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest, "abandoned");
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questComplete == true)
        {
            Debug.Log("Quest ended");

            dialoguePartner.ChangedQuestStatus(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest, "completed");
            // get reward
            // Npc Dialogue
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questProgress != 0) // progress within the dialogue
        {
            foreach(Quest quest in dialoguePartner.thisNpcDialogue.quest)
            {
                if (quest.title == dialoguePartner.activeDialoguePart.playerResponse[index].questTitle)
                {
                    message[0] = quest.reactionToProgress[quest.questProgress];
                    quest.questProgress += dialoguePartner.activeDialoguePart.playerResponse[index].questProgress;
                    SystemMessage(message); //Npc reaction to current quest progress
                    // noch unvollständig?
                }
            }
        }
    }

    void ChangeMenuOptionColor() //empty
    {
        // if already used grey out text
    }

    void CreateMenu()
    {
        int numberOfResponses = dialoguePartner.thisNpcDialogue.dialogueMenu.index - 2;  //since the prefab has a min of 2 Buttons -2
        ButtonPrefab = Instantiate(response[numberOfResponses], responseField.transform); //loads a Button prefab

        choices = ButtonPrefab.GetComponentsInChildren<Button>();
        int selection = 0;

        foreach (DialogueMenuOption question in dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption) // give every Button in choices a playerQuestion
        {
            int a = selection;
            choices[selection].GetComponentInChildren<Text>().text = question.playerQuestion;
            choices[selection].GetComponentInChildren<Button>().onClick.AddListener(delegate { ReturnButtonIndex(a); });
            choices[selection].GetComponentInChildren<Button>().onClick.AddListener(ResponseButton);

            selection++;
        }
    }

    void CreateResponseUI(int numberOfResponses) //creates response Buttons in responseField
    {
        ButtonPrefab = Instantiate(response[numberOfResponses - 2], responseField.transform); //loads a Button prefab
    }

    void ConversationEnd()
    {
        dialoguePartner.SaveNewInfo();
        state = DialogueState.ENDDIALOGUE;
        npcNameText.text = "";
        dialogueText.text = "";
        
        dialoguePartner.activeDialoguePart = null;
        dialoguePartner.noDialogueLeft = false;
        
        Destroy(ButtonPrefab);
        player.ConversationEnded();
    }

    void CheckForQuestItem()
    {
        // get quest item in database
        // get player inventory
        // check for quest item
        // select npc reaction
    }

    void GiveItem()
    {
        //remove item in inventory
        //show item Sprite
    }

    //langsamer machen
    IEnumerator TypeSentence(string sentence) //animate every letter to appear one after one
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
            yield return null; // wait for next frame
        }
    }
}
