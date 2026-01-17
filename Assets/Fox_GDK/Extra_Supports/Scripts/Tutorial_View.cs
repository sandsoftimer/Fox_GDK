using System.Collections;
using UnityEngine;

public class Tutorial_View : BaseGameBehaviour
{
    public Transform tutorialHolder;

    [Space]
    public GameObject level_1_tutorial_Holder;
    public GameObject marging_tutorial_Holder;

    [Space]
    public GameObject boosterTurotialShadow;

    [Space]
    public GameObject magnetTutorialHolder;
    public GameObject magnetBooster;

    [Space]
    public GameObject hammerTutorialHolder;
    public GameObject hammerBooster;

    [Space]
    public GameObject dockTutorialHolder;
    public GameObject dockBooster;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.tutorial_View = this;

        level_1_tutorial_Holder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL - 1);
        marging_tutorial_Holder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.MARGE_TUTORIAL_LEVEL - 1);

        magnetTutorialHolder.SetActive(false);
        hammerTutorialHolder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1);
        dockTutorialHolder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER - 1);

        gameObject.SetActive(Check_If_Tutorial_Needed());
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

        gameManager.gamePlayUI.Set_Visual(false);
        gameManager.allButtonsUI.Set_Visual(false);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
            case Booster_Type.MAGNET:
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                boosterTurotialShadow.SetActive(false);
                gameManager.maskManager.Set_Off_Mask();
                break;
        }
        SaveGame();
        gameplayData.tutorial_Tap_Blocker = false;
        tutorialHolder.gameObject.SetActive(false);
    }

    public override void OnBoosterPreimplemented(Booster_Item booster_Item)
    {
        base.OnBoosterPreimplemented(booster_Item);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
                break;
            case Booster_Type.MAGNET:
                break;
            case Booster_Type.GOAL:
                break;
            case Booster_Type.DOCK:
                break;
        }
    }

    public override void OnBoosterImplemented(Booster_Item booster_Item)
    {
        base.OnBoosterImplemented(booster_Item);

        gameManager.gamePlayUI.Set_Visual(true);
        gameManager.allButtonsUI.Set_Visual(true);
    }

    public override void OnBoosterCanceled(Booster_Item booster_Item)
    {
        base.OnBoosterCanceled(booster_Item);

        gameManager.gamePlayUI.Set_Visual(true);
        gameManager.allButtonsUI.Set_Visual(true);
    }

    public override void OnGameOver()
    {
        base.OnGameOver();

        gameObject.SetActive(false);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    bool Check_If_Tutorial_Needed()
    {
        return false;
    }

    public void Start_Game_Or_Tutorial()
    {
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
        if (gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL - 1)
        {
            StartCoroutine(Start_Level_1_Tutorial());
        }
    }

    IEnumerator Start_Dock_Booster_Tutorial()
    {
        yield return null;

        dockTutorialHolder.SetActive(true);

        dockBooster.transform.SetSiblingIndex(magnetBooster.transform.parent.childCount - 2);
        boosterTurotialShadow.transform.SetParent(magnetBooster.transform.parent);
        boosterTurotialShadow.transform.SetSiblingIndex(magnetBooster.transform.parent.childCount - 3);
        dockTutorialHolder.SetActive(true);
        boosterTurotialShadow.SetActive(true);

        gameManager.gamePlayUI.Set_Visual(false);
    }

    IEnumerator Start_Magnet_Booster_Tutorial()
    {
        yield return new WaitForSeconds(ConstantManager.ONE_HALF_TIME);

        magnetBooster.transform.SetSiblingIndex(magnetBooster.transform.parent.childCount - 2);
        boosterTurotialShadow.transform.SetParent(magnetBooster.transform.parent);
        boosterTurotialShadow.transform.SetSiblingIndex(magnetBooster.transform.parent.childCount - 3);
        magnetTutorialHolder.SetActive(true);
        boosterTurotialShadow.SetActive(true);

        gameManager.gamePlayUI.Set_Visual(false);
    }

    IEnumerator Start_Hammer_Booster_Tutorial()
    {
        yield return null;

        hammerBooster.transform.SetSiblingIndex(hammerBooster.transform.parent.childCount - 2);
        boosterTurotialShadow.transform.SetParent(hammerBooster.transform.parent);
        boosterTurotialShadow.transform.SetSiblingIndex(hammerBooster.transform.parent.childCount - 3);
        hammerTutorialHolder.transform.GetChild(0).gameObject.SetActive(true);
        boosterTurotialShadow.SetActive(true);
    }

    IEnumerator Start_Level_1_Tutorial()
    {
        level_1_tutorial_Holder.transform.FOXE_ActiveChild(-1);
        gameManager.maskManager.Set_Off_Mask();
        int currentIndex = 0;

        yield return new WaitForSeconds(ConstantManager.DEFAULT_ANIMATION_TIME);

    FETCH_NEXT:
        UI_Hole_Data uI_Hole_Data = level_1_tutorial_Holder.transform.FOXE_ActiveChild(currentIndex).GetComponent<UI_Hole_Data>();
        uI_Hole_Data.transform.localScale = new Vector3(0, 1, 1);

        if (currentIndex != 1)
            yield return new WaitForSeconds(ConstantManager.DEFAULT_ANIMATION_TIME + ConstantManager.ONE_FORTH_TIME * currentIndex + 1);

        gameManager.maskManager.Initialize_Next_Masking(uI_Hole_Data);

        currentIndex++;
        if (currentIndex < level_1_tutorial_Holder.transform.childCount)
        {
            goto FETCH_NEXT;
        }

        level_1_tutorial_Holder.SetActive(false);
        gameManager.maskManager.Set_Off_Mask();
        gameObject.SetActive(false);
    }

    bool collectionComplete;
    bool Get_Collection_Complete()
    {
        bool result = false;

        if (collectionComplete)
        {
            result = true;
            collectionComplete = false;
        }

        return result;
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
