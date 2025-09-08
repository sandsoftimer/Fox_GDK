using System;
using UnityEngine;

public class SqueezableObject : FoxObject
{
    public Transform vfx;
    public ParticleSystem placementEffect;
    public AudioClip effectSoundClip;

    [Space]
    public SqueezableData squeezableData;

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

    public void Play_Squeeze_Effect(Action action = null, bool playParticleOnFinish = true)
    {
        if (vfx != null)
        {
            Action _action = action;
            if (placementEffect != null)
            {
                if (playParticleOnFinish)
                    _action += () => placementEffect.Play();
                else
                    placementEffect.Play();
            }
            if (effectSoundClip != null)
            {
                //_action += () => gameManager.PlayThisSoundEffect(effectSoundClip);
            }
            vfx.localScale = Vector3.zero;
            vfx.SqueezeEffect(squeezableData, ConstantManager.ONE_HALF_TIME, _action);
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
