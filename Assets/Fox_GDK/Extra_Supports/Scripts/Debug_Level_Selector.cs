using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Level_Selector : BaseGameBehaviour
{
    public GameObject levelButtonPrefab;

    public Button debugButton;
    public Button closeButton;
    public Button levelStatsButton;
    public Button nextLevelButton, previousLevelButton;

    public GameObject debugView;
    public Transform contentHolder;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        Initialize();
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
    void Initialize()
    {
        debugButton.onClick.RemoveAllListeners();
        debugButton.onClick.AddListener(Show);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Hide);

        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(Load_Next_Level);

        previousLevelButton.onClick.RemoveAllListeners();
        previousLevelButton.onClick.AddListener(Load_Previous_Level);

        levelStatsButton.onClick.RemoveAllListeners();
        levelStatsButton.onClick.AddListener(Load_Level_Stats);

        for (int i = 0; i < gameManager.constantManager.TOTAL_GAME_LEVELS; i++)
        {
            GameObject go = Instantiate(levelButtonPrefab, contentHolder);
            go.name = $"{i + 1}";
            go.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{i + 1}";
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                int index = int.Parse(go.name);
                Load_Now(index - 1);
            });
        }

        debugView.SetActive(false);
    }

    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    void Load_Level_Stats()
    {
        //LevelStatDisplayPanel.Instance.Appear();
    }

    void Load_Now(int index)
    {
        gameplayData.currentLevelNumber = index;
        SaveGame();
#if PUBLISHER_SDK_INSTALLED
        index++;
#endif
        FoxTools.sceneManager.LoadLevel(index + 1);
    }

    private void Load_Previous_Level()
    {
        if (gameplayData.currentLevelNumber == 0)
            return;

        Load_Now(gameplayData.currentLevelNumber - 1);
    }

    private void Load_Next_Level()
    {
        if (gameplayData.currentLevelNumber > gameManager.constantManager.TOTAL_GAME_LEVELS - 2)
            return;

        Load_Now(gameplayData.currentLevelNumber + 1);
    }

    void Show()
    {
        gameManager.ChangeGameState(GameState.GAME_PLAY_PAUSED);
        debugView.SetActive(true);

    }

    void Hide()
    {
        gameManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
        debugView.SetActive(false);
    }


    #endregion ALL SELF DECLARE FUNCTIONS
}
