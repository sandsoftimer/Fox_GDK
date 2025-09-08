using UnityEngine;
using UnityEngine.UI;

public class CoolDownBehaviour : FoxObject
{
    public Color initialColor = Color.red;
    public Color finalColor = Color.green;
    public Image coloredImage;

    public float coolDownTime = 5;
    [ReadOnlyProperty] public float fillProgress;

    float remaininigTime = 0;

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

        fillProgress = 1;
        remaininigTime = coolDownTime;
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
    }

    void FixedUpdate()
    {
        //if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
        //    return;

        coloredImage.color = Color.Lerp(finalColor, initialColor, fillProgress);
        coloredImage.fillAmount = fillProgress;
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public float Update_Progress()
    {
        remaininigTime -= Time.deltaTime;
        fillProgress = remaininigTime / coolDownTime;
        return fillProgress;
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
