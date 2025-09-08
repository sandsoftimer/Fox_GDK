using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Booster_Panel_Controller : BaseGameBehaviour
{
    public Popup_Controller popupController;
    public Transform bottomPanel, boosterButtonsHolder;
    public GameObject booster_CancelerView;
    public Image activeBoosterIcon;
    public TMP_Text activeBoosterInfo;
    public Button activeBoosterCanceler, purchaseButton;
    public Animator noCurrencyTextAnim, too_Busy_Anim;
    //public GameObject magnetLockedView, magnetUnlockedView, hammerLockedView, hammerUnlockedView;
    public List<Booster_Item> booster_Items;

    Vector3 titleBGPosition;
    Booster_Item currentActiveBooster;

    int shuffleUseCount = 0;
    int arrangeUseCount = 0;
    int vipUseCount = 0;
    int slotUseCount = 0;
    bool boosterCenceledAlready;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.booster_Panel_Controller = this;

        popupController.gameObject.SetActive(false);
        boosterButtonsHolder.gameObject.SetActive(false);
        booster_CancelerView.SetActive(false);

        //magnetLockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER);
        //magnetUnlockedView.SetActive(gameplayData.currentLevelNumber >= gameManager.constantManager.MAGNET_UNLOCK_LEVEL_NUMBER);
        //hammerLockedView.SetActive(gameplayData.currentLevelNumber < gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER);
        //hammerUnlockedView.SetActive(gameplayData.currentLevelNumber >= gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER);

        bottomPanel.gameObject.SetActive(gameplayData.currentLevelNumber > 0);
        for (int i = 0; i < booster_Items.Count; i++)
        {
            Booster_Item booster_Item = booster_Items[i];
            booster_Item.button.onClick.AddListener(() =>
            {
                Initialize(booster_Item);
            });
        }

        activeBoosterCanceler.onClick.AddListener(() =>
        {
            gameManager.Cancel_Activated_Booster(currentActiveBooster);
        });
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    void Update()
    {
        //if (gameState.Equals(GameState.GAME_INITIALIZED) && Input.GetMouseButtonDown(0))
        //{
        //    gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
        //    gameState = GameState.GAME_PLAY_STARTED;
        //}

        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnBoosterImplemented(Booster_Item booster_Item)
    {
        base.OnBoosterImplemented(booster_Item);

        booster_CancelerView.SetActive(false);
    }

    public override void OnBoosterCanceled(Booster_Item booster_Item)
    {
        base.OnBoosterCanceled(booster_Item);

        booster_CancelerView.SetActive(false);
        gameplayData.Is_Game_Busy_By_Booster = false;
        gameplayData.Is_Magnet_Tap_Mood = false;
        gameplayData.Is_Hammer_Tap_Mood = false;
        gameManager.maskManager.Set_Off_Mask();
        SaveGame();

        if (gameState.Equals(GameState.GAME_PLAY_PAUSED))
        {
            gameManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
        }
    }

    public override void OnBoosterClickedWhileBusy(Booster_Item booster_Item)
    {
        base.OnBoosterClickedWhileBusy(booster_Item);

        too_Busy_Anim.SetTrigger("Show");
    }

    public override void OnBoosterPurchased(Booster_Item booster_Item)
    {
        base.OnBoosterPurchased(booster_Item);

        currentActiveBooster = booster_Item;
        booster_CancelerView.SetActive(true);
        activeBoosterIcon.sprite = booster_Item.icon;
        activeBoosterInfo.text = booster_Item.activeBoosterInfo;
        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
                //gameManager.Send_Custom_Event($"Shuffle:Level_{gameplayData.currentLevelNumber + 1}:Use_{++shuffleUseCount}");
                if (gameplayData.currentLevelNumber == gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1)
                {
                    activeBoosterCanceler.gameObject.SetActive(boosterCenceledAlready);
                    boosterCenceledAlready = true;
                }
                else activeBoosterCanceler.gameObject.SetActive(true);
                break;
            case Booster_Type.MAGNET:
                //gameManager.Send_Custom_Event($"Arrange:Level_{gameplayData.currentLevelNumber + 1}:Use_{++arrangeUseCount}");
                booster_CancelerView.SetActive(false);
                break;
            case Booster_Type.GOAL:
                //gameManager.Send_Custom_Event($"SLOT:Level_{gameplayData.currentLevelNumber + 1}:Use_{++vipUseCount}");
                booster_CancelerView.SetActive(false);
                break;
            case Booster_Type.DOCK:
                booster_CancelerView.SetActive(false);
                //gameManager.Send_Custom_Event($"Slot:Level_{gameplayData.currentLevelNumber + 1}:Use_{++slotUseCount}");
                break;
        }
        if (popupController.gameObject.activeSelf)
        {
            gameManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
            popupController.Instantly_Hide_Popup();
        }
    }

    public override void OnGameOver()
    {
        base.OnGameOver();

        boosterButtonsHolder.gameObject.SetActive(false);
    }

    public override void OnGameStart()
    {
        base.OnGameStart();

        boosterButtonsHolder.gameObject.SetActive(true);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void OnGameRetryYesButtonPress()
    {
        gameManager.OnDirectRetryYesButtonPress();
    }

    Action<bool> onBoosterPurchaseAttemptResultConfirmed;

    public void Close_Booster_Panel()
    {
        if (popupController.gameObject.activeSelf)
            popupController.Hide_Popup();
    }

    public void OnPurchaseButtonPress(Action<bool> onConfirmPurchase)
    {
        gameManager.ButtonTapSound.Play();
        ulong purchasePrice = Get_Price();
        if (gameManager.currencyMainSystem.REMAINING_CURRENCY >= purchasePrice)
        {
            gameManager.currencyMainSystem.DeductCurrency(purchasePrice);
            gameplayData.previousLevelCoinConsumed += (int)purchasePrice;
            //Modify_Booster(booster_Item, 1);
            currentActiveBooster.COUNT += 1;
            gameManager.TryPurchasedBooster(currentActiveBooster);
            onConfirmPurchase?.Invoke(true);
        }
        else
        {
            noCurrencyTextAnim.SetTrigger("Show");
        }
    }

    public void OnRVButtonPress(Action<bool> onConfirmPurchase)
    {
        gameManager.fox_AD_Controller.Show_Rewareded_AD(currentActiveBooster.type.ToString().ToLower(), (bool success) =>
        {
            if (success)
            {
                onConfirmPurchase?.Invoke(true);
                gameManager.ButtonTapSound.Play();

                switch (currentActiveBooster.type)
                {
                    case Booster_Type.HAMMER:
                        currentActiveBooster.COUNT += 2;
                        break;
                    case Booster_Type.MAGNET:
                        currentActiveBooster.COUNT += 1;
                        break;
                    case Booster_Type.GOAL:
                        break;
                    case Booster_Type.DOCK:
                        break;
                }

                currentActiveBooster.Update_Visuals();
                gameManager.TryPurchasedBooster(currentActiveBooster);
            }
        });
    }


    private ulong Get_Price()
    {
        return currentActiveBooster.purchasePrice;
    }

    /// <summary>
    /// This function will be called through buttons of Inspector
    /// </summary>
    /// <param Name="booster_Item"></param>
    public void Initialize(Booster_Item booster_Item)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
        {
            onBoosterPurchaseAttemptResultConfirmed?.Invoke(false);
            onBoosterPurchaseAttemptResultConfirmed = null;
            return;
        }

        if (gameplayData.Is_Game_Busy_By_Booster)
        {
            gameManager.BoosterClickedWhileBusy(booster_Item);
            onBoosterPurchaseAttemptResultConfirmed?.Invoke(false);
            onBoosterPurchaseAttemptResultConfirmed = null;
            return;
        }

        currentActiveBooster = booster_Item;

        if (Is_Booster_Available(booster_Item))
        {
            onBoosterPurchaseAttemptResultConfirmed?.Invoke(true);
            onBoosterPurchaseAttemptResultConfirmed = null;
        }
        else
        {
            popupController.Show_Popup(booster_Item.icon, booster_Item.boosterTitle, booster_Item.boosterInfo, booster_Item.purchasePrice.ToString(),
            null, onConfirm: (bool success) =>
            {
                onBoosterPurchaseAttemptResultConfirmed?.Invoke(success);
                onBoosterPurchaseAttemptResultConfirmed = null;
            });
        }
        gameManager.ButtonTapSound.Play();
    }

    private bool Is_Booster_Available(Booster_Item booster_Data)
    {
        bool flag = false;
        switch (booster_Data.type)
        {
            case Booster_Type.HAMMER:
                if (gameplayData.hammerRemaining > 0)
                {
                    flag = true;
                    //gameplayData.hammerRemaining--;
                    gameManager.TryPurchasedBooster(booster_Data);
                }
                break;
            case Booster_Type.MAGNET:
                if (gameplayData.magnetRemaining > 0)
                {
                    flag = true;
                    //gameplayData.magnetRemaining--;
                    gameManager.TryPurchasedBooster(booster_Data);
                }
                break;
            case Booster_Type.GOAL:
                if (gameplayData.goalRemaining > 0)
                {
                    flag = true;
                    //gameplayData.goalRemaining--;
                    gameManager.TryPurchasedBooster(booster_Data);
                }
                break;
            case Booster_Type.DOCK:
                if (gameplayData.dockRemaining > 0)
                {
                    flag = true;
                    //gameplayData.dockRemaining--;
                    gameManager.TryPurchasedBooster(booster_Data);
                }
                break;
        }
        //SaveGame();

        return flag;
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
