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

    public List<Scene> ingameScenes;

    public List<SingleSceneData> sceneData;

    //int sceneEnters;
    //int sceneProgress;

    static int storyProgress;

    public Scene activeScene;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        activeScene = SceneManager.GetActiveScene();
    }

    public void Debugger(string message)
    {
        Debug.Log(message);
    }

    static void GetStoryProgress()
    {

    }

    void SaveData()
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
