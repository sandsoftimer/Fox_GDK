using System;
using System.Collections.Generic;

public partial class GameplayData
{
    public int numberOfSlot = 2;
    public int hammerRemaining = 0, magnetRemaining = 0, goalRemaining = 0, dockRemaining = 0;
    public int previousLevelCoinConsumed = 0;

    public bool isContinueLevel = false;
    public bool directStart = true;
    public bool is_AD_Purchased = false;

    public bool noAd_Product_hasReceipt = false;
    public bool noAdBundle_Product_hasReceipt = false;
    public bool starter_Product_hasReceipt = false;

    public bool Is_Game_Busy_By_Booster;
    public bool Is_Hammer_Tap_Mood;
    public bool Is_Magnet_Tap_Mood;

    public DateTime last_Instatial_AD_Show_Time = new DateTime();

    public Dictionary<string, string> iap_Purchased_IDs = new();
}

public class ProductID
{
    public const string NO_ADS = "com.kolpoverse.luggage.no_ads";
    public const string NO_ADS_BUNDLE = "com.kolpoverse.luggage.no_ads_bundle";
    public const string STARTER_PACK = "com.kolpoverse.luggage.starter_pack";
    public const string BEGINNER_PACK = "com.kolpoverse.luggage.beginner_pack";
    public const string MASTER_PACK = "com.kolpoverse.luggage.master_pack";
    public const string MEGA_PACK = "com.kolpoverse.luggage.mega_pack";
    public const string COIN_PACK_1 = "com.kolpoverse.luggage.coins_bundle1";
    public const string COIN_PACK_2 = "com.kolpoverse.luggage.coins_bundle2";
    public const string COIN_PACK_3 = "com.kolpoverse.luggage.coins_bundle3";
    public const string COIN_PACK_4 = "com.kolpoverse.luggage.coins_bundle4";
    public const string COIN_PACK_5 = "com.kolpoverse.luggage.coins_bundle5";
    public const string COIN_PACK_6 = "com.kolpoverse.luggage.coins_bundle6";

    public static string Get_Product_Id(Product_ID product_ID)
    {
        string result = "";

        switch ((product_ID))
        {
            case Product_ID.NO_ADS:
                result = NO_ADS;
                break;
            case Product_ID.NO_ADS_BUNDLE:
                result = NO_ADS_BUNDLE;
                break;
            case Product_ID.STARTER_PACK:
                result = STARTER_PACK;
                break;
            case Product_ID.BEGINNER_PACK:
                result = BEGINNER_PACK;
                break;
            case Product_ID.MASTER_PACK:
                result = MASTER_PACK;
                break;
            case Product_ID.MEGA_PACK:
                result = MEGA_PACK;
                break;
            case Product_ID.COIN_PACK_1:
                result = COIN_PACK_1;
                break;
            case Product_ID.COIN_PACK_2:
                result = COIN_PACK_2;
                break;
            case Product_ID.COIN_PACK_3:
                result = COIN_PACK_3;
                break;
            case Product_ID.COIN_PACK_4:
                result = COIN_PACK_4;
                break;
            case Product_ID.COIN_PACK_5:
                result = COIN_PACK_5;
                break;
            case Product_ID.COIN_PACK_6:
                result = COIN_PACK_6;
                break;
        }
        return result;
    }

    public static Product_ID Get_Product_Id(string product_ID)
    {
        Product_ID result = Product_ID.NO_ADS;

        switch ((product_ID))
        {
            case NO_ADS:
                result = Product_ID.NO_ADS;
                break;
            case NO_ADS_BUNDLE:
                result = Product_ID.NO_ADS_BUNDLE;
                break;
            case STARTER_PACK:
                result = Product_ID.STARTER_PACK;
                break;
            case BEGINNER_PACK:
                result = Product_ID.BEGINNER_PACK;
                break;
            case MASTER_PACK:
                result = Product_ID.MASTER_PACK;
                break;
            case MEGA_PACK:
                result = Product_ID.MEGA_PACK;
                break;
            case COIN_PACK_1:
                result = Product_ID.COIN_PACK_1;
                break;
            case COIN_PACK_2:
                result = Product_ID.COIN_PACK_2;
                break;
            case COIN_PACK_3:
                result = Product_ID.COIN_PACK_3;
                break;
            case COIN_PACK_4:
                result = Product_ID.COIN_PACK_4;
                break;
            case COIN_PACK_5:
                result = Product_ID.COIN_PACK_5;
                break;
            case COIN_PACK_6:
                result = Product_ID.COIN_PACK_6;
                break;
        }
        return result;
    }
}
