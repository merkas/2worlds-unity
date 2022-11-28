using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcChatter : MonoBehaviour //needs own tag for player as trigger
{
    int chosenConversation;
    int chosenComment;
    public bool automatic;

    public List<Conversation> conversations;
    Conversation previousConversation;
    public bool conversationEnd;

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

    public void ChooseConversation()
    {
        if (conversations != null) // check if conversations left
        {
            int max = conversations.Count;
            
            if (max > 1) chosenConversation = Random.Range(0, max); // check if more than one conversation left
            else chosenConversation = max - 1;

            if (previousConversation != conversations[chosenConversation])
            {
                StartCoroutine(ShowNpcConversation());
            }
            else
            {
                ChooseConversation();
            }
        }
    }

    void ShowComment()
    {
        if (bubbleTurn == false) bubble = Instantiate(speechBubble, canvas.transform.position, Quaternion.identity);
        else bubble = Instantiate(speechBubble, canvas2.transform.position, Quaternion.identity);
        bubble.transform.localScale += new Vector3(canvas.transform.localScale.x * 0.8f, canvas.transform.localScale.y * 0.8f, 0);

        if (bubbleTurn == false)
        {
            chatterText.text = conversations[chosenConversation].conversation[chosenComment];
            chatterText.enabled = true;
        } 
        else 
        {
            chatterText2.text = conversations[chosenConversation].conversation[chosenComment];
            chatterText2.enabled = true;
        }
        speechBubble.SetActive(true);
    }

    public void HideComment()
    {
        Destroy(bubble);
        chatterText.text = "";
        chatterText2.text = "";
        chatterText.enabled = false;
        chatterText2.enabled = false;
    }

    IEnumerator ShowNpcConversation()
    {
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
        //previousConversation = conversations[chosenConversation];
        //conversations.Remove(previousConversation); // save previousConversation on scene leave
        conversations.Remove(conversations[chosenConversation]);
        if (conversations.Count == 0) conversations = null;
        //conversationEnd = true;
    }
}
