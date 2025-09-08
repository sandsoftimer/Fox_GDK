using UnityEngine.Purchasing;

public class No_ADs_View : BaseGameBehaviour
{
    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameObject.SetActive(false);
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

        switch (product_ID)
        {
            case Product_ID.NO_ADS:
                gameObject.SetActive(false);
                break;
            case Product_ID.NO_ADS_BUNDLE:
                break;
            case Product_ID.STARTER_PACK:
                break;
            case Product_ID.BEGINNER_PACK:
                break;
            case Product_ID.MASTER_PACK:
                break;
            case Product_ID.MEGA_PACK:
                break;
            case Product_ID.COIN_PACK_1:
                break;
            case Product_ID.COIN_PACK_2:
                break;
            case Product_ID.COIN_PACK_3:
                break;
            case Product_ID.COIN_PACK_4:
                break;
            case Product_ID.COIN_PACK_5:
                break;
            case Product_ID.COIN_PACK_6:
                break;
        }
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS
}
