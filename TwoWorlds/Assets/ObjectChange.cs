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

    public int sceneProgressMin;
    public int sceneProgressMax;

    public int NumberOfEntersMin;
    public int NumberOfEntersMax;
}
