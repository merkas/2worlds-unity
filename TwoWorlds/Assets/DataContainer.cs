using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataContainer : MonoBehaviour
{
    public SingleSceneData thisSceneData;

    public List<ObjectChange> objectsToChange;

    private void Awake()
    {
        thisSceneData.sceneOfThisData = SceneManager.GetActiveScene();
        thisSceneData.sceneName = SceneManager.GetActiveScene().name; 
    }

    private void Start()
    {
        SceneDataSave.instance.activeSceneData = thisSceneData;
        SceneDataSave.instance.objectsToCheck = objectsToChange;
    }

    private void OnDestroy()
    {
        // before destroyed, save new data to scene data save?
        //SceneManager.sceneLoaded -= SceneLoaded;
        SceneDataSave.instance.OnSceneExit();
        SceneDataSave.instance.currentNpcs.Clear(); // positioned here and not in npcPartner, so it gets called only once per scene
    }
}
