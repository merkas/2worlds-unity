using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoThis
{
    TRIGGER,
    SETACTIVE,
    OTHER
}

[System.Serializable]
public class ObjectChange
{
    public GameObject objectToChange;

    public DoThis doThis;

    public bool instantiate;
    public bool setActive;
    public bool destroy;
    public bool changeSprite;

    public bool activateTrigger;

    public Transform spawnPoint;
    public Sprite changeToThisSprite;

    // bool?
    public int sceneProgressMin;
    public int sceneProgressMax;

    // bool?
    public int sceneEntersMin;
    public int sceneEntersMax;

    // bool?
    public int corruptionMin;
    public int corruptionMax;
}
