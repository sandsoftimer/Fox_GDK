using TMPro;
using UnityEngine.Purchasing;

public class TopBoosterPanel : BaseGameBehaviour
{
    public TMP_Text vipTopText, shuffleTopText, arrangeTopText;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        Update_Visuals();
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

    public override void OnInAppPurchsedDone(PurchaseEventArgs purchaseEventArgs, Product_ID product_ID)
    {
        base.OnInAppPurchsedDone(purchaseEventArgs, product_ID);

        Update_Visuals();
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    private void Update_Visuals()
    {
        FoxTools.functionManager.ExecuteAfterWaiting(0.1f, () =>
        {
            vipTopText.text = $"{gameplayData.goalRemaining}";
            shuffleTopText.text = $"{gameplayData.hammerRemaining}";
            arrangeTopText.text = $"{gameplayData.magnetRemaining}";
        });
    }


    #endregion ALL SELF DECLARE FUNCTIONS
}
