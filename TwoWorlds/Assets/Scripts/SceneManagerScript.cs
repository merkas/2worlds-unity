using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string SampleScene;


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

            if (Input.GetKeyDown(KeyCode.T))
            {
                AlphaOnePress();
            }
    }

    public void EscPress()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == SampleScene)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }


#if UNITY_EDITOR

    public void AlphaOnePress()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == SampleScene)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

#endif
}
