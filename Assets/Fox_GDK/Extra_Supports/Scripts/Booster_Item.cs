using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Booster_Item : BaseGameBehaviour
{
    public Booster_Type type;
    public string boosterTitle;
    public string boosterInfo;
    public string activeBoosterInfo;
    public ulong purchasePrice;
    public Button button;
    public GameObject lockedView, unlockedView;
    public GameObject greenDot, redDot;
    public TMP_Text redDotCountText;
    public TMP_Text lockText;
    public Sprite icon;

    public int COUNT
    {
        get
        {
            int result = 0;
            switch (type)
            {
                case Booster_Type.HAMMER:
                    result = gameplayData.hammerRemaining;
                    break;
                case Booster_Type.MAGNET:
                    result = gameplayData.magnetRemaining;
                    break;
                case Booster_Type.GOAL:
                    result = gameplayData.goalRemaining;
                    break;
                case Booster_Type.DOCK:
                    result = gameplayData.dockRemaining;
                    break;
            }
            return result;
        }
        set
        {
            switch (type)
            {
                case Booster_Type.HAMMER:
                    gameplayData.hammerRemaining = value;
                    break;
                case Booster_Type.MAGNET:
                    gameplayData.magnetRemaining = value;
                    break;
                case Booster_Type.GOAL:
                    gameplayData.goalRemaining = value;
                    break;
                case Booster_Type.DOCK:
                    gameplayData.dockRemaining = value;
                    break;
            }
        }
    }

    public override void Awake()
    {
        base.Awake();

        button = GetComponent<Button>();
        switch (type)
        {
            case Booster_Type.HAMMER:

                if (gameplayData.hammerRemaining <= 0 && gameplayData.currentLevelNumber == gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1)
                {
                    gameplayData.hammerRemaining = 1;
                    SaveGame();
                }
                lockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1 || gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER < 0);
                lockText.text = $"LVL - {gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER}";
                break;
            case Booster_Type.MAGNET:
                lockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER);
                lockText.text = $"LVL - {gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER}";
                break;
            case Booster_Type.GOAL:

                if (gameplayData.goalRemaining <= 0 && gameplayData.currentLevelNumber == gameManager.constantManager.GOAL_UNLOCK_LEVEL_NUMBER - 1)
                {
                    gameplayData.goalRemaining = 1;
                    SaveGame();
                }
                lockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.GOAL_UNLOCK_LEVEL_NUMBER - 1 || gameManager.constantManager.GOAL_UNLOCK_LEVEL_NUMBER < 0);
                lockText.text = $"LVL - {gameManager.constantManager.GOAL_UNLOCK_LEVEL_NUMBER}";
                break;
            case Booster_Type.DOCK:


                if (gameplayData.dockRemaining <= 0 && gameplayData.currentLevelNumber == gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER - 1)
                {
                    gameplayData.dockRemaining = 1;
                    SaveGame();
                }
                lockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER - 1 || gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER < 0);
                lockText.text = $"LVL - {gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER}";
                break;
        }

        button.interactable = !lockedView.activeSelf;
        unlockedView.SetActive(!lockedView.activeSelf);

    }

    public override void Start()
    {
        base.Start();

        if (button.interactable)
        {
            if (COUNT > 0)
            {
                redDot.SetActive(true);
                greenDot.SetActive(false);
                redDotCountText.text = $"{COUNT}";
            }
            else
            {
                redDot.SetActive(false);
                greenDot.SetActive(true);
            }
        }
        else
        {
            greenDot.SetActive(false);
            redDot.SetActive(false);
        }
    }

    public override void OnBoosterPreimplemented(Booster_Item booster_Item)
    {
        base.OnBoosterPreimplemented(booster_Item);

        Update_Visuals();
    }

    public override void OnBoosterImplemented(Booster_Item booster_Item)
    {
        base.OnBoosterImplemented(booster_Item);

        Update_Visuals();
    }

    private void Unlock_Magnet()
    {
        if (gameplayData.magnetRemaining <= 0 && gameplayData.currentLevelNumber == gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER - 1)
        {
            gameplayData.magnetRemaining = 1;
            SaveGame();
        }
        lockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER - 1 || gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER < 0);
        lockText.text = $"LVL - {gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER}";
        button.interactable = !lockedView.activeSelf;
        unlockedView.SetActive(!lockedView.activeSelf);

        redDotCountText.text = $"{COUNT}";
        redDot.SetActive(true);
        greenDot.SetActive(false);
    }

    public void Update_Visuals()
    {
        redDotCountText.text = $"{COUNT}";
        if (COUNT > 0)
        {
            redDot.SetActive(true);
            greenDot.SetActive(false);
        }
        else
        {
            redDot.SetActive(false);
            greenDot.SetActive(true);
        }
    }
}
