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

    public GameObject transitionScreen;
    Animator transitionAnimator;
    float transitionTime = 1f;

    public GameObject player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        transitionAnimator = transitionScreen.GetComponent<Animator>();
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
        
        if (activeDirector != null)
        {
            activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            dialogueMoment = true;
        }
        else UIManager.instance.OpenTextBox(false);
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

    public void LoadNewScene(int index)
    {
        StartCoroutine(LoadNewLevel(index));
    }

    IEnumerator LoadNewLevel(int index)
    {
        transitionAnimator.SetTrigger("StartSceneTransition");
        yield return new WaitForSeconds(transitionTime);
        // instead load into base scene and unload old scene
        SceneManager.LoadScene(index);
    }
}
