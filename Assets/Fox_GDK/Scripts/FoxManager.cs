using Com.FunFox.Utility;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;

[DefaultExecutionOrder(ConstantManager.FoxManagerOrder)]
public class FoxManager : MonoBehaviour, IHierarchyIcon
{
    public static Action<PurchaseEventArgs, Product_ID> OnInAppPurchsedDone;
    public static Action<Booster_Item> OnBoosterPurchased;
    public static Action<Booster_Item> OnBoosterCanceled;
    public static Action<Booster_Item> OnBoosterPreimplemented;
    public static Action<Booster_Item> OnBoosterImplemented;
    public static Action<Booster_Item> OnBoosterClickedWhileBusy;

    public static Action OnReviveGame;

    public static Action<FoxObject> OnAddFoxBehaviour;
    public static Action<GameState> OnChangeGameState;
    public static Action OnGameInitialize;
    public static Action OnGameStart;
    public static Action OnGameOver;
    public static Action OnCompleteTask;
    public static Action OnIncompleteTask;
    public static Action OnPauseGamePlay;
    public static Action OnUnPauseGamePlay;

    public static Action<KV_TappingType, Vector3> OnTap;
    public static Action<Vector3> OnDrag;
    public static Action<KV_SwippingType> OnSwip;
    public static Action<float> OnZoom;

    public GameplayData gameplayData = new GameplayData();
    public GameState gameState;
    public TextMeshProUGUI levelText;
    public Fox_Canvas_Animator allButtonsUI, gameStartingUI, gamePlayUI, gameSuccessUI, gameFaildUI, gameCustomUI;

    [Space]
    public float cam_Focus_Distance = 45;

    [Space]
    public CurrencySystem currencyMainSystem;
    public Popup_Controller retryPopupController;

    [Space]
    public AudioSource activityTapSound;
    public AudioSource ButtonTapSound;
    public AudioSource gameFailSound;
    public AudioSource gameSuccessSound;

    [Header("==================")]
    public bool debugModeOn;

    [ReadOnlyProperty] public EventSystem eventSystem;
    [ReadOnlyProperty] public Booster_Panel_Controller booster_Panel_Controller;
    [ReadOnlyProperty] public LevelContinueView levelContinueView;
    [ReadOnlyProperty] public Fox_ADs_Controller fox_AD_Controller;
    [ReadOnlyProperty] public Fox_Events_Controller fox_ADs_Controller;
    [ReadOnlyProperty] public PlayerController playerController;
    [ReadOnlyProperty] public GameOver_Confetti gameOver_Confetti;
    [ReadOnlyProperty] public Hard_Level_Visuals hardware_level_visuals;
    [ReadOnlyProperty] public FeaturesController featuresController;
    [ReadOnlyProperty] public Tutorial_View tutorial_View;
    [ReadOnlyProperty] public Mask_Manager maskManager;
    [ReadOnlyProperty] public ConstantManager constantManager;
    [ReadOnlyProperty] public bool centerInputDetector;

    [HideInInspector] public int totalGivenTask = 0, totalCompletedTask = 0, totalIncompleteTask = 0;
    [HideInInspector] public FoxTools FoxTools;

    GameState previousState;
    [HideInInspector] public AudioSource runtimeAudioSource;
    Dictionary<Transform, Camera_Focus_Data> focusList;
    bool focusAlreadyRunning;
    bool startedAlready;

    private void OnEnable()
    {
        OnAddFoxBehaviour += AddFoxBehaviour;
    }

    private void OnDisable()
    {
        OnAddFoxBehaviour -= AddFoxBehaviour;
    }

    public virtual void Awake()
    {
        FoxTools = FoxTools.Instance;
        FoxTools.Set_Scene_Manager(this);
        gameplayData = FoxTools.savefileManager.LoadGameData<GameplayData>();
        runtimeAudioSource = gameObject.AddComponent<AudioSource>();
        runtimeAudioSource.playOnAwake = false;
        constantManager = Resources.Load("Constant Manager") as ConstantManager;

        AudioListener.volume = gameplayData.soundEffectOn ? 1 : 0;

        if (levelText)
            levelText.text = "Lvl " + (gameplayData.currentLevelNumber + 1);
    }

    public virtual void Start()
    {
        Activate_EventSystem();
        if (debugModeOn)
        {
            Instantiate(Resources.Load<Debug_Level_Selector>("Debug_Level_Selector"), gameCustomUI.transform);
        }
    }

