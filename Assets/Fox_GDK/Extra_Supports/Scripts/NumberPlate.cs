using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPlate : BaseGameBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image background, difficulty;
    public TMP_Text numberText;
    public float speed = 4;

    [ReadOnlyProperty] public int levelNumber;

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


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public virtual void Initialize(int levelNumber)
    {
        if (levelNumber == -1)
            return;

        this.levelNumber = levelNumber;

        numberText.text = $"{levelNumber + 1}";

        Level_Data level_Data = Resources.Load($"Level_{(levelNumber % gameManager.constantManager.TOTAL_GAME_LEVELS) + 1}") as Level_Data;
        difficulty.enabled = level_Data.difficulty_Type.Equals(Difficulty_Type.HARD);

        if (levelNumber < gameplayData.currentLevelNumber)
        {
            background.sprite = sprites[(int)level_Data.difficulty_Type];
            background.fillAmount = 1;
        }
        else if (levelNumber == gameplayData.currentLevelNumber)
        {
            background.sprite = sprites[(int)level_Data.difficulty_Type];
        }
    }

    public IEnumerator Unlock_Effect()
    {
        float fillAmout = 0;
        while (fillAmout < 1)
        {
            yield return null;
            fillAmout += Time.deltaTime * speed;
            background.fillAmount = fillAmout;
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
