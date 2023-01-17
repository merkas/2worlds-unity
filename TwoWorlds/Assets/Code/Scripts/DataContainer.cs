using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataContainer : MonoBehaviour
{
    public SingleSceneData thisSceneData;

    public List<ObjectChange> objectsToChange;

    public GameObject[] spawnPoint;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode) // might not be called before SceneDataSave
    {
        if (mode == LoadSceneMode.Additive)
            SceneDataSave.instance.GetCurrentDataContainer(this.gameObject, scene, mode);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneDataSave.instance.OnSceneExit();
        SceneDataSave.instance.currentNpcs.Clear(); // positioned here and not in npcPartner, so it gets called only once per scene
    }
}
