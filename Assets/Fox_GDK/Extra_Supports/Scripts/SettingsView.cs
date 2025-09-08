using UnityEngine;
using UnityEngine.UI;

public class SettingsView : BaseGameBehaviour
{
    public Popup_Controller popupController;
    public Image viewBackground;
    public Button settingsButton;
    public Button resumeButton;
    public Button quitButton;
    public GameObject privacySettingsObject;
    public GameObject preGameSettings, postGameSettings;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        settingsButton.onClick.AddListener(ShowSettingsView);
        resumeButton.onClick.AddListener(OnResumeButtonPress);
        quitButton.onClick.AddListener(OnQuitButtonPress);

        viewBackground.gameObject.SetActive(false);
        //settingsButton.gameObject.SetActive(gameplayData.currentLevelNumber != 0);
        //if (!CheckGDPRCountry.CheckCountryForGDPR())
        //{
        //    privacySettingsObject.SetActive(false);
        //}
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

        viewBackground.gameObject.SetActive(false);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void ConsentsButtonPressed()
    {
    }

    public void ShowSettingsView()
    {
        if (gameState.Equals(GameState.NONE))
        {
            preGameSettings.SetActive(true);
            postGameSettings.SetActive(false);
        }
        else
        {
            preGameSettings.SetActive(false);
            postGameSettings.SetActive(true);
        }
        popupController.Show_Popup(null, "Settings", "", null);
    }

    public void OnQuitButtonPress()
    {
        gameManager.ReloadLevel();
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    void OnResumeButtonPress()
    {
        popupController.Hide_Popup();
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
