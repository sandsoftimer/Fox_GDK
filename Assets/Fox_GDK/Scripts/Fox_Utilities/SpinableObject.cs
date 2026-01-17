using System.Collections.Generic;
using UnityEngine;

public class SpinableObject : BaseGameBehaviour
{
    public List<Transform> spinableVFXs;
    public Vector3 spinSpeed;
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
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    private void FixedUpdate()
    {
        for (int i = 0; i < spinableVFXs.Count; i++)
        {
            spinableVFXs[i].Rotate(Time.fixedDeltaTime * spinSpeed.x, Time.fixedDeltaTime * spinSpeed.y, Time.fixedDeltaTime * spinSpeed.z);
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS

}
