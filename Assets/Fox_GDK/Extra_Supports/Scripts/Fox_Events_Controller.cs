#if PUBLISHER_SDK_INSTALLED
using LionStudios.Suite.Analytics;
#endif

using System.Collections.Generic;
using UnityEngine;

public class Fox_Events_Controller : BaseGameBehaviour
{
    string missionType;
    string missionName;
    int missionID;
    int missionAttempt;
    int userScore;

    bool gameAlreadyStarted;
    List<GameObject> gameObjects;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.fox_ADs_Controller = this;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    protected override void OnReviveGame()
    {
        base.OnReviveGame();

        missionType = "main";
        missionName = $"main_{gameplayData.currentLevelNumber + 1}";
        missionID = gameplayData.currentLevelNumber + 1;
        missionAttempt = gameplayData.commonAttempt;
        userScore = 0;
        string stepName = "revive";
#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        if (Application.isMobilePlatform)
        {
            LionAnalytics.MissionStep(
                missionType: missionType,
                missionName: $"{missionName}",
                missionID: missionID,
                userScore: userScore,
                missionAttempt: missionAttempt,
                stepName: stepName);
        }
#elif UNITY_EDITOR
        if (gameManager.debugModeOn)
            Debug.LogError($"Game Revived\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n userScore: {userScore}\n missionAttempt: {missionAttempt}\n stepName: {stepName}");
#endif
        //gameplayData.reviveAttemptNumber++;
    }

    public override void OnGameOver()
    {
        base.OnGameOver();

        if (gameplayData.gameoverSuccess)
        {
            missionType = "main";
            missionName = $"main_{gameplayData.currentLevelNumber}";
            missionID = gameplayData.currentLevelNumber;
            missionAttempt = gameplayData.commonAttempt;
            userScore = 0;

#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

            if (Application.isMobilePlatform)
            {
                LionAnalytics.MissionCompleted(
                    missionType: missionType,
                    missionName: $"{missionName}",
                    missionID: missionID,
                    userScore: userScore,
                    missionAttempt: missionAttempt);
            }
#elif UNITY_EDITOR
            if (gameManager.debugModeOn)
                Debug.LogError($"Game Success\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n userScore: {userScore}\n missionAttempt: {missionAttempt}");
#endif
            gameplayData.retryAttemptNumber = 1;
            gameplayData.reviveAttemptNumber = 1;
            gameplayData.softFailAttemptNumber = 1;
            gameplayData.commonAttempt = 1;
        }
        else
        {
            missionType = "main";
            missionName = $"main_{gameplayData.currentLevelNumber + 1}";
            missionID = gameplayData.currentLevelNumber + 1;
            missionAttempt = gameplayData.commonAttempt;
            userScore = 0;
            string stepName = "soft_fail";
#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        if (Application.isMobilePlatform)
        {
            LionAnalytics.MissionStep(
                missionType: missionType,
                missionName: $"{missionName}",
                missionID: missionID,
                userScore: userScore,
                missionAttempt: missionAttempt,
                stepName: stepName);
        }
#elif UNITY_EDITOR
            if (gameManager.debugModeOn)
                Debug.LogError($"Game Soft-fail\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n userScore: {userScore}\n missionAttempt: {missionAttempt}\n stepName: {stepName}");
#endif
            //gameplayData.softFailAttemptNumber++;
        }
        SaveGame();
    }

    public void GameFailed()
    {
        missionType = "main";
        missionName = $"main_{gameplayData.currentLevelNumber + 1}";
        missionID = gameplayData.currentLevelNumber + 1;
        missionAttempt = gameplayData.commonAttempt;
        userScore = 0;
        string failReason = "dock_full";
#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        if (Application.isMobilePlatform)
        {
            LionAnalytics.MissionFailed(
            missionType: missionType,
            missionName: $"{missionName}",
            missionID: missionID,
            userScore: userScore,
            missionAttempt: missionAttempt,
            failReason: failReason);
        }
#elif UNITY_EDITOR
        if (gameManager.debugModeOn)
            Debug.LogError($"Game Failed\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n userScore: {userScore}\n missionAttempt: {missionAttempt}\n failReason: {failReason}");
#endif
        gameplayData.commonAttempt++;
        SaveGame();
    }


    public override void OnGameStart()
    {
        base.OnGameStart();

        if (gameAlreadyStarted)
            return;

        gameAlreadyStarted = true;
        missionType = "main";
        missionName = $"main_{gameplayData.currentLevelNumber + 1}";
        missionID = gameplayData.currentLevelNumber + 1;
        missionAttempt = gameplayData.commonAttempt;
        userScore = 0;

#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        if (Application.isMobilePlatform)
        {
            LionAnalytics.MissionStarted(
            missionType: missionType,
            missionName: $"{missionName}",
            missionID: missionID,
            missionAttempt: missionAttempt);
        }
#elif UNITY_EDITOR
        if (gameManager.debugModeOn)
            Debug.LogError($"Game Started\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n missionAttempt: {missionAttempt}");
#endif
    }

    public override void OnBoosterPreimplemented(Booster_Item booster_Item)
    {
        base.OnBoosterPreimplemented(booster_Item);

        missionID = gameplayData.currentLevelNumber + 1;
        missionType = "main";
        missionAttempt = gameplayData.commonAttempt;
        missionName = $"main_{gameplayData.currentLevelNumber + 1}";
        userScore = 0;
        string powerUpName = $"{booster_Item.type}".ToLower();
        int powerUpRemaining = booster_Item.COUNT;

#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        if (Application.isMobilePlatform)
        {
            LionAnalytics.PowerUpUsed(
            missionID: missionID.ToString(),
            missionType: missionType,
            missionAttempt: missionAttempt,
            powerUpName: powerUpName,
            missionName: $"{missionName}",
            new Dictionary<string, object>
            {
                { "power_up_remaining", $"{powerUpRemaining}" }
            });
        }
#elif UNITY_EDITOR
        if (gameManager.debugModeOn)
            Debug.LogError($"Booster Purchased\nmissionID: {missionID}\n missionType: {missionType}\n missionAttempt: {missionAttempt}\n powerUpName: {powerUpName}\n missionName: {missionName}\n power_up_remaining: {powerUpRemaining}");
#endif

    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void OnDirectRetryYesButtonPress()
    {
        missionType = "main";
        missionName = $"main_{gameplayData.currentLevelNumber + 1}";
        missionID = gameplayData.currentLevelNumber + 1;
        missionAttempt = gameplayData.commonAttempt;
        userScore = 0;

#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR
        if (Application.isMobilePlatform)
        {
            LionAnalytics.MissionAbandoned(
                missionType: missionType,
                missionName: $"{missionName}",
                missionID: missionID,
                userScore: userScore,
                missionAttempt: missionAttempt);
        }
#elif UNITY_EDITOR
        if (gameManager.debugModeOn)
            Debug.LogError($"Mission Abandoned\nmissionType: {missionType}\n missionName: {missionName}\n missionID: {missionID}\n userScore: {userScore}\n missionAttempt: {missionAttempt}");
#endif
        gameplayData.commonAttempt++;
        gameplayData.directStart = true;
        SaveGame();
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
