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

    public GameObject dialogueBox;
    public string timelineDialogue;
    public Image charSprite;
    public TextMeshProUGUI characterName;
    TextMeshProUGUI dialogueText;

    PlayableDirector activeDirector;
    bool activeTimeline = false;
    public GameObject timeline;

    private void Start()
    {
        OpenTimelineDialogueBox(false);
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
        OpenTimelineDialogueBox(true);
    }

    public void GetBinding(TextMeshProUGUI text) // dialogue tracks
    {
        dialogueText = text;
    }

    public void OpenTimelineDialogueBox(bool active)
    {
        dialogueBox.SetActive(active);
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText) // not working
    {
        Debug.Log("Type Sentence started");
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // = wait for next frame
        }
    }
}
