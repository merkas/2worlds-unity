using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string SampleScene;
    public string GameSettingsMenu;

    // Start is called before the first frame update
    void Start()
    {

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

    public void EscPress()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == SampleScene)
        {
            SceneManager.LoadScene("PauseMenu");
        }
    }

    
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
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

}
