using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public static SoundManager instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource;


    //SoundManager always exists once and if more than 1, 1 gets deleted
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Plays sound from the Audioclip dragged in the inspector of an object
    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    //Master volume value for the Settings slider
    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }




}
