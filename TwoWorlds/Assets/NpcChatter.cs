using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcChatter : MonoBehaviour //needs own tag for player as trigger
{
    public List<string> comments;
    string previousComment;
    int chosenComment;
    int chosenConversation;

    public List<Conversation> conversations;
    bool conversationUsed = false;
    Conversation previousConversation;

    public Transform bubbleSpawn;
    public GameObject speechBubble; //Prefab

    public GameObject canvas;
    public GameObject canvas2;

    GameObject bubble;
    bool bubbleTurn = false;

    TextMeshProUGUI chatterText;
    TextMeshProUGUI chatterText2;

    private void Start()
    {
        chatterText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        chatterText2 = canvas2.GetComponentInChildren<TextMeshProUGUI>();

        chatterText.enabled = false;
        chatterText2.enabled = false;
    }

    public void ChooseComment()
    {
        if (conversations.Count < 1 || conversationUsed == true)
        {
            int max = comments.Count - 1;
            chosenComment = Random.Range(0, max);

            if (comments[chosenComment] != previousComment)
                ShowComment();
            //else ChooseComment(); // geht das? oder stattdessen while-loop?
        }
        else // npc conversation
        {
            int max = conversations.Count - 1;
            chosenConversation = Random.Range(0, max);
            // check if already used chosenConversation
            if (conversationUsed == false) StartCoroutine(ShowNpcConversation());
        }
        
    }

    void ShowComment()
    {
        if (bubbleTurn == false) bubble = Instantiate(speechBubble, canvas.transform.position, Quaternion.identity);
        else bubble = Instantiate(speechBubble, canvas2.transform.position, Quaternion.identity);
        bubble.transform.localScale += new Vector3(canvas.transform.localScale.x * 0.8f, canvas.transform.localScale.y * 0.8f, 0);

        

        if (bubbleTurn == false)
        {
            if (conversations.Count < 1) chatterText.text = comments[chosenComment];
            else chatterText.text = conversations[chosenConversation].conversation[chosenComment];
            chatterText.enabled = true;
        } 
        else 
        {
            if (conversations.Count < 1) chatterText2.text = comments[chosenComment];
            else chatterText2.text = conversations[chosenConversation].conversation[chosenComment];
            chatterText2.enabled = true;
        }
        speechBubble.SetActive(true);
    }

    public void HideComment()
    {
        previousComment = comments[chosenComment];
        Destroy(bubble);
        chatterText.text = "";
        chatterText2.text = "";
        chatterText.enabled = false;
        chatterText2.enabled = false;
    }

    IEnumerator ShowNpcConversation()
    {
        //conversationUsed = true;
        int selection = 0;
        foreach(string text in conversations[chosenConversation].conversation)
        {
            chosenComment = selection;
            ShowComment();
            yield return new WaitForSeconds(2f);
            HideComment();
            selection++;
            if (bubbleTurn == false) bubbleTurn = true;
            else bubbleTurn = false;
        }
        conversations[chosenConversation] = previousConversation;
    }
}
