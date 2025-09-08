using System.Collections;
using UnityEngine;

public class Tutorial_View : BaseGameBehaviour
{
    public Transform tutorialHolder;

    [Space]
    public GameObject level_1_tutorial_Holder;
    public GameObject marging_tutorial_Holder;
    public GameObject hiddel_Holes_tutorial_Holder;

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


    bool alreadyUnlockedMagnet;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.tutorial_View = this;

        level_1_tutorial_Holder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL - 1);
        marging_tutorial_Holder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.MARGE_TUTORIAL_LEVEL - 1);
        hiddel_Holes_tutorial_Holder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.HIDDEN_HOLES_TUTORIAL_LEVEL - 1);

        magnetTutorialHolder.SetActive(false);
        hammerTutorialHolder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1);
        dockTutorialHolder.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER - 1);
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

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Start_Game_Or_Tutorial()
    {
        if (gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Level_1_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.MARGE_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Marging_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.HIDDEN_HOLES_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Hidden_Hole_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.SQURE_HOLES_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Squre_Hole_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.HIDDEN_BLOCK_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Hidden_Block_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.MULTISTORIED_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Multistored_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.FROZEN_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Frozen_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.TRIANGLE_HOLES_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Triangle_Holes_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.CONNECTED_HOLES_TUTORIAL_LEVEL - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Connected_Holes_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.DOCK_UNLOCK_LEVEL_NUMBER - 1)
        {
            //gameplayData.tutorial_Tap_Blocker = true;
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Dock_Booster_Tutorial());
        }
        else if (gameplayData.currentLevelNumber == gameManager.constantManager.HAMMER_UNLOCK_LEVEL_NUMBER - 1)
        {
            gameplayData.tutorial_Tap_Blocker = true;
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            StartCoroutine(Start_Hammer_Booster_Tutorial());
        }
        else
        {
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
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
        int currentIndex = 0;

    FETCH_NEXT:
        yield return null;
        for (int i = 0; i < level_1_tutorial_Holder.transform.childCount; i++)
        {
            level_1_tutorial_Holder.transform.GetChild(i).gameObject.SetActive(i == currentIndex);
        }
        if (currentIndex == 1)
        {
            gameManager.maskManager.Set_Off_Mask();
        }
        else
            gameManager.maskManager.Initialize_Next_Masking(currentIndex);

        yield return new WaitUntil(() => Get_Input(currentIndex));
        if (currentIndex == 1)
        {
            //BobbinManualReleaseController.ReleaseMarkedBobbin();
            //Debug.LogError("BobbinManualReleaseController.ReleaseMarkedBobbin");
        }
        if (currentIndex < level_1_tutorial_Holder.transform.childCount - 2)
        {
            currentIndex++;
            goto FETCH_NEXT;
        }

        level_1_tutorial_Holder.gameObject.SetActive(false);
        gameManager.maskManager.Set_Off_Mask();
        //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Marging_Tutorial()
    {
        int currentIndex = 0;

    FETCH_NEXT:
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(1);

        yield return new WaitUntil(() => Get_Input(currentIndex));
        if (currentIndex < 2)
        {
            currentIndex++;
            goto FETCH_NEXT;
        }

        marging_tutorial_Holder.gameObject.SetActive(false);
        gameManager.maskManager.Set_Off_Mask();
        //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Hidden_Hole_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(2);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        hiddel_Holes_tutorial_Holder.gameObject.SetActive(false);
        gameManager.maskManager.Set_Off_Mask();
        //gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Squre_Hole_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(3);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Hidden_Block_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(4);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Multistored_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(5);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Frozen_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(6);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Triangle_Holes_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(7);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    IEnumerator Start_Connected_Holes_Tutorial()
    {
        yield return null;
        gameManager.maskManager.Initialize_Next_Masking(8);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        gameManager.maskManager.Set_Off_Mask();
        gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
    }

    bool validTapHappend;
    bool Get_Input(int index)
    {
        bool result = false;

        result = validTapHappend;
        validTapHappend = false;

        return result;
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
