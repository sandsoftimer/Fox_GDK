using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Luggage_PopupController : BaseGameBehaviour
{
    public Color zeroColor = Color.white;
    public Color viewColor = Color.white;

    public GameObject popUpBody;
    public Transform titleBg;
    public Image icon;
    public Transform buttonsHolder;
    public Transform closeButton;
    public TMP_Text bodyInfoText, purchasePriceText, titleText;

    Image backgroundImage;
    Vector3 titleBgPosition = Vector3.zero;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        backgroundImage = GetComponent<Image>();
        titleBgPosition = titleBg.localPosition;
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

    public void Instantly_Show_Popup()
    {
        gameObject.SetActive(true);
    }

    public void Instantly_Hide_Popup()
    {
        gameObject.SetActive(false);
    }

    public void Show_Popup(Sprite icon, string title, string bodyText, string purchasePrice, Action action = null)
    {
        gameManager.ChangeGameState(GameState.GAME_PLAY_PAUSED);
        gameObject.SetActive(true);

        if (this.icon)
            this.icon.sprite = icon;
        if (titleText)
            titleText.text = title;
        if (bodyInfoText)
            bodyInfoText.text = bodyText;
        if (purchasePriceText)
            purchasePriceText.text = purchasePrice;

        StartCoroutine(Show_Center_Panel(action));
    }

    public void Hide_Popup()
    {
        StartCoroutine(Hide_Popup_Panel());
    }

    IEnumerator Hide_Popup_Panel()
    {
        backgroundImage.DOColor(zeroColor, ConstantManager.ONE_FORTH_TIME);
        popUpBody.transform.DOScale(Vector3.zero, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
        {
            gameManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
            gameObject.SetActive(false);
        });
        yield return null;
    }

    IEnumerator Show_Center_Panel(Action action = null)
    {
        backgroundImage.color = zeroColor;
        backgroundImage.DOColor(viewColor, ConstantManager.ONE_FORTH_TIME);

        popUpBody.transform.localScale = Vector3.one;
        popUpBody.gameObject.SetActive(true);

        if (closeButton)
            closeButton.localScale = Vector3.zero;

        if (icon)
        {
            icon.transform.localScale = Vector3.zero;
            icon.transform.eulerAngles = new Vector3(0, 0, 45f);
        }
        buttonsHolder.transform.localScale = Vector3.zero;

        titleBg.transform.localPosition = titleBgPosition.FOXE_ModifyThisVector(0, 50, 0);
        titleBg.transform.DOLocalMove(titleBgPosition, ConstantManager.ONE_FORTH_TIME).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.1f);

        if (icon)
        {
            icon.transform.DORotate(Vector3.one, ConstantManager.ONE_HALF_TIME).SetEase(Ease.OutBack);
            icon.transform.DOScale(Vector3.one, ConstantManager.ONE_HALF_TIME).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.1f);
        }
        buttonsHolder.transform.DOScale(Vector3.one, ConstantManager.ONE_HALF_TIME).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if (closeButton)
            {
                closeButton.transform.DOScale(Vector3.one, ConstantManager.ONE_HALF_TIME).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    action?.Invoke();
                });
            }
            else
            {
                action?.Invoke();
            }
        });
    }


    #endregion ALL SELF DECLARE FUNCTIONS
}
