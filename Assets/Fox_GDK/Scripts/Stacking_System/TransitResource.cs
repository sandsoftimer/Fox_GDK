using DG.Tweening;
using System;
using UnityEngine;

public class TransitResource : FoxObject
{
    [Range(0, 10)]
    public float jumpPower = 1f;
    public float transitionTime = ConstantManager.ONE_HALF_TIME;

    //public float jumpPowerAssigned = 3f;
    //float bigJumpPower = 5;
    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();
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

    public void TransitThisResource(Transform resource, Vector3 moveTo, Vector3 rotateTo, bool vanishAfterTransit, Action actionAfterTransition = null)
    {
        //DOTween.Kill(resource.GetInstanceID());
        resource.DOKill(true);

        resource.DORotate(rotateTo, transitionTime);
        resource.DOJump(moveTo, jumpPower, 1, transitionTime).OnComplete(() =>
        {

            actionAfterTransition?.Invoke();
            if (vanishAfterTransit)
            {
                FoxTools.poolManager.Destroy(resource.gameObject);
            }
        }).SetEase(Ease.OutQuad);
    }

    public void TransitThisResource(Transform resource, ParabolaMove parabolaMove)
    {
        //DOTween.Kill(resource.GetInstanceID());
        resource.DOKill(true);
        parabolaMove.Initialized();
    }


    #endregion ALL SELF DECLARE FUNCTIONS

}
