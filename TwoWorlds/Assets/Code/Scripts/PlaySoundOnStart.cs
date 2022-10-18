using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(_clip);
    }

    //Script auf Objekte ziehen und diesen ihren AudioClip zuweisen.

}
