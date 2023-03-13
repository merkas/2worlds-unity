using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcComment : MonoBehaviour
{
    public List<string> comments;
    string previousComment;
    int chosenComment;

    public bool automatic;
    public bool playerReaction;
    public string characterDialogue;
    public string characterName;

    //public Transform bubbleSpawn;
    public GameObject speechBubble; //Prefab
    float timer;
    bool commentOnShow;
    public GameObject canvas;

    GameObject bubble;

    TextMeshProUGUI chatterText;

    private void Start()
    {
        chatterText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        chatterText.enabled = false;
    }

    public void ChooseComment()
    {
        int max = comments.Count;
        if (max > 1)
        {
            chosenComment = Random.Range(0, max);
            if (comments[chosenComment] != previousComment)
                ShowComment();
            else ChooseComment(); // is working
        }
        else
        {
            chosenComment = 0;
            ShowComment();
            GetComponent<BoxCollider2D>().enabled = false; // actual collider as child object since it's not always needed?
        }
        
    }

    void ShowComment()
    {
        bubble = Instantiate(speechBubble, canvas.transform);
        bubble.transform.localScale -= new Vector3(chatterText.transform.localScale.x * .5f, chatterText.transform.localScale.y * .5f, 0);
        bubble.transform.position = canvas.transform.position;
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
            }
        }
    }

    public void HideComment()
    {
        previousComment = comments[chosenComment];

        Destroy(bubble);
        chatterText.text = "";
        chatterText.enabled = false;
        commentOnShow = false;
        timer = 0;

        if (playerReaction == true && characterDialogue != null)
        {
            UIManager.instance.UseGeneralTextbox(characterDialogue, characterName);
        }
    }
}
