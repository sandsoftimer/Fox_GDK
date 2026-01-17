using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeaturesController : BaseGameBehaviour
{
    public List<FeatureData> newFeaturesData;

    [Space]
    public GameObject featureUnlockView;
    public Fox_Canvas_Animator viewAnim;
    public RectTransform holder;
    public TMP_Text featureName;
    public TMP_Text featureDescription;
    public Image featureIcon;

    FeatureData foundFeature = null;
    Action showGameoverView = null;
    int unlockLevelNumber;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        //featureUnlockView.SetActive(false);
        featureUnlockView.GetComponent<Button>().onClick.AddListener(OnDone);

        gameManager.featuresController = this;
        Set_Unlock_Info();
        foundFeature = Is_Any_Feature_To_Unlock();
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

    public override void OnGameStart()
    {
        base.OnGameStart();
    }

    public override void OnPauseGamePlay()
    {
        base.OnPauseGamePlay();

        gameManager.allButtonsUI.Set_Visual(false);
    }

    public override void OnUnPauseGamePlay()
    {
        base.OnUnPauseGamePlay();

        gameManager.allButtonsUI.Set_Visual(true);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public bool Any_Feature_Unlocked(Action action)
    {
        if (foundFeature != null)
        {
            showGameoverView = action;
            Initialize_Feature(foundFeature);
        }

        return foundFeature != null;
    }

    public FeatureData Is_Any_Feature_To_Unlock()
    {
        for (int i = 0; i < newFeaturesData.Count; i++)
        {
            if (gameplayData.currentLevelNumber + 1 == newFeaturesData[i].unlockLevelNumber)
            {
                foundFeature = newFeaturesData[i];
                break;
            }
        }
        return foundFeature;
    }

    public void Initialize_Feature(FeatureData featureData)
    {
        //if (PlayerPrefsX.GetBool(featureData.featureName))
        //    return;

        PlayerPrefsX.SetBool(featureData.featureName, true);

        //gameManager.ChangeGameState(GameState.GAME_PLAY_PAUSED);

        featureName.text = featureData.featureName;
        featureDescription.text = featureData.featureDescription;
        featureIcon.sprite = featureData.featureIcon;
        holder.anchoredPosition = featureData.panelPosition;
        featureUnlockView.SetActive(true);
        viewAnim.Set_Visual(true);
    }

    void OnDone()
    {
        viewAnim.Set_Visual(false);
        //featureUnlockView.SetActive(false);
        showGameoverView?.Invoke();
    }

    private void Set_Unlock_Info()
    {
        for (int i = 0; i < newFeaturesData.Count; i++)
        {
            switch (newFeaturesData[i].type)
            {
                case FeatureType.ICE_BOOSTER:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.ICE_BOOSTER_UNLOCK;
                    break;
                case FeatureType.UZI_GUN:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.UZI_GUN_UNLOCK;
                    break;
                case FeatureType.TROOPS_LEVEL_3:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.TROOPS_LEVEL_3_UNLOCK;
                    break;
                case FeatureType.SNAIL_BOOSTER:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.SNAIL_BOOSTER_UNLOCK;
                    break;
                case FeatureType.MACHINE_GUN_UPDATE:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.MACHINE_GUN_UPDATE_UNLOCK;
                    break;
                case FeatureType.TROOPS_LEVEL_4:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.TROOPS_LEVEL_4_UNLOCK;
                    break;
                case FeatureType.EXPLOSION_BOOSTER:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.EXPLOSION_BOOSTER_UNLOCK;
                    break;
            }
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}


[Serializable]
public class FeatureData
{
    public string featureName;
    public FeatureType type;
    public string featureDescription;
    public Sprite featureIcon;
    public int unlockLevelNumber;
    public Vector3 panelPosition;
}

public enum FeatureType
{
    ICE_BOOSTER,
    UZI_GUN,
    TROOPS_LEVEL_3,
    SNAIL_BOOSTER,
    MACHINE_GUN_UPDATE,
    TROOPS_LEVEL_4,
    EXPLOSION_BOOSTER
}