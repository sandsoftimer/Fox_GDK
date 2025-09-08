using UnityEngine;
using UnityEngine.UI;

public class Button_Tap_Sound : BaseGameBehaviour
{
    public AudioSource sound;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        if (sound)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                sound.Play();
            });
        }
        else
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (gameManager.ButtonTapSound)
                    gameManager.ButtonTapSound.Play();
            });
        }
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


    #endregion ALL SELF DECLARE FUNCTIONS
}
