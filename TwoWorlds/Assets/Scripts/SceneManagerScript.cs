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
        //Current Scene Log
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

    //Lädt Spielscene
    public void Start2Game()
    {
        SceneManager.LoadScene("SampleScene");
    }
    //Lädt Startmenü
    public void OpenStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    //Lädt Settingsmenü
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    //Schließt Spiel
    public void Exit200()
    {
        Debug.Log("Exit");
        Application.Quit();
    }


#if UNITY_EDITOR

    //Für Editor only, da Esc evtl. Game Fenster schließt
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
