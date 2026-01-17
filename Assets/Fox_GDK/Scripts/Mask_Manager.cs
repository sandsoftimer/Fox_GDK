using DG.Tweening;
using UnityEngine.UI;

public class Mask_Manager : BaseGameBehaviour
{
    public Image img;

    UI_Hole_Data uI_Hole_Data;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.maskManager = this;

        img.gameObject.SetActive(false);
        //gameObject.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL);
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

    public override void OnBoosterPurchased(Booster_Item booster_Item)
    {
        base.OnBoosterPurchased(booster_Item);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                gameObject.SetActive(false);
                break;
            case Booster_Type.MAGNET:
                break;
        }
    }

    public override void OnBoosterPreimplemented(Booster_Item booster_Item)
    {
        base.OnBoosterPreimplemented(booster_Item);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                gameObject.SetActive(false);
                break;
            case Booster_Type.MAGNET:
                Set_Off_Mask();
                break;
        }

    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Initialize_Next_Masking(UI_Hole_Data uI_Hole_Data)
    {
        this.uI_Hole_Data = uI_Hole_Data;

        img.DOKill();
        img.gameObject.SetActive(true);
        img.color = gameManager.constantManager.noColor;
        img.DOColor(gameManager.constantManager.whiteColor, ConstantManager.ONE_HALF_TIME);

        img.material.SetVector("_HoleSize", uI_Hole_Data.sizes);
        img.material.SetVector("_HoleCenter", uI_Hole_Data.position);
    }

    public void Set_Off_Mask()
    {
        img.DOColor(gameManager.constantManager.noColor, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
        {
            img.gameObject.SetActive(false);
        });
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
