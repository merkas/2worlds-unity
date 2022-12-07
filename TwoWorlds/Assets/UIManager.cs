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
    TextMeshProUGUI dialogueText;

    PlayableDirector activeDirector;
    bool activeTimeline = false;
    public GameObject timeline;

    private void Start()
    {
        OpenTextBox(false);
        textBoxWindow.SetActive(false);
        //DontDestroyOnLoad(this);
    }

    public void TestTimeline()
    {
        timeline.GetComponent<PlayableDirector>().Play();
    }

    public void ChangeSceneButton()
    {
        SceneManager.LoadScene("TestSceneX");
    }

    public void SetDialogue(string dialogue, string charName, Sprite characterSprite = null)
    {
        if (characterSprite != null) charSprite.sprite = characterSprite;
        if (dialogueText != null ) dialogueText.text = dialogue;

        characterName.text = charName;
        OpenTextBox(true);
    }

    public void GetBinding(TextMeshProUGUI text) // dialogue tracks
    {
        dialogueText = text;
    }

    public void OpenTextBox(bool active)
    {
        if (textBoxWindow != null) textBoxWindow.SetActive(active);
    }

    public void UseGeneralTextbox(string textToDisplay, string charName = null)
    {
        dialogueText.text = textToDisplay;
        if (charName == null) characterName.text = "Faye"; // default
        else characterName.text = charName;
        OpenTextBox(true);
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText) // not working
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // = wait for next frame
        }
    }
}
