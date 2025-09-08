using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelContinueView : BaseGameBehaviour
{
    public Popup_Controller popupController;
    public Button currencyBuyButton, rvBuyButton, closeButton;
    public GameObject retryView;
    public Animator noCurrencyAnim;
    public TMP_Text bodyText, titleText, purchaseText;
    public Sprite dualSprite, singleSprite;
    public ulong purchasePrice = 2000;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.levelContinueView = this;
        popupController.gameObject.SetActive(false);
        retryView.SetActive(false);

        currencyBuyButton.onClick.AddListener(OnPurchaseByCoin);
        rvBuyButton.onClick.AddListener(OnRVButtonPress);
        closeButton.onClick.AddListener(OnCloseButtonPress);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        purchaseText.text = $"{purchasePrice}";
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

    protected override void OnReviveGame()
    {
        base.OnReviveGame();

        //gameManager.TryPurchasedBooster(new Booster_Item { type = Booster_Type.MAGNET });

        //if (slotRemaining > 0)
        //    gameManager.TryPurchasedBooster(new Booster_Item { type = Booster_Type.DOCK });

        popupController.Instantly_Hide_Popup();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);

        gameplayData.isContinueLevel = true;
        //Debug.LogError("Revived");
        //gameManager.Send_Custom_Event($"Revive:Level_{gameplayData.currentLevelNumber + 1}:Attempt_{gameplayData.reviveAttemptNumber++}");
    }

    public override void OnGameOver()
    {
        base.OnGameOver();

        if (gameplayData.gameoverSuccess)
            return;

        Temp_Solution_For_This_Build_Olny();
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Temp_Solution_For_This_Build_Olny()
    {
        gameManager.ChangeGameState(GameState.GAME_PLAY_PAUSED);
        //retryView.SetActive(true);

        Popup_This();
        //OnCloseButtonPress();
    }

    public void Popup_This()
    {
        currencyBuyButton.gameObject.SetActive(gameManager.currencyMainSystem.REMAINING_CURRENCY >= purchasePrice);
        rvBuyButton.gameObject.SetActive(gameManager.currencyMainSystem.REMAINING_CURRENCY < purchasePrice);

        popupController.Instantly_Show_Popup();
    }

    public void OnGameRetryYesButtonPress()
    {
        gameManager.fox_ADs_Controller.GameFailed();
        gameManager.ReloadLevel();
    }

    public void OnRVButtonPress()
    {
        gameManager.fox_AD_Controller.Show_Rewareded_AD("revive", (bool success) =>
        {
            if (success)
            {
                gameManager.ReviveGame();
            }
        });
    }

    public void OnPurchaseByCoin()
    {
        if (purchasePrice > gameManager.currencyMainSystem.REMAINING_CURRENCY)
        {
            noCurrencyAnim.SetTrigger("Show");
        }
        else
        {
            gameManager.currencyMainSystem.DeductCurrency(purchasePrice);

            gameManager.ReviveGame();
        }
    }
    private void OnCloseButtonPress()
    {
        popupController.Instantly_Hide_Popup();
        retryView.SetActive(true);
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
