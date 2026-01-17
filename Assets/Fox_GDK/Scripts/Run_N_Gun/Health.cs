using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : FoxObject
{
    public Action OnFinishHealth;
    public Action<float, Bullet> OnLooseHealth;
    public Action<float> OnGainHealth;

    public Transform visualsHolder;
    public Transform damageTextHolder;
    public Image healthAcctualBar;
    public TMP_Text healthAcctualText;
    public Image decreaseAnimBar;
    public GameObject damageTextPrefab;
    public float healthMaxValue = 100f;
    public float fillAnimationTime = 1;
    public bool hideBarInitially = false, hideBarOnDie = true;
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

        OnGainHealth += Gain;
        OnLooseHealth += Damage;
    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        fillAnim = Fill_Animation();
        ResetHealth(healthMaxValue);
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

    public void Damage(float value, Bullet bullet)
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
        if (health == 0)
            OnFinishHealth?.Invoke();

        //return health;
    }

    public void Gain(float value)
    {
        health = Mathf.Clamp(health + Mathf.Abs(value), 0, healthMaxValue);
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (visualsHolder == null)
            return;

        //if (hideBarInitially && health == healthMaxValue)
        //{
        //    visualsHolder.gameObject.SetActive(false);
        //    return;
        //}

        if (healthAcctualBar != null)
        {
            healthAcctualBar.fillAmount = Mathf.InverseLerp(0, healthMaxValue, health);
            healthAcctualBar.gameObject.SetActive(health > 0);
            visualsHolder.gameObject.SetActive(visualsHolder != null && !(health == 0 && hideBarOnDie));
        }
        if (healthAcctualText) healthAcctualText.text = $"{Mathf.Ceil(health)}";

        if (decreaseAnimBar)
        {
            if (fillAnim != null)
                StopCoroutine(fillAnim);
            fillAnim = Fill_Animation();
            StartCoroutine(fillAnim);
        }
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

    public void ResetHealth(float healthPickValue)
    {
        this.healthMaxValue = healthPickValue;
        health = healthPickValue;
        if (healthAcctualBar)
            healthAcctualBar.fillAmount = Mathf.InverseLerp(0, healthMaxValue, health);
        if (decreaseAnimBar)
            decreaseAnimBar.fillAmount = healthAcctualBar.fillAmount;
        UpdateVisuals();
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
