using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hard_Level_Visuals : BaseGameBehaviour
{
    public GameObject miniSkullView, fullSkullView;
    public Transform initialPos, activePos, endPos;
    public Image background, bigSkull, miniSkull;
    public TMP_Text hardLevelBigText, hardLevelSmallText;
    public Color zeroColor, viewColor;
    public float stayTime = 2;


    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        miniSkullView.SetActive(false);
        fullSkullView.SetActive(false);

        gameManager.hardware_level_visuals = this;
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

    public override void OnGameOver()
    {
        base.OnGameOver();

        miniSkullView.SetActive(false);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Show_Hard_Level_Alarm(Action action = null)
    {
        StartCoroutine(Show_Alarm(action));
    }

    IEnumerator Show_Alarm(Action action = null)
    {
        fullSkullView.SetActive(true);
        bigSkull.transform.localScale = Vector3.zero;
        background.color = zeroColor;
        hardLevelBigText.transform.position = initialPos.position;

        background.DOColor(viewColor, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
        {
            bigSkull.transform.DOScale(Vector3.one, ConstantManager.ONE_FORTH_TIME);
            hardLevelBigText.transform.DOMove(activePos.position, ConstantManager.ONE_FORTH_TIME);
        });
        yield return new WaitForSeconds(stayTime);
        background.DOColor(zeroColor, ConstantManager.ONE_FORTH_TIME);
        bigSkull.transform.DOMove(miniSkull.transform.position, ConstantManager.ONE_FORTH_TIME).SetEase(Ease.InBack).SetEase(Ease.InSine);
        bigSkull.rectTransform.DOSizeDelta(miniSkull.rectTransform.sizeDelta, ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
        {
            action?.Invoke();
            fullSkullView.SetActive(false);
            miniSkullView.SetActive(true);
        });

        hardLevelBigText.rectTransform.DOMove(hardLevelSmallText.rectTransform.position, ConstantManager.ONE_FORTH_TIME).SetEase(Ease.InBack).SetEase(Ease.InSine);
        hardLevelBigText.rectTransform.DOSizeDelta(hardLevelSmallText.rectTransform.sizeDelta, ConstantManager.ONE_FORTH_TIME);
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
