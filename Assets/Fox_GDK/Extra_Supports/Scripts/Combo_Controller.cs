using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo_Controller : BaseGameBehaviour
{
    public GameObject comboUIPrefab;
    public Image vfxImage;
    public float comboTime = 5f;
    public float fadingHeight = 20f;
    public float fadingTime = 2;


    public List<Sprite> comboFontVFX;

    public bool debugMood;
    int comboStreak = -1;
    float coolDown = 0;

    int index = -1;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        Reset_Streak();
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

        coolDown -= Time.deltaTime;
        if (coolDown < 0)
        {
            Reset_Streak();
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public void OnCompleteHoles()
    {
        coolDown = comboTime;

        comboStreak += debugMood ? 1 : 1;
        if (comboStreak > 1)
        {
            if (debugMood)
            {
                Combo_Happaned(gameManager.playerController.transform);
            }
            else
            {
            }
        }
    }


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    void Combo_Happaned(Transform target)
    {
        index++;
        if (index >= comboFontVFX.Count)
            return;

        vfxImage.transform.DOKill();
        vfxImage.sprite = comboFontVFX[index];
        vfxImage.gameObject.SetActive(true);

        vfxImage.transform.position = Camera.main.WorldToScreenPoint(target.position);
        vfxImage.gameObject.SetActive(true);

        float direction = fadingHeight * Random.Range(0, 2) == 0 ? 1 : -1f;
        //FoxTools.cameraManager.ShakeDefaultCamera();
        direction = fadingHeight;

        vfxImage.transform.eulerAngles = Random.Range(0, 2) == 0 ? new Vector3(0, 0, -45) : new Vector3(0, 0, 45);
        vfxImage.transform.DORotate(Vector3.zero, ConstantManager.ONE_FORTH_TIME).SetEase(Ease.OutBack).OnComplete(() =>
        {
            vfxImage.transform.DOMove(vfxImage.transform.position + new Vector3(0, direction, 0), fadingTime).OnComplete(() =>
            {
                vfxImage.gameObject.SetActive(false);
            });
        });
    }

    private void Reset_Streak()
    {
        comboStreak = 0;
        index = -1;
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
