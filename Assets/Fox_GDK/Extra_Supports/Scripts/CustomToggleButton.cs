using UnityEngine;
using UnityEngine.UI;

public class CustomToggleButton : BaseGameBehaviour
{
    public CustomToggleButton_Type type;
    public Image mainImage, iconImage;
    public Sprite activeSprite, activeIcon;
    public Sprite deactiveSprite, deactiveIcon;

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

        Initialize();
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

    void Initialize()
    {
        if (type.Equals(CustomToggleButton_Type.SOUND))
        {
            if (gameplayData.soundEffectOn)
            {
                mainImage.sprite = activeSprite;
                iconImage.sprite = activeIcon;
                AudioListener.volume = 1;
            }
            else
            {
                mainImage.sprite = deactiveSprite;
                iconImage.sprite = deactiveIcon;
                AudioListener.volume = 0;
            }
        }
        else if (type.Equals(CustomToggleButton_Type.VIBRATION))
        {
            if (gameplayData.vibrationEffectOn)
            {
                mainImage.sprite = activeSprite;
                iconImage.sprite = activeIcon;
            }
            else
            {
                mainImage.sprite = deactiveSprite;
                iconImage.sprite = deactiveIcon;
            }
        }
    }

    public void OnButtonPress()
    {
        if (type.Equals(CustomToggleButton_Type.SOUND))
        {
            if (gameplayData.soundEffectOn)
            {
                mainImage.sprite = deactiveSprite;
                iconImage.sprite = deactiveIcon;
                AudioListener.volume = 0;
            }
            else
            {
                mainImage.sprite = activeSprite;
                iconImage.sprite = activeIcon;
                AudioListener.volume = 1;
            }
            gameplayData.soundEffectOn = !gameplayData.soundEffectOn;
        }
        else if (type.Equals(CustomToggleButton_Type.VIBRATION))
        {
            if (gameplayData.vibrationEffectOn)
            {
                mainImage.sprite = deactiveSprite;
                iconImage.sprite = deactiveIcon;
            }
            else
            {
                mainImage.sprite = activeSprite;
                iconImage.sprite = activeIcon;
            }
            gameplayData.vibrationEffectOn = !gameplayData.vibrationEffectOn;
        }
        SaveGame();
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
