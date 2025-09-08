using Com.FunFox.Utility;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FooterController : BaseGameBehaviour
{
    public List<GameObject> panels;
    public Transform footerHolder;
    public Transform footerLayoutBG;
    public Transform tabPanelHolder;
    public Transform pointer;

    GameObject activeBG;
    List<Transform> tabs;
    RectTransform canvasRect;

    float scale = 1.5f;
    int currentIndex = 1;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(true);
        }
        Registar_For_Input_Callbacks();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        canvasRect = gameManager.GetComponent<RectTransform>();

        tabs = new();
        for (int i = 0; i < footerLayoutBG.childCount; i++)
        {
            Transform t = footerLayoutBG.GetChild(i).GetChild(0);
            tabs.Add(t);

            switch (i)
            {
                case 0:
                    tabPanelHolder.GetChild(0).localPosition = new Vector3(-canvasRect.sizeDelta.x, 0, 0);
                    break;
                case 1:
                    tabPanelHolder.GetChild(1).localPosition = Vector3.zero;
                    break;
                case 2:
                    tabPanelHolder.GetChild(2).localPosition = new Vector3(canvasRect.sizeDelta.x, 0, 0);
                    break;
            }
        }
        FoxTools.functionManager.ExecuteAfterWaiting(0.1f, () => OnPressTab(1));
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnSwipeLeft()
    {
        base.OnSwipeLeft();

        OnPressTab(Mathf.Clamp(currentIndex + 1, 0, footerLayoutBG.childCount));
    }

    public override void OnSwipeRight()
    {
        base.OnSwipeRight();

        OnPressTab(Mathf.Clamp(currentIndex - 1, 0, footerLayoutBG.childCount));
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void OnPressTab(int value)
    {
        if (gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        currentIndex = value;
        for (int i = 0; i < footerLayoutBG.childCount; i++)
        {
            if (i == value)
            {
                tabs[i].DOScale(Vector3.one * scale, ConstantManager.ONE_FORTH_TIME);
                pointer.DOMove(tabs[i].position, ConstantManager.ONE_FORTH_TIME);
                tabs[i].GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                tabs[i].DOScale(Vector3.one, ConstantManager.ONE_FORTH_TIME);
                tabs[i].GetChild(0).gameObject.SetActive(false);
            }
        }
        switch (value)
        {
            case 0:
                tabPanelHolder.DOLocalMove(new Vector3(canvasRect.sizeDelta.x, 0, 0), ConstantManager.ONE_FORTH_TIME);
                break;
            case 1:
                tabPanelHolder.DOLocalMove(new Vector3(0, 0, 0), ConstantManager.ONE_FORTH_TIME);
                break;
            case 2:
                tabPanelHolder.DOLocalMove(new Vector3(-canvasRect.sizeDelta.x, 0, 0), ConstantManager.ONE_FORTH_TIME);
                break;
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
