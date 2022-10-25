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
    ENDDIALOGUE
}

public class DialogueManager : MonoBehaviour
{
    DialogueState state;

    GameObject npc;
    GameObject player;

    DialoguePartner dialoguePartner;
    string activeText;

    //everything needed of the UI
    public GameObject dialogueWindow;
    public Text npcNameText;
    public Text dialogueText;
    public GameObject responseField;
    public GameObject menu;

    public Button ContinueButton;

    public GameObject[] response; //Button-Prefabs
    GameObject ButtonPrefab;
    Button[] choices;
    int chosenIndex;
    Queue<string> npcSentences;

    private void Start()
    {
        state = DialogueState.NODIALOGUE;
    }

    public void NewConversation(GameObject Npc, GameObject Player)
    {
        npc = Npc;
        player = Player;
    }

    public void StartConversation()//called by player
    {
        dialoguePartner = npc.GetComponent<DialoguePartner>();
        npcSentences = new Queue<string>();
        
        dialoguePartner.ChooseGreeting(player.GetComponent<PlayerStandIn>().corruptionStat);
        //Greeting();

        if (dialoguePartner.thisNpcDialogue.NpcWithMenu == true)
        {
            CreateMenu();
            NpcMenu();
        }
        else
        {
            dialoguePartner.GetData(player.GetComponent<PlayerStandIn>().corruptionStat); // get dialogue
            NpcTalking();
        }

        OpenDialogueWindow();
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
        }

        if (state == DialogueState.ENDDIALOGUE || state == DialogueState.NODIALOGUE)
        {
            dialogueWindow.SetActive(false);
        }
    }

    void NpcTalking(int index = default)
    {
        state = DialogueState.NPCTALKING;

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
        if (state == DialogueState.NPCTALKING)
        {
            if (npcSentences.Count == 0)
            {
                PlayerTurn();
                npcSentences.Clear();
                return;
            }

            string sentence = npcSentences.Dequeue();
            //StopAllCoroutines();
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
            player.GetComponent<PlayerStandIn>().corruptionStat +=
                    dialoguePartner.activeDialoguePart.playerResponse[chosenIndex].corruptionStatChange;
            // visual reaction
            // ShowTextReaction();

            dialoguePartner.CheckIfConversationEnded(chosenIndex);

            if (dialoguePartner.noDialogueLeft != true)
            {
                state = DialogueState.NPCTALKING;
                dialoguePartner.CheckReactionToPlayerResponse(chosenIndex); //test
                dialoguePartner.LoadNextDialoguePart(chosenIndex);
                NpcTalking();
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

    void ShowTextReaction()
    {
        //like a system message in dialogue box
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
        activeText = null;
        dialoguePartner.activeDialoguePart = null;
        //destroy children objects of response field
        Destroy(ButtonPrefab);
        player.GetComponent<PlayerStandIn>().ConversationEnded();
        //foreach (Button choice in choices)
        //    Destroy(choice); //test
    }

    void CheckForItem()
    {
        if (dialoguePartner.thisNpcDialogue.GetItem == true)
        {
            if (dialoguePartner.thisNpcDialogue.getCondition == true)
            {
                dialoguePartner.CheckForItemConditions(player.GetComponent<PlayerStandIn>().corruptionStat);
            }
            else
            {
                dialoguePartner.thisNpcDialogue.GetItem = false; //item gegeben
            }
            dialogueText.text = "You got an item!";
            //show item Sprite
            //get item in inventory
        }
    }

    void GiveItem()
    {
        //remove item in inventory
        //show item Sprite
        //ggf change npc stats
    }

    //langsamer machen
    IEnumerator TypeSentence(string sentence) //animate every letter to appear one after one
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // wait for next frame
        }
    }
}
