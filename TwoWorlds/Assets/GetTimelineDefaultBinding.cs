using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum TrackType
{
       DIALOGUE,
       CONTINUEBUTTON,
       PLAYERANIMATION,
       OTHERANIMATION
}

public class GetTimelineDefaultBinding : MonoBehaviour
{
    PlayableDirector director;
    //List<Object> bindings;

    public List<TrackType> trackTypes; // only for bindings using external refrences

    public List<GameObject> externReferences;
    TimelineAsset timelineAsset;

    public List<GameObject> bindings;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameManager.instance.SendDataToDirector(director);

        timelineAsset = (TimelineAsset)director.playableAsset;

        //director.playableGraph.GetType
        //director.playableGraph.GetRootPlayable



        //bindings.Add(director.GetGenericBinding());
        //var timelineAsset = director.playableAsset as TimelineAsset;
        var trackList = timelineAsset.GetOutputTracks();

        var sceneBindings = timelineAsset.outputs; // gives only tracks on timeline, not bindings
        Debug.Log(sceneBindings);
        
        //director.SetGenericBinding()

        //foreach (PlayableBinding binding in director.GetGenericBinding)
        foreach (var binding in sceneBindings)
        {
            
        }

    }
}
