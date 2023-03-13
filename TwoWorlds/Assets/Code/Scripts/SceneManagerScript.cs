using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public string activeScene;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject overlay;

    bool pauseGameMenu;
    //public Button Back;
    //public Button Resume;
    //public Button Settings;
    //public Button MainMenu;
    //public Slider SoundSlider;
    //public Text Sound;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive)
        {
            activeScene = SceneManager.GetActiveScene().name;
        }
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        overlay.SetActive(false);
        pauseGameMenu = false;
        
        //Back.gameObject.SetActive(false);
        //Resume.gameObject.SetActive(false);
        //Settings.gameObject.SetActive(false);
        //MainMenu.gameObject.SetActive(false);
        //Sound.gameObject.SetActive(false);
        //SoundSlider.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
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

    public void EscPress() // instead in gameManager to manage input in same script? and Pause Menu won't be an extra scene anymore
    {
        //Scene scene = SceneManager.GetActiveScene();
        //if (scene.name == activeScene)
        //{

            if (pauseGameMenu == false)
            {
                Time.timeScale = 0;
                pauseGameMenu = true;
                pauseMenu.SetActive(true);
                overlay.SetActive(true);
                //Resume.gameObject.SetActive(true);
                //Settings.gameObject.SetActive(true);
                //MainMenu.gameObject.SetActive(true);
                //SoundSlider.gameObject.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(false);
                settingsMenu.SetActive(false);
                overlay.SetActive(false);
                pauseGameMenu = false;
                Time.timeScale = 1;
            }
        //}
    }

    public void PauseGame()
    {
        pauseGameMenu = true;
        overlay.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        //Resume.gameObject.SetActive(true);
        //Settings.gameObject.SetActive(true);
        //MainMenu.gameObject.SetActive(true);
        //SoundSlider.gameObject.SetActive(false);
        //Back.gameObject.SetActive(false);
        //Sound.gameObject.SetActive(false);
    }

    public void ContinueGame()
    {
        overlay.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseGameMenu = false;
        Time.timeScale = 1;
    }
     
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void OpenGameSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        //Resume.gameObject.SetActive(false);
        //Settings.gameObject.SetActive(false);
        //MainMenu.gameObject.SetActive(false);
        //Back.gameObject.SetActive(true);
        //Sound.gameObject.SetActive(true);
        //SoundSlider.gameObject.SetActive(true);
    }

    public void OpenStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
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
