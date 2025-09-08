using UnityEngine;

public class PlayerController : BaseGameBehaviour
{
    public GameObject booste_3D_Prefab;

    internal Booste_3D_Controller booste_3D_Controller;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.playerController = this;

        booste_3D_Controller = Instantiate(booste_3D_Prefab).GetComponent<Booste_3D_Controller>();

        Registar_For_Input_Callbacks();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (gameState.Equals(GameState.GAME_INITIALIZED) && Input.GetMouseButtonDown(0))
        {
            gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            gameState = GameState.GAME_PLAY_STARTED;
        }

        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            gameManager.GameOver(true);
        }
#endif

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Initialize()
    {

    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
