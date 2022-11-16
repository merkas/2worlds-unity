using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcChatter : MonoBehaviour //needs own tag for player as trigger
{
    public List<string> comments;
    string previousComment;
    int chosenComment;

    public Transform bubbleSpawn;
    public GameObject speechBubble;
    public GameObject canvas;
    GameObject bubble;
    //public Text commentText;

    public TextMeshProUGUI chatterText;

    private void Start()
    {
        chatterText.enabled = false;
    }

    public void ChooseComment()
    {
        int max = comments.Count - 1;
        chosenComment = Random.Range(0, max);

        if (comments[chosenComment] != previousComment)
            ShowComment();
        //else ChooseComment(); // geht das? oder stattdessen while-loop?
    }

    void ShowComment()
    {
        bubble = Instantiate(speechBubble, canvas.transform.position, Quaternion.identity);

        bubble.transform.localScale += new Vector3(canvas.transform.localScale.x * 0.8f, canvas.transform.localScale.y * 0.8f, 0);

        chatterText.text = comments[chosenComment];
        chatterText.enabled = true;
        speechBubble.SetActive(true);
    }

    public void HideComment()
    {
        previousComment = comments[chosenComment];
        Destroy(bubble);
        chatterText.text = "";
    }

    IEnumerator ShowNpcConversation()
    {
        yield return new WaitForSeconds(1);
        
    }
}
