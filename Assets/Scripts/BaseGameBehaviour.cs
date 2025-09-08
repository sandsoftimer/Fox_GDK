public class BaseGameBehaviour : FoxObject
{
    GameManager _gameManager;
    public GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = FindAnyObjectByType<GameManager>();
            return _gameManager;
        }
    }

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

    }

    public override void OnDisable()
    {
        base.OnDisable();

    }


    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        _gameManager = foxManager as GameManager;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS
}
