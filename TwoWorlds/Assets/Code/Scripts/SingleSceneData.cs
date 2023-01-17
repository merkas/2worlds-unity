using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SingleSceneData
{
    public int sceneEnters;
    public int sceneProgress;

    public Scene sceneOfThisData;
    public string sceneName;

    public int[] progressPoint; // storyProgress needed for sceneProgress to happen
}
