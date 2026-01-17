using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ZombieAudioData
{
    public AudioSource audioSource;
    public int playOnZombieCount;
}

public class BackgroundAudioController : MonoBehaviour
{
    public AudioSource startingBGMusic;

    float volume = 1;

    private void Awake()
    {
    }

    private void Start()
    {
        startingBGMusic.Play();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
    }

    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS
}
