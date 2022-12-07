using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Gameplay, // player can move around
    FightMode,
    Cutscene // player can't move around, space to continue
}

public class GameManager : MonoBehaviour
{
    GameMode gameMode;

    PlayableDirector activeDirector;
    bool dialogueMoment;

    #region Singleton
    public static GameManager instance;

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

    public int storyProgress = 0;
    //public int corruption;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    private void Update() // only for checking
    {
        if (dialogueMoment == true && Input.GetKey(KeyCode.Space))
        {
            ResumeTimeline();
        }
    }

    public void PauseTimeline(PlayableDirector activeOne)
    {
        activeDirector = activeOne;
        activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        dialogueMoment = true;
    }

    public void ResumeTimeline()
    {
        activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        dialogueMoment = false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
