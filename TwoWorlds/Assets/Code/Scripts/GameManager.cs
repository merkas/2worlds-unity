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
    string activeScene;
    PlayableDirector activeDirector;
    bool dialogueMoment;

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
            Debug.Log("Active scene: " + activeScene);
        }
        //if (scene.name != "BaseScene" && mode != LoadSceneMode.Additive) // not working
        //{
        //    SceneManager.LoadSceneAsync("BaseScene");
        //    Debug.Log("Base Scene had to be loaded in");
        //}
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
        
        //SceneManager.LoadScene(index);
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(activeScene);

        transitionAnimator.SetTrigger("SceneEnter");
    }

    public void SendDataToDirector(PlayableDirector director)
    {
        director.GetComponent<GetTimelineDefaultBinding>().externReferences = timelineReferences;
    }
}
