using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleSceneData: MonoBehaviour
{
    public int sceneEnters;
    int sceneProgress;
    int storyProgress;

    public Scene thisScene;

    public List<ObjectChange> objectsToChange;
    

    private void Awake()
    {
        thisScene = SceneManager.GetActiveScene();
        SceneDataSave.instance.sceneData.Add(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public void ChangeSceneButton() // for testing
    {
        SceneManager.LoadScene("DialogueTest");
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneDataSave.instance.activeScene == thisScene)
            sceneEnters++;
    }

    void CheckSceneChanges()
    {

    }

    void OnSceneProgress()
    {
        foreach (ObjectChange obj in objectsToChange)
        {
            if (obj.objectToChange == null) // check for empty SO refrence
            {
                Debug.Log("Found ListObject without GameObject");
                objectsToChange.Remove(obj);
                return;
            }

            if (obj.sceneProgressMin >= sceneProgress && obj.NumberOfEntersMin >= sceneEnters) // check min conditions
            {
                if (obj.sceneProgressMax <= sceneProgress && obj.NumberOfEntersMax <= sceneEnters) // check max conditions
                {
                    // change object depending on its bools
                }
            }
        }
    }
}
