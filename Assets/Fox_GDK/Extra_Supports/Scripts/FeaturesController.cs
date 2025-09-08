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
    public RectTransform holder;
    public TMP_Text featureName;
    public TMP_Text featureDescription;
    public Image featureIcon;

    int unlockLevelNumber;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        featureUnlockView.SetActive(false);
        featureUnlockView.GetComponent<Button>().onClick.AddListener(OnCancel);

        gameManager.featuresController = this;
        Set_Unlock_Info();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        FeatureData foundFeature = null;
        for (int i = 0; i < newFeaturesData.Count; i++)
        {
            if ((gameplayData.currentLevelNumber + 1) == newFeaturesData[i].unlockLevelNumber)
            {
                foundFeature = newFeaturesData[i];
                break;
            }
        }

        if (foundFeature != null)
        {
            Initialize_Feature(foundFeature);
        }
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
    }

    void OnCancel()
    {
        featureUnlockView.SetActive(false);
        //gameManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
    }

    private void Set_Unlock_Info()
    {
        for (int i = 0; i < newFeaturesData.Count; i++)
        {
            switch (newFeaturesData[i].type)
            {
                case FeatureType.SQURE_HOLE:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.SQURE_HOLES_TUTORIAL_LEVEL;
                    break;
                case FeatureType.HIDDEN_BLOCK:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.HIDDEN_BLOCK_TUTORIAL_LEVEL;
                    break;
                case FeatureType.FROZEN_SPAWNER:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.FROZEN_TUTORIAL_LEVEL;
                    break;
                case FeatureType.TRIANGLE_HOLE:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.TRIANGLE_HOLES_TUTORIAL_LEVEL;
                    break;
                case FeatureType.CONNECTED_HOLES:
                    newFeaturesData[i].unlockLevelNumber = gameManager.constantManager.CONNECTED_HOLES_TUTORIAL_LEVEL;
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
    SQURE_HOLE,
    HIDDEN_BLOCK,
    FROZEN_SPAWNER,
    TRIANGLE_HOLE,
    CONNECTED_HOLES,
    MULTISTORED,
}