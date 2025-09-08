using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : FoxObject
{
    public Transform visualsHolder;
    public Transform damageTextHolder;
    public Image healthAcctualBar, decreaseAnimBar;
    public GameObject damageTextPrefab;
    public float healthMaxValue = 100f;
    public float fillAnimationTime = 1;
    public bool hideBarOnDie = true;
    public bool showDamageText = false;

    public float health = 100f;
    IEnumerator fillAnim = null;

    public float GET_CURRENT_HEALTH
    {
        get { return health; }
    }

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        fillAnim = Fill_Animation();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ResetHealth(healthMaxValue);
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

    void FixedUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void SetActiveUI(bool value)
    {
        visualsHolder.gameObject.SetActive(value);
        healthAcctualBar.gameObject.SetActive(value);
    }

    public float Damage(float value)
    {
        health = Mathf.Clamp(health - Mathf.Abs(value), 0, health);
        if (showDamageText)
        {
            GameObject go = FoxTools.poolManager.Instantiate(damageTextPrefab, Vector3.zero, Quaternion.identity, damageTextHolder);
            TMP_Text damageText = go.transform.GetChild(0).GetComponent<TMP_Text>();
            damageText.text = $"-{Mathf.Abs(value)}";
            FoxTools.poolManager.Destroy(go, 2);
        }

        UpdateVisuals();
        return health;
    }

    public void Gain(float value)
    {
        health = Mathf.Clamp(health + Mathf.Abs(value), 0, healthMaxValue);
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (healthAcctualBar != null)
        {
            healthAcctualBar.fillAmount = Mathf.InverseLerp(0, healthMaxValue, health);
            healthAcctualBar.gameObject.SetActive(health > 0);
            visualsHolder.gameObject.SetActive(visualsHolder != null && !(health == 0 && hideBarOnDie));
        }

        if(fillAnim != null)
            StopCoroutine(fillAnim);
        fillAnim = Fill_Animation();
        StartCoroutine(fillAnim);
        //Debug.LogError($"Start {fillAnim == null}");
    }

    public void ResetHealth(float healthPickValue)
    {
        this.healthMaxValue = healthPickValue;
        health = healthPickValue;
        healthAcctualBar.fillAmount = Mathf.InverseLerp(0, healthMaxValue, health);
        decreaseAnimBar.fillAmount = healthAcctualBar.fillAmount;
        UpdateVisuals();
    }

    IEnumerator Fill_Animation()
    {
        if (decreaseAnimBar.fillAmount > healthAcctualBar.fillAmount)
        {
            yield return new WaitForSeconds(fillAnimationTime);
            float decreaseValue = Time.deltaTime;
            while (decreaseAnimBar.fillAmount > healthAcctualBar.fillAmount)
            {
                decreaseAnimBar.fillAmount -= decreaseValue;
                yield return null;
            }
        }
        decreaseAnimBar.fillAmount = healthAcctualBar.fillAmount;
        //Debug.LogError($"{decreaseAnimBar.fillAmount} == {healthAcctualBar.fillAmount}");
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
