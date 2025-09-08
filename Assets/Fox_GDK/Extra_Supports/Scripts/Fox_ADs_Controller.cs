#if PUBLISHER_SDK_INSTALLED
using LionStudios.Suite.Ads;
using LionStudios.Suite.Analytics;
#endif


using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class Fox_ADs_Controller : BaseGameBehaviour
{
    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.fox_AD_Controller = this;
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

    public override void OnInAppPurchsedDone(PurchaseEventArgs purchaseEvent, Product_ID product_ID)
    {
        base.OnInAppPurchsedDone(purchaseEvent, product_ID);

        switch (product_ID)
        {
            case Product_ID.NO_ADS:
            case Product_ID.NO_ADS_BUNDLE:
                Hide_Banner_AD();
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

    //Optional
    private void OnRewardedAdClosed()
    {
        Debug.LogError("On Rewarded ad closed");
    }

#if PUBLISHER_SDK_INSTALLED

    //Optional
    private Reward GetTestReward()
    {
        Reward testReward = new Reward("test coins", "test currency", 10);
        return testReward;
    }
#endif

    public void Show_Rewareded_AD(string rewardName, Action<bool> OnRewarded)
    {
#if PUBLISHER_SDK_INSTALLED

        if (!LionAds.IsRewardedReady)
        {
            OnRewarded?.Invoke(false);
            FoxTools.canvasManager.ShowMessage("Ad not ready", "Your rewarded Ad in not ready.\n<color=#69BC20>Please try again.</color>");
            return;
        }
        else
            LionAds.TryShowRewarded(rewardName, () => OnRewarded?.Invoke(true), () => OnRewardedAdClosed(), GetTestReward());
#else
        //LionAds.TryShowRewarded(rewardName, () => OnRewarded?.Invoke(true), () => OnRewardedAdClosed(), GetTestReward());
        FoxTools.functionManager.ExecuteAfterWaiting(ConstantManager.ONE_FORTH_TIME, () => { OnRewarded?.Invoke(true); });
#endif
    }

    public void Show_Instatial_AD()
    {
        if (gameplayData.is_AD_Purchased)
            return;

#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        //LionAds.TryShowInterstitial("LevelStart", OnInterClosed, additionalData);
#else
        Debug.LogError("Instatial Ad Shows Here");
#endif
    }

    public void Show_Banner_AD()
    {
        if (gameplayData.is_AD_Purchased)
            return;


#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        LionAds.ShowBanner();
#else
        //LionAds.ShowBanner();
        Debug.LogError("Banner is Showing Now");
#endif
    }

    public void Hide_Banner_AD()
    {
#if PUBLISHER_SDK_INSTALLED && !UNITY_EDITOR

        LionAds.HideBanner();
#else
        //LionAds.HideBanner();
        Debug.LogError("Banner is Hiding Now");
#endif
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
