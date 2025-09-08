using UnityEngine;

public class GameManager : FoxManager
{

    public override void Awake()
    {
        base.Awake();

        gameplayData.Is_Game_Busy_By_Booster = false;
        gameplayData.Is_Magnet_Tap_Mood = false;
        gameplayData.Is_Hammer_Tap_Mood = false;

        switch (constantManager.SCENE_LOOPING_TYPE)
        {
            case Fox_SceneLooping_Type.OBJECT_LOOPING:
                gameplayData.current_Level_Data = Resources.Load<Level_Data>($"Level_{GetModedLevelNumber() + 1}");
                break;
            case Fox_SceneLooping_Type.SCENE_LOOPING:
                break;
        }
        gameCustomUI.Set_Visual(true);
    }

    public override void Start()
    {
        base.Start();

        playerController.Initialize();

        tutorial_View.Start_Game_Or_Tutorial();
    }

    public override void GameStart()
    {
        base.GameStart();

        gameStartingUI.Set_Visual(false);
        gamePlayUI.Set_Visual(true);
        allButtonsUI.Set_Visual(true);
    }

    public override void GameOver()
    {
        base.GameOver();

        if (gameplayData.gameoverSuccess)
        {
            gamePlayUI.Set_Visual(false);
            gameCustomUI.Set_Visual(false);
            gameSuccessUI.Set_Visual(true);
        }
        else
        {
            gameFaildUI.Set_Visual(true);
        }
    }
}
