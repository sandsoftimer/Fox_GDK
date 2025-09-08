using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverReward : BaseGameBehaviour
{
    public GameObject view;
    public Animator anim;
    public List<Animator> stars;
    public Transform centerGlowHolder;
    public Button normalButton, claim2XButton;
    public TMP_Text successLevelText, regularAmountText, doubleAmountText;

    public ulong regularReward = 40, doubleReward = 80;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        view.gameObject.SetActive(false);

        normalButton.onClick.AddListener(OnPressNormalButton);
        claim2XButton.onClick.AddListener(OnClaim2X_ButtonPress);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        regularAmountText.text = $"{regularReward}";
        doubleAmountText.text = $"{doubleReward}";
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameOver()
    {
        base.OnGameOver();

        if (gameplayData.gameoverSuccess)
        {
            successLevelText.text = $"Level {gameplayData.currentLevelNumber}";
            FoxTools.functionManager.ExecuteAfterWaiting(ConstantManager.ONE_HALF_TIME, () =>
            {
                view.gameObject.SetActive(true);
                anim.SetBool("Hide", false);
            });
        }
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Show_Unlock_Item_Progression()
    {
        StartCoroutine(VibrateStars());
    }

    IEnumerator VibrateStars()
    {
        yield return new WaitForSeconds(0.2f);
        stars[0].SetTrigger("Animate");
        yield return new WaitForSeconds(0.2f);
        stars[1].SetTrigger("Animate");
        stars[2].SetTrigger("Animate");
    }

    public void OnPressNormalButton()
    {
        normalButton.gameObject.SetActive(false);
        claim2XButton.gameObject.SetActive(false);
        //gameplayData.previousLevelEaring = (int)regularReward;
        gameManager.currencyMainSystem.AddCurrency(regularReward, Vector3.zero, gameManager.gameCustomUI.transform, 5, 0.75f,
        () =>
        {
            OnTakenRewardAmount();
        });
    }

    public void OnClaim2X_ButtonPress()
    {
        gameManager.fox_AD_Controller.Show_Rewareded_AD("3x_coin", (bool success) =>
        {
            if (success)
            {
                normalButton.gameObject.SetActive(false);
                claim2XButton.gameObject.SetActive(false);
                //gameplayData.previousLevelEaring = (int)doubleReward;
                gameManager.currencyMainSystem.AddCurrency(doubleReward, Vector3.zero, gameManager.gameCustomUI.transform, 5, 0.75f,
                () =>
                {
                    OnTakenRewardAmount();
                });
            }

        });
    }

    public void OnTakenRewardAmount()
    {
        SaveGame();

        int index = gameManager.GetModedLevelNumber() + 1;
#if PUBLISHER_SDK_INSTALLED
        index++;
#endif
        FoxTools.sceneManager.LoadLevel(index);
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
