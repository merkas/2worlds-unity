using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using UnityEngine.Timeline;

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

    public List<GameObject> externReferences;

    public List<PlayableOutput> bindings = new List<PlayableOutput>();
    public List<ExternTimelineReference> externBindings;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameManager.instance.SendDataToDirector(director);

        //Debug.Log(director.playableGraph.GetOutputCount());
        
        for (int i = 0; i < director.playableGraph.GetOutputCount(); i++)
        {
            bindings.Add(director.playableGraph.GetOutput(i));
            //Debug.Log(bindings[i].GetUserData());
            if (bindings[i].GetUserData() == null) AddExternalBinding(i);
        }

    }

    void AddExternalBinding(int count)
    {
        foreach (ExternTimelineReference externBinding in externBindings) // get refrence of extern binding object
        {
            if (externBinding.bindingIndex == count) // check for bindings index in refrence list
            {
                foreach (GameObject externObject in externReferences) // get object in external objects list
                {
                    if (externBinding.referenceName == externObject.name)
                    {
                        bindings[count].SetUserData(externObject);
                        
                        //Debug.Log(bindings[count].GetUserData());
                    }
                }
            }
        }
    }
}
