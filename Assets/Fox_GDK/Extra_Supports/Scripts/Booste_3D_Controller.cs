using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booste_3D_Controller : BaseGameBehaviour
{
    public List<ParticleSystem> particles;
    public Transform vfx;
    public Vector3 hammerOffset = Vector3.zero;
    public Vector3 magnetOffset = Vector3.zero;

    List<Vector3> particleInitialPos = new();
    List<Vector3> particleInitialRot = new();
    List<Vector3> particleInitialScale = new();

    Booster_Item currentActiveBooster;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < particles.Count; i++)
        {
            particleInitialPos.Add(particles[i].transform.localPosition);
            particleInitialRot.Add(particles[i].transform.localEulerAngles);
            particleInitialScale.Add(particles[i].transform.localScale);
        }
        vfx.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnBoosterPurchased(Booster_Item booster_Data)
    {
        base.OnBoosterPurchased(booster_Data);

        Initialize(booster_Data);
        switch (booster_Data.type)
        {
            case Booster_Type.HAMMER:

                gameplayData.Is_Hammer_Tap_Mood = true;
                SaveGame();

                break;
            case Booster_Type.MAGNET:
            //gameplayData.Is_Magnet_Tap_Mood = true;
            //SaveGame();
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                StartCoroutine(Execute_Booster_After_Animation(transform));
                break;
        }
    }

    public override void OnBoosterImplemented(Booster_Item booster_Data)
    {
        base.OnBoosterImplemented(booster_Data);

        currentActiveBooster = null;
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public IEnumerator Execute_Booster_After_Animation(Transform target, Action Oncomplete = null)
    {
        yield return null;
        gameManager.BoosterPreImplemented(currentActiveBooster);

        Vector3 pos = Vector3.zero;
        transform.position = new Vector3(30, 60, -60);
        switch (currentActiveBooster.type)
        {
            case Booster_Type.HAMMER:

                vfx.gameObject.SetActive(true);
                pos = target.position;
                vfx.eulerAngles = new Vector3(65, 0, -15);

                transform.DOMove(pos.FOXE_ModifyThisVector(hammerOffset), ConstantManager.ONE_HALF_TIME).OnComplete(() =>
                {
                    vfx.DOLocalRotate(new Vector3(0, 35, 0), ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                    {
                        vfx.DOLocalRotate(new Vector3(90, 0, 0), ConstantManager.ONE_FORTH_TIME / 2).OnComplete(() =>
                        {
                            particles[(int)currentActiveBooster.type].transform.parent = null;
                            particles[(int)currentActiveBooster.type].Play();

                            Oncomplete?.Invoke();
                            vfx.DOLocalRotate(new Vector3(45, 10, 0), ConstantManager.ONE_FORTH_TIME / 2).OnComplete(() =>
                            {
                                vfx.DOScale(Vector3.zero, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                                {
                                    gameplayData.Is_Hammer_Tap_Mood = false;
                                    OnFinishingBoosterEffect();
                                }).SetEase(Ease.InSine);
                            }).SetEase(Ease.OutSine);

                        }).SetEase(Ease.InSine);
                    }).SetEase(Ease.OutSine);
                });
                break;
            case Booster_Type.MAGNET:

                //vfx.gameObject.SetActive(true);
                //transform.position = pos.FOXE_ModifyThisVector(magnetOffset);
                //transform.localScale = Vector3.zero;
                //transform.DOScale(Vector3.one, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                //{
                //    particles[(int)currentActiveBooster.type].Play();

                //    FoxTools.functionManager.ExecuteAfterWaiting(ConstantManager.ONE_FORTH_TIME, () =>
                //    {
                //        particles[(int)currentActiveBooster.type].Stop();
                //        Oncomplete?.Invoke();
                //        FoxTools.functionManager.ExecuteAfterWaiting(ConstantManager.ONE_FORTH_TIME, () =>
                //        {
                //            vfx.DOLocalRotate(new Vector3(45, 10, 0), ConstantManager.ONE_FORTH_TIME / 2).OnComplete(() =>
                //            {
                //                vfx.DOScale(Vector3.zero, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                //                {
                //                    gameplayData.Is_Magnet_Tap_Mood = false;
                //                    OnFinishingBoosterEffect();
                //                }).SetEase(Ease.InSine);
                //            }).SetEase(Ease.OutSine);
                //        });


                //    });
                //});
                OnFinishingBoosterEffect();
                break;
            case Booster_Type.GOAL:
                OnFinishingBoosterEffect();
                break;
            case Booster_Type.DOCK:
                OnFinishingBoosterEffect();
                break;
        }
    }

    void OnFinishingBoosterEffect()
    {
        StartCoroutine(Finish_Booster_Acivity());
        IEnumerator Finish_Booster_Acivity()
        {
            gameplayData.Is_Game_Busy_By_Booster = false;
            vfx.gameObject.SetActive(false);
            SaveGame();
            yield return null;
            gameManager.BoosterImplemented(currentActiveBooster);
        }
    }

    public void Initialize(Booster_Item booster_Data)
    {
        currentActiveBooster = booster_Data;
        gameplayData.Is_Game_Busy_By_Booster = true;
        SaveGame();

        int index = (int)booster_Data.type;
        vfx.FOXE_ActiveChild(index);
        particles[index].transform.SetParent(vfx.GetChild(index));
        particles[index].transform.localPosition = particleInitialPos[index];
        particles[index].transform.localEulerAngles = particleInitialRot[index];
        particles[index].transform.localScale = particleInitialScale[index];
        vfx.localScale = Vector3.one;
        vfx.eulerAngles = Vector3.zero;
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
