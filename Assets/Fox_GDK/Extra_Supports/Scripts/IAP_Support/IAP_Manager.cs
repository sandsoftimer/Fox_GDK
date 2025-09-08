using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAP_Manager : BaseGameBehaviour, IDetailedStoreListener
{
    public IStoreController storeController;
    public IExtensionProvider extensionProvider;

    public IAP_Product_Data noAds;
    public IAP_Product_Data noAdsBundle;
    public IAP_Product_Data starterPack;
    public IAP_Product_Data coinPack_1;
    public IAP_Product_Data coinPack_2;
    public IAP_Product_Data coinPack_3;
    public IAP_Product_Data coinPack_4;
    public IAP_Product_Data coinPack_5;
    public IAP_Product_Data coinPack_6;
    public IAP_Product_Data beginner_pack;
    public IAP_Product_Data master_pack;
    public IAP_Product_Data mega_pack;

    public GameObject extraNoADButton;

    public bool Is_Initialize
    {
        get { return storeController != null && extensionProvider != null; }
    }

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        SetupBuilders();
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
        Check_Already_Nonconsumable_Purchased();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("Initialization failed");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("Initialization failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError("Purchase failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        PurchaseProcessingResult result = PurchaseProcessingResult.Complete;
        var product = purchaseEvent.purchasedProduct;
        Debug.LogError($"Purchase success id: {product.definition.id}");

        Implement_IAP(purchaseEvent, product);
        return result;
    }

    public override void OnInAppPurchsedDone(PurchaseEventArgs purchaseEventArgs, Product_ID product_ID)
    {
        base.OnInAppPurchsedDone(purchaseEventArgs, product_ID);

#if MOONEE_SDK_INSTALLED && !UNITY_EDITOR
        MoonSDK.TrackAdjustRevenueEvent(purchaseEventArgs, iAPType.product, (gameplayData.currentLevelNumber + 1).ToString());
#endif

        switch (product_ID)
        {
            case Product_ID.NO_ADS:
                gameplayData.noAd_Product_hasReceipt = true;
                gameplayData.is_AD_Purchased = true;
                break;
            case Product_ID.NO_ADS_BUNDLE:
                gameplayData.noAdBundle_Product_hasReceipt = true;
                gameplayData.is_AD_Purchased = true;
                gameManager.currencyMainSystem.AddCurrency(1000, ConstantManager.DEFAULT_ANIMATION_TIME);
                gameplayData.magnetRemaining++;
                gameplayData.goalRemaining++;
                gameplayData.hammerRemaining++;
                break;
            case Product_ID.STARTER_PACK:
                gameplayData.starter_Product_hasReceipt = true;
                gameManager.currencyMainSystem.AddCurrency(800, ConstantManager.DEFAULT_ANIMATION_TIME);
                gameplayData.magnetRemaining++;
                gameplayData.goalRemaining++;
                gameplayData.hammerRemaining++;
                break;
            case Product_ID.BEGINNER_PACK:
                gameManager.currencyMainSystem.AddCurrency(800, ConstantManager.DEFAULT_ANIMATION_TIME);
                gameplayData.magnetRemaining += 2;
                gameplayData.goalRemaining += 2;
                gameplayData.hammerRemaining += 2;
                break;
            case Product_ID.MASTER_PACK:
                gameManager.currencyMainSystem.AddCurrency(1600, ConstantManager.DEFAULT_ANIMATION_TIME);
                gameplayData.magnetRemaining += 3;
                gameplayData.goalRemaining += 3;
                gameplayData.hammerRemaining += 3;
                break;
            case Product_ID.MEGA_PACK:
                gameManager.currencyMainSystem.AddCurrency(3200, ConstantManager.DEFAULT_ANIMATION_TIME);
                gameplayData.magnetRemaining += 6;
                gameplayData.goalRemaining += 6;
                gameplayData.hammerRemaining += 6;
                break;
            case Product_ID.COIN_PACK_1:
                gameManager.currencyMainSystem.AddCurrency(300, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
            case Product_ID.COIN_PACK_2:
                gameManager.currencyMainSystem.AddCurrency(1500, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
            case Product_ID.COIN_PACK_3:
                gameManager.currencyMainSystem.AddCurrency(3300, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
            case Product_ID.COIN_PACK_4:
                gameManager.currencyMainSystem.AddCurrency(7500, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
            case Product_ID.COIN_PACK_5:
                gameManager.currencyMainSystem.AddCurrency(14000, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
            case Product_ID.COIN_PACK_6:
                gameManager.currencyMainSystem.AddCurrency(30000, ConstantManager.DEFAULT_ANIMATION_TIME);
                break;
        }
        SaveGame();

        Check_Already_Nonconsumable_Purchased();
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    void Implement_IAP(PurchaseEventArgs purchaseEvent, Product product)
    {
        Product_ID productID = ProductID.Get_Product_Id(product.definition.id);

        if (!gameplayData.iap_Purchased_IDs.ContainsKey(product.definition.id))
        {
            gameplayData.iap_Purchased_IDs.Add(product.definition.id, product.definition.id);
        }
        SaveGame();
        gameManager.IAP_Purchase_Done(purchaseEvent, productID);
    }

    void Check_Already_Nonconsumable_Purchased()
    {
        if (storeController == null)
            return;

        var noAd_Product = storeController.products.WithID(ProductID.NO_ADS);
        var noAdBundle_Product = storeController.products.WithID(ProductID.NO_ADS_BUNDLE);
        var starter_Product = storeController.products.WithID(ProductID.STARTER_PACK);

#if MOONEE_SDK_INSTALLED && !UNITY_EDITOR
        noAds.gameObject.SetActive(!(noAd_Product.hasReceipt || noAdBundle_Product.hasReceipt));
        noAdsBundle.gameObject.SetActive(!noAdBundle_Product.hasReceipt);
        starterPack.gameObject.SetActive(!starter_Product.hasReceipt);
        extraNoADButton.SetActive(!(noAd_Product.hasReceipt || noAdBundle_Product.hasReceipt));
#else
        noAds.gameObject.SetActive(!(gameplayData.noAd_Product_hasReceipt || gameplayData.noAdBundle_Product_hasReceipt));
        noAdsBundle.gameObject.SetActive(!gameplayData.noAdBundle_Product_hasReceipt);
        starterPack.gameObject.SetActive(!gameplayData.starter_Product_hasReceipt);
        extraNoADButton.SetActive(!(gameplayData.noAd_Product_hasReceipt || gameplayData.noAdBundle_Product_hasReceipt));
#endif

        if ((noAd_Product.hasReceipt || noAdBundle_Product.hasReceipt) && gameplayData.is_AD_Purchased == false)
        {
            gameplayData.is_AD_Purchased = true;
            SaveGame();
            gameManager.fox_AD_Controller.Hide_Banner_AD();
        }
        else
        {
            gameManager.fox_AD_Controller.Show_Banner_AD();
        }
    }

    public void Show_IAP_Buy_Popup(IAP_Product_Data data)
    {
#if MOONEE_SDK_INSTALLED && !UNITY_EDITOR
        string productId = ProductID.Get_Product_Id(data.id);
        if (Is_Initialize)
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
#else
        Product_ID productID = data.id;
        string product_Id = ProductID.Get_Product_Id(data.id);

        if (!gameplayData.iap_Purchased_IDs.ContainsKey(product_Id))
        {
            gameplayData.iap_Purchased_IDs.Add(product_Id, product_Id);
        }
        SaveGame();
        gameManager.IAP_Purchase_Done(null, productID);
#endif
    }
    void SetupBuilders()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(ProductID.NO_ADS, ProductType.NonConsumable);
        builder.AddProduct(ProductID.NO_ADS_BUNDLE, ProductType.NonConsumable);
        builder.AddProduct(ProductID.STARTER_PACK, ProductType.NonConsumable);
        builder.AddProduct(ProductID.COIN_PACK_1, ProductType.Consumable);
        builder.AddProduct(ProductID.COIN_PACK_2, ProductType.Consumable);
        builder.AddProduct(ProductID.COIN_PACK_3, ProductType.Consumable);
        builder.AddProduct(ProductID.COIN_PACK_4, ProductType.Consumable);
        builder.AddProduct(ProductID.COIN_PACK_5, ProductType.Consumable);
        builder.AddProduct(ProductID.COIN_PACK_6, ProductType.Consumable);
        builder.AddProduct(ProductID.BEGINNER_PACK, ProductType.Consumable);
        builder.AddProduct(ProductID.MASTER_PACK, ProductType.Consumable);
        builder.AddProduct(ProductID.MEGA_PACK, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    #endregion ALL SELF DECLARE FUNCTIONS
}

[Serializable]
public class PayloadData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity;
    public bool acknowledged;
}

[Serializable]
public class Payload
{
    public string json;
    public string signature;
    public PayloadData payloadData;
}

[Serializable]
public class Data
{
    public string Payload;
    public string Store;
    public string TransactionID;
}