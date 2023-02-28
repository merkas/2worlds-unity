using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Gameplay, // player can move around
    FightMode,
    Cutscene // player can't move around
}

public class GameManager : MonoBehaviour
{
    GameMode gameMode;
    string activeScene;
    PlayableDirector activeDirector;
    bool dialogueMoment;
    public GameObject playerCam;
    public List<GameObject> timelineReferences;

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
    public int spawnPointIndex;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Start()
    {
        //DontDestroyOnLoad(this);
        transitionAnimator = transitionScreen.GetComponent<Animator>();
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive)
        {
            activeScene = scene.name;
        }
    }

    private void Update() // implement in an input manager instead?
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueMoment == true) ResumeTimeline();
            else UIManager.instance.NextText();
        }
        //if (dialogueMoment == true && Input.GetKeyDown(KeyCode.Space))
        //{
        //    ResumeTimeline();
        //}
        //else if (dialogueMoment == false && Input.GetKeyDown(KeyCode.Space))
        //{
        //    UIManager.instance.NextText();
        //}
    }

    public void PauseTimeline(PlayableDirector activeOne)
    {
        activeDirector = activeOne;
        
        if (activeDirector != null)
        {
            activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            dialogueMoment = true;
            UIManager.instance.continueTimelineButton.enabled = true;
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

    public void LoadNewScene(int index, int spawnPointIndex)
    {
        StartCoroutine(LoadNewLevel(index, spawnPointIndex));
    }

    IEnumerator LoadNewLevel(int index, int spawnPointIndex)
    {
        transitionAnimator.SetTrigger("StartSceneTransition");
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        transitionAnimator.SetTrigger("SceneEnter");
    }

    public void SendDataToDirector(PlayableDirector director)
    {
        director.GetComponent<GetTimelineDefaultBinding>().externReferences = timelineReferences;
    }
}
