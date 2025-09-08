using System;
using UnityEngine;

public class OffScreenHandler : FoxObject
{
    public Camera mainCam;
    public Vector2 offset;
    public Vector2 iconOffset = new Vector2(50f, 50f);
    public bool rotateIcon;
    public GameObject[] offScreenDisables;


    [Space(10)]
    public bool showOffScreenIndicator;
    public GameObject offScreenUIIndicatorPrefab;
    public Transform canvasParent;

    GameObject offScreenIndicator;
    bool isOffScreen;

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

        if (mainCam == null)
            mainCam = Camera.main;
    }

    public override void OnDisable()
    {
        //showOffScreenIndicator = false;

        if (offScreenIndicator)
            FoxTools.poolManager.Destroy(offScreenIndicator);

        offScreenIndicator = null;
    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (mainCam == null)
            mainCam = Camera.main;

        if (canvasParent == null)
            canvasParent = FoxTools.canvasManager.canvas.transform;
    }

    void FixedUpdate()
    {
        //if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
        //    return;

        isOffScreen = transform.FOXE_IsOffScreen(mainCam, offset);

        for (int i = 0; i < offScreenDisables.Length; i++)
        {
            offScreenDisables[i].SetActive(!isOffScreen);
        }

        if (offScreenUIIndicatorPrefab != null && showOffScreenIndicator)
        {
            if (isOffScreen)
            {
                if (offScreenIndicator == null)
                {
                    offScreenIndicator = FoxTools.poolManager.Instantiate(offScreenUIIndicatorPrefab, Vector2.zero, Quaternion.identity, canvasParent);
                    offScreenIndicator.transform.localScale = Vector3.one;
                }
                Vector2 point;
                point = transform.FOXE_OffScreenIndicatorPoint(mainCam, iconOffset);
                offScreenIndicator.transform.position = point;

                if (rotateIcon)
                {
                    Vector2 objectScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                    double angle = Math.Atan2(objectScreenPoint.y - point.y, objectScreenPoint.x - point.x);
                    offScreenIndicator.transform.rotation = Quaternion.Euler(0, 0, (float)angle * Mathf.Rad2Deg);
                }
            }
            else
            {
                if (offScreenIndicator != null)
                {
                    FoxTools.poolManager.Destroy(offScreenIndicator);
                    offScreenIndicator = null;
                }
            }
        }
        else
        {
            if (offScreenIndicator != null)
            {
                FoxTools.poolManager.Destroy(offScreenIndicator);
                offScreenIndicator = null;
            }
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    //public void Dispose()
    //{
    //    showOffScreenIndicator = false;

    //    if (offScreenIndicator)
    //        FoxTools.poolManager.Destroy(offScreenIndicator);

    //    offScreenIndicator = null;
    //    Destroy(this);
    //}

    #endregion ALL SELF DECLARE FUNCTIONS

}
