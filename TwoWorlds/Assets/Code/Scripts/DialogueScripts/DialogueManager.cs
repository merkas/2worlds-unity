using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;

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
    PlayerController player;

    //UI
    public GameObject dialogueWindow;
    public Text npcNameText;
    public Text dialogueText;
    public GameObject responseField;
    public Image itemImage;

    public Button ContinueButton;

    public GameObject[] response; //Button-Prefabs
    GameObject ButtonPrefab; // loaded Prefab
    Button[] choices; // all Buttons in ButtonPrefab
    int chosenIndex;
    Queue<string> sentences;
    string[] message = new string[1];

    bool messageBeforeNpc;

    private void Start()
    {
        state = DialogueState.NODIALOGUE;
        UpdateUI();
    }

    public void NewConversation(GameObject Npc, GameObject Player)
    {
        npc = Npc;
        playerObject = Player;
    }

    public void StartConversation()//called by player
    {
        dialoguePartner = npc.GetComponent<DialoguePartner>();
        player = playerObject.GetComponent<PlayerController>();
        sentences = new Queue<string>();

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

        if (dialoguePartner.NpcWithMenu == true)
        {
            CreateMenu();
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
            Sprite sprite = dialoguePartner.item.item.icon;
            string[] message = dialoguePartner.itemText;
            SystemMessage(dialoguePartner.itemText, dialoguePartner.item.item);
            
            Inventory.instance.AddItem(dialoguePartner.item);
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
                        if (playerQuest.questProgress == playerQuest.maxProgress) // quest completed
                        {
                            int index = npcQuest.reactionToProgress.Length;
                            sentences.Enqueue(npcQuest.reactionToProgress[index]);
                            NextText();

                            dialoguePartner.ChangedQuestStatus(playerQuest, "completed");

                            if (playerQuest.reward != null)
                            {
                                Inventory.instance.AddItem(playerQuest.reward); // get reward

                                message[0] = dialoguePartner.npcName + " gave you " +
                                    dialoguePartner.activeDialoguePart.playerResponse[index].getItem.item.itemName + "!";
                                SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].getItem.item);
                            }
                        }
                        else
                        {
                            playerQuest.questProgress = npcQuest.questProgress;
                            playerQuest.formerQuestProgress = npcQuest.formerQuestProgress;
                            sentences.Enqueue(npcQuest.reactionToProgress[npcQuest.questProgress]);
                            NextText();
                        }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) NextText();
    }

    private void UpdateUI() //UI (de)activation
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
        }

        if (state == DialogueState.ENDDIALOGUE || state == DialogueState.NODIALOGUE)
        {
            dialogueWindow.SetActive(false);
        }

        if (state != DialogueState.SYSTEMMESSAGE)
            itemImage.enabled = false;
    }

    void SystemMessage(string[] message, Item item = default)
    {
        state = DialogueState.SYSTEMMESSAGE;
        UpdateUI();

        foreach (string text in message)
        {
            sentences.Enqueue(text);
        }
        NextText();
        
        //show animation of Sprite moving to screen center?
        itemImage.sprite = item.icon;
        if (item != default) itemImage.enabled = true;
    }

    void Greeting() //npc greeting
    {
        if (dialoguePartner.NpcWithMenu != true)
        {
            dialoguePartner.ChooseGreeting(player.corruptionStat);

            sentences.Enqueue(dialoguePartner.chosenGreeting);
            NextText();
        }  

        else if (dialoguePartner.NpcWithMenu == true)
        {
            state = DialogueState.NPCTALKING;
            responseField.SetActive(false);
            sentences.Enqueue(dialoguePartner.thisNpcDialogue.dialogueMenu.npcMainText);
            NextText();
        }
    }

    void NpcTalking(int index = default)
    {
        state = DialogueState.NPCTALKING;
        UpdateUI();

        messageBeforeNpc = false;
        if (dialoguePartner.NpcWithMenu == true)
        {
            responseField.SetActive(false);
            sentences.Enqueue(dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption[index].npcAnswer);
            NextText();
        }
        else if (dialoguePartner.NpcWithMenu != true)
        {
            foreach (string text in dialoguePartner.activeDialoguePart.npcText)
            {
                sentences.Enqueue(text);
            }

            NextText();
        }
    }

    public void NextText() //ContinueButton
    {
        if (state == DialogueState.NPCTALKING || state == DialogueState.SYSTEMMESSAGE)
        {
            if (sentences.Count == 0)
            {
                sentences.Clear();
                if (state == DialogueState.NPCTALKING) PlayerTurn();
                else if (state == DialogueState.SYSTEMMESSAGE) NpcTalking();
                return;
            }

            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }
        else if (state == DialogueState.AUTOPLAYERRESPONSE)
            SelectedResponse();
    }

    void PlayerTurn()
    {
        state = DialogueState.PLAYERTURN;
        UpdateUI();

        if (dialoguePartner.NpcWithMenu == true) // menu
        {
            state = DialogueState.PLAYERTURN;
            responseField.SetActive(true);
        }
        if (dialoguePartner.NpcWithMenu != true
            && dialoguePartner.activeDialoguePart.playerChoice != true) // automatic response
        {
            state = DialogueState.AUTOPLAYERRESPONSE;
            if (dialoguePartner.activeDialoguePart.playerResponse.Length == 0) ConversationEnd();
            else
            {
                sentences.Enqueue(dialoguePartner.activeDialoguePart.playerResponse[0].answer);
                NextText();
            }
        }
        else if (dialoguePartner.NpcWithMenu != true
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
        if (dialoguePartner.NpcWithMenu != true)
            Destroy(ButtonPrefab);
    }

    void SelectedResponse() //response chosen, NPC reaction awaited
    {
        if (dialoguePartner.NpcWithMenu != true) // choice
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
                    UpdateUI();

                    dialoguePartner.LoadNextDialoguePart(chosenIndex);
                    if (dialoguePartner.activeDialoguePart == null) ConversationEnd();
                    if (messageBeforeNpc == false)
                    {
                        NpcTalking();
                    }
                }
            }
        }

        else if (dialoguePartner.NpcWithMenu == true) // menu
        {
            if (dialoguePartner.thisNpcDialogue.dialogueMenu.menuOption[chosenIndex].endConversation == true)
                ConversationEnd();
            else
                NpcTalking(chosenIndex);

            
        }

        if (dialoguePartner.noDialogueLeft == true)
        {
            ConversationEnd();
        }
    }

    void CheckReactionToPlayerResponse(int index)
    {
        messageBeforeNpc = false;
        if (dialoguePartner.activeDialoguePart.playerResponse[index].getAnItem == true) // get item
        {
            message[0] = dialoguePartner.npcName + " gave you " +
                dialoguePartner.activeDialoguePart.playerResponse[index].getItem.item.itemName + "!";
            SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].getItem.item);

            Inventory.instance.AddItem(dialoguePartner.activeDialoguePart.playerResponse[index].getItem);

            messageBeforeNpc = true;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].giveAnItem == true) // give npc an item
        {
            Debug.Log("player gives an item");
            message[0] = "You gave " + dialoguePartner.npcName + " " +
                dialoguePartner.activeDialoguePart.playerResponse[index].giveItem.item.itemName + ".";
            SystemMessage(message, dialoguePartner.activeDialoguePart.playerResponse[index].giveItem.item);
            // check if quest item?
            Inventory.instance.RemoveItem(dialoguePartner.activeDialoguePart.playerResponse[index].giveItem);

            messageBeforeNpc = true;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questTrigger == true) // = accept quest
        {
            Quest newQuest = dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest;
            player.activeQuests.Add(newQuest);
            dialoguePartner.ChangedQuestStatus(newQuest, "triggered");
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].abandonedQuest == true) // = abandoned quest
        {
            dialoguePartner.ChangedQuestStatus(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest, "abandoned");
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questComplete == true) // = completed quest
        {
            Debug.Log("Quest ended");

            dialoguePartner.ChangedQuestStatus(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest, "completed");
            Inventory.instance.AddItem(dialoguePartner.activeDialoguePart.playerResponse[index].changedQuest.reward);

            // Npc Dialogue
            messageBeforeNpc = false;
        }
        if (dialoguePartner.activeDialoguePart.playerResponse[index].questProgress != 0) // quest progress within the dialogue
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
        if (dialoguePartner.activeDialoguePart.playerResponse[index].changeToMenuInNextConversation == true)
        {
            dialoguePartner.changeToMenuNpc = true;
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
        UpdateUI();

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

    void GiveItem(CompleteItem item)
    {
        Inventory.instance.RemoveItem(item);
        itemImage.sprite = item.item.icon;
        //show item Sprite
    }

    IEnumerator TypeSentence(string sentence) //animate every letter to appear one after one
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
            yield return null; // = wait for next frame
        }
    }
}
