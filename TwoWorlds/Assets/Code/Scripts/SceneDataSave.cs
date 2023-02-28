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

    Scene activeScene;
    
    public SingleSceneData activeSceneData;
    public List<ObjectChange> objectsToCheck;

    public List<NpcData> currentNpcs;
    public List<NpcData> allNpcs; // just for check public

    public GameObject player;

    bool newSceneAdded = false;
    //GameObject currentDataContainer;

    private void Start()
    {
        // load default values / save file   
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    public void GetCurrentDataContainer(GameObject container, Scene scene, LoadSceneMode mode) // called by data container
    {
        activeSceneData = container.GetComponent<DataContainer>().thisSceneData;
        objectsToCheck = container.GetComponent<DataContainer>().objectsToChange;
        //currentDataContainer = container;
        SceneLoaded(scene, mode);
        if (container.GetComponent<DataContainer>().spawnPoint.Length > 1)
        {
            player.transform.position = container.GetComponent<DataContainer>().spawnPoint[GameManager.instance.spawnPointIndex].transform.position;
        }
        else player.transform.position = container.GetComponent<DataContainer>().spawnPoint[0].transform.position;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        newSceneAdded = false;
        activeScene = scene;

        bool foundSceneInList = false;

        foreach (SingleSceneData data in sceneData)
        {
            if (data.sceneName == activeScene.name)
            {
                foundSceneInList = true;
                
                data.sceneEnters++;
                activeSceneData = data;
                
                break;
            }
        }

        if (foundSceneInList != true)
        {
            activeSceneData.sceneName = activeScene.name;
            AddSceneDataToList(activeSceneData);
        }

        //CheckForSceneProgress(GameManager.instance.storyProgress);
        LoadCurrentScene();   
    }

    void SceneUnloaded(Scene currentScene)
    {
        activeSceneData = null;
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

    public void SaveNewNpcData(NpcData data) // called on scene leave, check if working
    {
        int index = 0;
        foreach(NpcData npcData in allNpcs)
        {
            if (npcData.npcName == data.npcName)
            {
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
        // implement check for unwanted scenes like main menu, credits and fight scene - unnecessary, since they won't have data containers? check
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

            if (obj.sceneProgressMin > activeSceneData.sceneProgress || obj.sceneProgressMax < activeSceneData.sceneProgress) return;
            if (obj.sceneEntersMin > activeSceneData.sceneEnters || obj.sceneEntersMax < activeSceneData.sceneEnters) return;
            //if (obj.corruptionMin > corruption && obj.corruptionMax < corruption) return; // implement, when corruption stat script stands

            ChangeObjectWhenConditionsMet(obj);
        }
    }

    void ChangeObjectWhenConditionsMet(ObjectChange obj) // change object depending on its bools
    {
        if (obj.doThis == DoThis.TRIGGER) // collider is enabled or disabled
        {
            if (obj.activateTrigger == true) obj.objectToChange.GetComponent<Collider2D>().enabled = true;
            else obj.objectToChange.GetComponent<Collider2D>().enabled = false;
        }
        else if (obj.doThis == DoThis.SETACTIVE)
        {
            if (obj.setActive == true) obj.objectToChange.SetActive(true);
            else obj.objectToChange.SetActive(false);
        }
        else if (obj.doThis == DoThis.OTHER)
        {
            if (obj.changeSprite == true)
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
        SceneManager.sceneUnloaded -= SceneUnloaded;
    }
}
