using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            //return;
        }
        else instance = this;
    }
    #endregion

    public GameObject textBoxWindow;
    public string timelineDialogue;
    public Image charSprite;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueTextInBox;
    TextMeshProUGUI dialogueText;

    //PlayableDirector activeDirector;
    //bool activeTimeline = false;

    public Button continueButton; //continue in text display
    public Button continueTimelineButton;
    Queue<string> sentencesToDisplay;

    private void Start()
    {
        OpenTextBox(false);
        textBoxWindow.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueTimelineButton.gameObject.SetActive(false);
        //DontDestroyOnLoad(this);
    }

    public void SetDialogue(string dialogue, string charName, Sprite characterSprite = null)
    {
        if (characterSprite != null) charSprite.sprite = characterSprite;

        if (dialogueText == null) dialogueText = dialogueTextInBox;

        dialogueText.text = dialogue;
        
        characterName.text = charName;
        if (textBoxWindow.activeSelf == false) OpenTextBox(true);
    }

    public void GetBinding(TextMeshProUGUI text) // dialogue tracks, has to be reset after use
    {
        dialogueText = text; // deleted, when timeline pauses
    }

    public void OpenTextBox(bool active)
    {
        if (textBoxWindow != null) textBoxWindow.SetActive(active);

        if (active == false) // reset to default text object
        {
            dialogueText = dialogueTextInBox;
            if (continueButton != null)
            {
                continueButton.enabled = false;
                continueButton.gameObject.SetActive(false);
            }
            if (continueTimelineButton != null)
            {
                continueTimelineButton.enabled = false;
                continueTimelineButton.gameObject.SetActive(false);
            }
        }
    }

    public void UseGeneralTextbox(string textToDisplay, string charName = null) // implement check, if not interactable, to hide it again
    {
        dialogueText.text = textToDisplay;
        if (charName == null) characterName.text = "Faye"; // default
        else characterName.text = charName;
        OpenTextBox(true);
    }

    public void UseGeneralTextboxMultipleTimes(string[] textToDisplay, string charName = null)
    {
        sentencesToDisplay = new Queue<string>();
        if (charName == null) characterName.text = "Faye"; // default
        else characterName.text = charName;

        foreach (string text in textToDisplay)
        {
            sentencesToDisplay.Enqueue(text);
        }
        NextText();
        OpenTextBox(true);
        continueButton.gameObject.SetActive(true);
        continueButton.enabled = true;
    }

    public void NextText() //ContinueButton
    {
        if (sentencesToDisplay == null) return;

        if (sentencesToDisplay.Count == 0)
        {
            sentencesToDisplay.Clear();
            OpenTextBox(false);
            return;
        }

        string sentence = sentencesToDisplay.Dequeue();
        dialogueText.text = sentence;
    }
}
