using UnityEngine.UI;

public class Direct_Retry_View : BaseGameBehaviour
{
    public Button retryButton;
    public Button closeButton;
    public Button yesButton;
    public Popup_Controller popupController;

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

        //CMP.eventHandler += OnConsentsChangesEventListener;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        //CMP.eventHandler -= OnConsentsChangesEventListener;
    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        retryButton.onClick.AddListener(Show_View);
        closeButton.onClick.AddListener(OnCloseButtonPress);
        yesButton.onClick.AddListener(OnYesButtonPress);

        popupController.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameOver()
    {
        base.OnGameOver();

        popupController.gameObject.SetActive(false);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Show_View()
    {
        popupController.Show_Popup(null, "Retry Level", "Are You Sure ?", null);
    }

    public void OnYesButtonPress()
    {
        gameManager.OnDirectRetryYesButtonPress();
    }

    public void OnCloseButtonPress()
    {
        Hide_View();
    }

    public void Hide_View()
    {
        popupController.Hide_Popup();
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
