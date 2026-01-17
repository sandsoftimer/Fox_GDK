using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SaturationManager : BaseGameBehaviour
{
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    [Range(-100, 100)]
    public float target_Saturation = -85f; // 0 for black and white, 100 for normal saturation

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.saturationManager = this;
        volume = GetComponent<Volume>();

        // Try to get the Color Adjustments override from the volume profile
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            // Enable the saturation parameter to be controlled by the script
            colorAdjustments.saturation.overrideState = true;
        }
        volume.enabled = false;
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

        if (!gameplayData.gameoverSuccess)
        {
            volume.enabled = true;
            StartSaturation();
        }
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void StartSaturation()
    {
        float saturation = 0;
        DOVirtual.Float(saturation, target_Saturation, ConstantManager.DEFAULT_ANIMATION_TIME, (saturation) =>
        {
            SetSaturation(saturation);
        }).SetEase(Ease.InSine).OnComplete(() =>
        {
            gameManager.levelContinueView.Temp_Solution_For_This_Build_Olny();
        });
    }

    // Public method to set the saturation value (0 to 100)
    public void SetSaturation(float value)
    {
        if (colorAdjustments != null)
        {
            // The saturation value in Unity's Color Adjustments ranges from -100 to 100.
            colorAdjustments.saturation.value = value;
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
