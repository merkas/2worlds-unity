using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.Timeline;

[System.Serializable]
public class DialogueClipBehaviour : PlayableBehaviour
{
    public string dialogueText;
    public Sprite characterSprite;
    public string characterName;
    PlayableDirector director;

    public bool pauseThis = true; // change later
    bool pauseScheduled;
    bool clipPlayed;

    TextMeshProUGUI textBinding;
    bool sendBinding;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector); // Klammern nötig?
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        
        if (!clipPlayed && info.weight > 0)
        {
            if (characterSprite != null) UIManager.instance.SetDialogue(/*bindingText,*/ dialogueText, characterName, characterSprite);
            else UIManager.instance.SetDialogue(/*bindingText,*/ dialogueText, characterName);

            if (Application.isPlaying) // what is meant with application?
            {
                if (pauseThis)
                {
                    pauseScheduled = true;
                }
            }
            clipPlayed = true;
        }
        textBinding = playerData as TextMeshProUGUI;
        if (sendBinding == true)
        {
            UIManager.instance.GetBinding(textBinding);
            sendBinding = false;
        }
        //text.text = dialogueText;
        //text.color = new Color(1, 1, 1, info.weight); // for fading in/out of text
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        sendBinding = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (pauseScheduled == true)
        {
            pauseScheduled = false;
            GameManager.instance.PauseTimeline(director);
        }
        else
        {
            UIManager.instance.OpenTextBox(false);
        }
        clipPlayed = false;
    }

}
