using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string activeScene;
    public string GameSettingsMenu;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        Debug.Log("SceneManager subscribed event");
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive)
        {
            activeScene = SceneManager.GetActiveScene().name;
            Debug.Log(activeScene);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    void Awake()
    {
        Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
    }

  
    void Update()
    {   
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EscPress();
            }
    }

    public void StartGame() 
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("BaseScene"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level1A", LoadSceneMode.Additive));
    }

    public void EscPress()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == activeScene)
        {
            SceneManager.LoadScene("PauseMenu");
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(activeScene);
    }
     
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public void OpenGameSettings()
    {
        SceneManager.LoadScene("GameSettingsMenu");
    }

    public void OpenStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void OpenPauseMenu()
    {
        SceneManager.LoadScene("PauseMenu");
    }
    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }




    //Whitebox

    public void OpenCombat()
    {
        SceneManager.LoadScene("CombatTest");
    }
}
