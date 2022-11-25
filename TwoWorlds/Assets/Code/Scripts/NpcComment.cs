using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcComment : MonoBehaviour
{
    public List<string> comments;
    string previousComment;
    int chosenComment;

    public Transform bubbleSpawn;
    public GameObject speechBubble; //Prefab
    float timer;
    bool commentOnShow;
    public GameObject canvas;

    GameObject bubble;

    bool commentOn;

    TextMeshProUGUI chatterText;

    private void Start()
    {
        chatterText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        chatterText.enabled = false;
    }

    public void ChooseComment()
    {
        int max = comments.Count;
        chosenComment = Random.Range(0, max);
        commentOn = true;
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
        commentOnShow = true;
    }

    private void Update()
    {
        if (commentOnShow == true)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                HideComment();
                //commentOnShow = false;
            }
        }
    }

    public void HideComment()
    {
        if (commentOn == true)
        {
            previousComment = comments[chosenComment];
            commentOn = false;
        }
        Destroy(bubble);
        chatterText.text = "";
        chatterText.enabled = false;
        commentOnShow = false;
        timer = 0;
    }
}
