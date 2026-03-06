using DG.Tweening;
using System;
using System.Collections.Generic;
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
    public List<ZombieAudioData> zombieAudioDatas = new List<ZombieAudioData>();

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
        for (int i = 0; i < zombieAudioDatas.Count; i++)
        {
            zombieAudioDatas[i].audioSource.Stop();
        }
    }

    #region ALL OVERRIDING FUNCTIONS

    public void OnWaveStart(GameManager gameManager)
    {
        if (volume != 1)
            return;

        DOVirtual.Float(volume, 0.075f, ConstantManager.DEFAULT_ANIMATION_TIME, (volume) =>
        {
            startingBGMusic.volume = volume;
        });
        Play_Zombie_Sound(gameManager);

        if (!zombieAudioDatas[0].audioSource.isPlaying)
            zombieAudioDatas[0].audioSource.Play();
    }

    public void OnEnemyDied(GameManager gameManager)
    {
        Play_Zombie_Sound(gameManager);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    void Play_Zombie_Sound(GameManager gameManager)
    {
        for (int i = 0; i < zombieAudioDatas.Count; i++)
        {
            //if (gameManager.currentRemainningEnemies.Count >= zombieAudioDatas[i].playOnZombieCount)
            //{
            //    if (!zombieAudioDatas[i].audioSource.isPlaying)
            //        zombieAudioDatas[i].audioSource.Play();
            //}
            //else
            //{
            //    if (zombieAudioDatas[i].audioSource.isPlaying)
            //        zombieAudioDatas[i].audioSource.Stop();
            //}
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