    void Activate_EventSystem()
    {
        List<EventSystem> eventSystems = new();
        eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None).ToList();
        foreach (EventSystem eventSystem in eventSystems)
        {
            eventSystem.gameObject.SetActive(false);
        }
        if (eventSystems.Count > 0)
        {
            eventSystem = eventSystems[0];
            eventSystem.gameObject.SetActive(true);
        }
        else
        {
            eventSystem = new GameObject().AddComponent<EventSystem>();
            eventSystem.name = "EventSystem";
        }
    }

    public void AddFoxBehaviour(FoxObject foxObject)
    {
        foxObject.foxManager = this;
        foxObject.gameState = gameState;
        foxObject.gameplayData = gameplayData;
    }

    public virtual void ProcessTapping(KV_TappingType tappingType, Vector3 tapOnWorldSpace)
    {
        OnTap?.Invoke(tappingType, tapOnWorldSpace);
    }
    public virtual void ProcessDragging(Vector3 dragAmount)
    {
        OnDrag?.Invoke(dragAmount);
    }
    public virtual void ProcessSwipping(KV_SwippingType swippingType)
    {
        OnSwip?.Invoke(swippingType);
    }
    public virtual void ProcessZooming(float zoom)
    {
        OnZoom?.Invoke(zoom);
    }

    public virtual void OnCompleteATask()
    {
        totalCompletedTask++;
        OnCompleteTask?.Invoke();

        if (totalCompletedTask.Equals(totalGivenTask))
        {
            gameplayData.gameoverSuccess = true;
            gameplayData.gameEndTime = Time.time;

            ChangeGameState(GameState.GAME_PLAY_ENDED);
        }
    }

    public virtual void OnIncompleteATask()
    {
        totalIncompleteTask++;
        OnIncompleteTask?.Invoke();
    }

    public virtual int GetModedLevelNumber()
    {
        int sceneLoopingStartIndex = constantManager.SCENE_LOOPING_STARTING_INDEX - 1;
        if (gameplayData.currentLevelNumber >= constantManager.TOTAL_GAME_LEVELS)
        {
            return sceneLoopingStartIndex + ((gameplayData.currentLevelNumber - sceneLoopingStartIndex) % (constantManager.TOTAL_GAME_LEVELS - sceneLoopingStartIndex));
        }
        else
        {
            return gameplayData.currentLevelNumber % constantManager.TOTAL_GAME_LEVELS;
        }
    }

    public int GetLevelMultiplayer()
    {
        return gameplayData.currentLevelNumber / constantManager.TOTAL_GAME_LEVELS;
    }

    public virtual void NextLevel()
    {
        FoxTools.sceneManager.LoadNextLevel();
    }

    public virtual void ReloadLevel()
    {
        FoxTools.sceneManager.ReLoadLevel();
    }

    public virtual void ChangeGameState(GameState gameState)
    {
        StartCoroutine(ChangeState(gameState));
    }

    IEnumerator ChangeState(GameState gameState)
    {
        yield return null;
        switch (gameState)
        {
            case GameState.NONE:
                break;
            case GameState.GAME_INITIALIZED:
                GameInitialize();
                break;
            case GameState.GAME_PLAY_STARTED:
                GameStart();
                break;
            case GameState.GAME_PLAY_ENDED:
                GameOver();
                break;
            case GameState.GAME_PLAY_PAUSED:
                PauseGamePlay();
                break;
            case GameState.GAME_PLAY_UNPAUSED:
                gameState = UnPauseGame(gameState);
                break;
        }

        Debug.Log("Executing: " + gameState.ToString());
        this.gameState = gameState;
        OnChangeGameState?.Invoke(gameState);
    }

    public virtual void GameInitialize()
    {
        OnGameInitialize?.Invoke();
    }

    public virtual void GameStart()
    {
        if (!startedAlready)
        {
            startedAlready = true;
            gameplayData.gameStartTime = Time.time;
        }

        OnGameStart?.Invoke();
    }

    public virtual void GameOver(bool value)
    {
        gameplayData.gameEndTime = Time.time;
        gameplayData.totalLevelCompletedTime = gameplayData.gameEndTime - gameplayData.gameStartTime;

        gameplayData.gameoverSuccess = value;
        ChangeGameState(GameState.GAME_PLAY_ENDED);
    }

    public virtual void GameOver()
    {
        gameplayData.gameEndTime = Time.time;
        gameplayData.totalLevelCompletedTime = gameplayData.gameEndTime - gameplayData.gameStartTime;

        if (gameplayData.gameoverSuccess)
        {
            gameplayData.currentLevelNumber++;
            if (gameOver_Confetti != null)
                gameOver_Confetti.Play_Particle();

            if (gameSuccessSound)
                gameSuccessSound.Play();

        }
        else
        {
            if (gameFailSound)
                gameFailSound.Play();
        }
        SaveGame();

        OnGameOver?.Invoke();
    }

    public virtual void PlayThisSoundEffect(AudioClip audioClip)
    {
        if (runtimeAudioSource.clip == null)
            runtimeAudioSource.clip = audioClip;

        runtimeAudioSource.Play();
    }

    public virtual void PlayThisSoundSource(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public virtual void SaveGame()
    {
        FoxTools.savefileManager.SaveGameData(gameplayData);
    }

    public virtual void PauseGamePlay()
    {
        previousState = gameState;
        OnPauseGamePlay?.Invoke();
    }

    public virtual GameState UnPauseGame(GameState gameState)
    {
        gameState = previousState;
        OnUnPauseGamePlay?.Invoke();
        return gameState;
    }

    #region COMMON_GAME_FUNCTIONALITIES

    public void Dragging_On()
    {
        centerInputDetector = true;
    }

    public void Dragging_Off()
    {
        centerInputDetector = false;
    }

    public virtual void OnDirectRetryYesButtonPress()
    {
        fox_ADs_Controller.OnDirectRetryYesButtonPress();
        ReloadLevel();
    }

    public virtual void ReviveGame()
    {
        OnReviveGame?.Invoke();
    }

    public virtual void BoosterClickedWhileBusy(Booster_Item booster_Data)
    {
        OnBoosterClickedWhileBusy?.Invoke(booster_Data);
    }

    public virtual void Cancel_Activated_Booster(Booster_Item booster_Item)
    {
        OnBoosterCanceled?.Invoke(booster_Item);
    }

    public virtual void TryPurchasedBooster(Booster_Item booster_Data)
    {
        if (gameplayData.Is_Game_Busy_By_Booster)
        {
            OnBoosterClickedWhileBusy?.Invoke(booster_Data);
        }
        else
        {
            OnBoosterPurchased?.Invoke(booster_Data);
        }
    }

    public virtual void BoosterPreImplemented(Booster_Item booster_Item)
    {
        booster_Item.COUNT--;
        OnBoosterPreimplemented?.Invoke(booster_Item);
    }

    public virtual void BoosterImplemented(Booster_Item booster_Data)
    {
        OnBoosterImplemented?.Invoke(booster_Data);
    }

    public virtual void IAP_Purchase_Done(PurchaseEventArgs purchaseEventArgs, Product_ID product_ID)
    {
        OnInAppPurchsedDone?.Invoke(purchaseEventArgs, product_ID);
    }

    public void Send_Custom_Event(string eventName)
    {
#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR
                //GameAnalytics.NewDesignEvent(eventName);
#else
        Debug.LogError(eventName);
#endif
    }

    Vector3 cameraOldPosition;
    public virtual void Camera_Focus(Camera_Focus_Data camera_Focus_Data)
    {
        if (focusList == null)
            focusList = new();

        focusList.Add(camera_Focus_Data.followObject, camera_Focus_Data);
        if (focusAlreadyRunning)
            return;

        cameraOldPosition = Camera.main.transform.position;
        FoxTools.functionManager.ExecuteAfterWaiting(Time.deltaTime, () => Focus_On_Next_Object());
    }

    void Focus_On_Next_Object()
    {
        CinemachineBrain targetFollower = Camera.main.GetComponent<CinemachineBrain>();
        targetFollower.enabled = false;

        focusAlreadyRunning = true;
        if (focusList.Count > 0)
        {
            if (!gameState.Equals(GameState.GAME_PLAY_PAUSED))
                ChangeGameState(GameState.GAME_PLAY_PAUSED);

            Camera_Focus_Data camera_Focus_Data = focusList.Values.First();
            Transform followObject = camera_Focus_Data.followObject;
            focusList.Remove(followObject);

            Vector3 dir = -Camera.main.transform.forward;
            Vector3 pos = followObject.position + dir.normalized * cam_Focus_Distance;
            pos.y = Camera.main.transform.position.y;
            Tweener tweener = Camera.main.transform.DOMove(pos, ConstantManager.DEFAULT_ANIMATION_TIME)
                .OnComplete(() =>
                {
                    camera_Focus_Data.actionAfterFocus?.Invoke();
                    FoxTools.functionManager.ExecuteAfterWaiting(camera_Focus_Data.timeToFocusOnObject, () =>
                    {
                        Focus_On_Next_Object();
                    });
                }).SetEase(Ease.OutQuint);

            tweener.OnUpdate(() =>
            {
                // if the tween isn't close enough to the targetHole, set the end position to the targetHole again
                if (Vector3.Distance(Camera.main.transform.position, pos) > 0.1f)
                {
                    dir = -Camera.main.transform.forward;
                    pos = followObject.position + dir.normalized * cam_Focus_Distance;
                    pos.y = Camera.main.transform.position.y;
                    tweener.ChangeEndValue(pos, true);
                }
            });
        }
        else
        {
            Camera.main.transform.DOMove(cameraOldPosition, 1)
                .OnComplete(() =>
                {
                    ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
                    focusAlreadyRunning = false;
                    targetFollower.enabled = true;
                });
        }
    }
    #endregion COMMON_GAME_FUNCTIONALITIES

    public string EditorIconPath { get { return "FoxManagerIcon"; } }
}
