using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataSave : MonoBehaviour
{
    #region Singleton
    public static SceneDataSave instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            //return;
        }
        else instance = this;
    }
    #endregion

    public List<SingleSceneData> sceneData;

    public int storyProgress = 0;

    public Scene activeScene;
    public SingleSceneData activeSceneData;
    public List<ObjectChange> objectsToCheck;

    public List<NpcData> currentNpcs;
    public List<NpcData> allNpcs; // just for check public

    bool newSceneAdded = false;

    private void Start()
    {
        // load default values / save file   
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode) // called after Awake of DataContainer
    {
        newSceneAdded = false;
        activeScene = SceneManager.GetActiveScene();
        bool foundSceneInList = false;

        foreach (SingleSceneData data in sceneData)
        {
            if (data.sceneName == activeSceneData.sceneOfThisData.name)
            {
                foundSceneInList = true;
                activeSceneData = data;
                break;
            }
        }
        
        if (foundSceneInList == true)
        {
            activeSceneData.sceneEnters++;
        }
        else
        {
            AddSceneDataToList(activeSceneData); 
        }

        //CheckForSceneProgress(GameManager.instance.storyProgress);
        LoadCurrentScene();
    }

    public void LoadNpcData(NpcData data, GameObject npc)
    {
        if (newSceneAdded == true)
        {
            currentNpcs.Add(data);
            allNpcs.Add(data);
        }
        else
        {
            foreach(NpcData npcData in allNpcs)
            {
                if (npcData.npcName == data.npcName) // since name stays always the same
                {
                    npc.GetComponent<DialoguePartner>().UpdateData(npcData);
                    currentNpcs.Add(npcData); // current npcData takes saved data as well
                }
            }
        }
    }

    public void SaveNewNpcData(NpcData data) // called on scene leave
    {
        int index = 0;
        foreach(NpcData npcData in allNpcs)
        {
            if (npcData.npcName == data.npcName)
            {
                Debug.Log("Found matching data, updating data");
                allNpcs[index].numberOfTalks = data.numberOfTalks; // check if strange numbers appear in inspector

                allNpcs[index].npcWithMenu = data.npcWithMenu;
                allNpcs[index].getItem = data.getItem;
                break;
            }
            index++;
        }
    }

    public void OnSceneExit() // called by dataContainer
    {
        newSceneAdded = false;
    }

    public void AddSceneDataToList(SingleSceneData activeSceneData)
    {
        // check for unwanted scenes like main menu, credits and fight scene
        newSceneAdded = true;
        sceneData.Add(activeSceneData);
    }

    void CheckForSceneProgress(int storyProgress)
    {
        int index = 0;
        foreach(int checkpoint in activeSceneData.progressPoint)
        {
            if (storyProgress == checkpoint)
            {
                //int progress = 1;
                // scene Progress depending on last checkpoint activated

                //activeSceneData.sceneProgress += progress;
                activeSceneData.sceneProgress = index; // check if working
            }
            index++;
        }
    }

    void SceneData(int corruption, int numberOfEnters, int sceneProgress) // give data to scene
    {

    }

    void LoadCurrentScene() // has to be checked
    {
        //int corruption = GameManager.instance.corruption;
        foreach (ObjectChange obj in objectsToCheck)
        {
            if (obj.objectToChange == null) // check for empty refrence
            {
                Debug.Log("Found ListObject without GameObject");
                objectsToCheck.Remove(obj);
                return;
            }
            
            if (obj.sceneProgressMin <= activeSceneData.sceneProgress && obj.sceneEntersMin <= activeSceneData.sceneEnters) // check min conditions
            {
                if (obj.sceneProgressMax >= activeSceneData.sceneProgress && obj.sceneEntersMax >= activeSceneData.sceneEnters
                   /* && obj.corruptionMin <= corruption && obj.corruptionMax >= corruption*/) // check max conditions, implement, when corruption stat script stands
                {
                    ChangeObjectWhenConditionsMet(obj);
                }
            }
        }
    }

    void ChangeObjectWhenConditionsMet(ObjectChange obj) // change object depending on its bools
    {
        if (obj.doThis == DoThis.TRIGGER)
        {
            if (obj.activateTrigger == true) obj.objectToChange.GetComponent<Collider2D>().isTrigger = true;
            else obj.objectToChange.GetComponent<Collider2D>().isTrigger = false;
        }

        else if (obj.doThis == DoThis.SETACTIVE)
        {
            if (obj.setActive == true) obj.objectToChange.SetActive(true);
            else obj.objectToChange.SetActive(false);
        }

        else if (obj.doThis == DoThis.OTHER)
        {
            if (obj.changeSprite == true) // only one-time use for now
            {
                obj.objectToChange.GetComponent<SpriteRenderer>().sprite = obj.changeToThisSprite;
            }
            else if (obj.instantiate == true)
            {
                Instantiate(obj.objectToChange, obj.spawnPoint.position, Quaternion.identity);
            }
            else if (obj.destroy == true)
            {
                Destroy(obj.objectToChange);
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
