using System;
using UnityEngine;

[Serializable]
public partial class GameplayData
{
    public int gameScore;
    public int currentLevelNumber;

    public int retryAttemptNumber = 1;
    public int reviveAttemptNumber = 1;
    public int softFailAttemptNumber = 1;
    public int commonAttempt = 1;

    public float gameStartTime, gameEndTime;
    public float totalLevelCompletedTime;

    public ulong totalCoin;

    public ulong healthBaseValue;
    public ulong healthValueIncreaser;
    public ulong healthUpgradeLevel;

    public ulong damageBaseValue;
    public ulong damageValueIncreaser;
    public ulong damageUpgradeLevel;

    public bool gameoverSuccess;
    public bool soundEffectOn = true;
    public bool backgroundMusicOn = true;
    public bool vibrationEffectOn = true;
    public bool tutorial_Tap_Blocker = false;

    public Level_Data current_Level_Data;

    partial void Partial_Constructor();

    public GameplayData()
    {
        Partial_Constructor();
        gameScore = 0;
        currentLevelNumber = 0;

        totalCoin = 0;
        healthBaseValue = 100;
        healthValueIncreaser = 20;
        damageBaseValue = 10;
        damageValueIncreaser = 2;
        healthUpgradeLevel = 0;
        damageUpgradeLevel = 0;

        gameStartTime = 0f;
        gameEndTime = 0f;
        totalLevelCompletedTime = 0f;

        gameoverSuccess = false;
    }

    public ulong Get_Upgraded_Health
    {
        get { return healthBaseValue + healthUpgradeLevel * healthValueIncreaser; }
    }

    public ulong Get_Upgraded_Damage_Power
    {
        get { return damageBaseValue + damageUpgradeLevel * damageValueIncreaser; }
    }
}

public class EditorButtonStyle
{
    public Color buttonColor;
    public Color buttonTextColor;
    public float buttonWidth = 25;
    public float buttonHeight = 25;
}

public class Camera_Focus_Data
{
    public Transform followObject;
    public float timeToFocusOnObject = 0.5f;
    public float cameraHeight = 15f;
    public Action actionAfterFocus;
}

[Serializable]
public class SqueezableData
{
    public float bumpUpValue = 1f;
    public float bumpDownValue = 0.15f;
    public float squeezeUpValue = 0.05f;
    public float squeezeDownValue = 0.05f;
}
