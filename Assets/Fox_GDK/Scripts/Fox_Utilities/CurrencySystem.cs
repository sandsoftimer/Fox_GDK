using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class CurrencySystem : FoxObject
{
    public string CURRENCY_NAME = "COIN_SYSTEM";
    public Image iconImageUI;
    public GameObject colletiveCurrencyPrefab;
    public float currencyCollectSpeed = 2f;
    public TMP_Text currencyUI;
    public TMP_Text currencyDeductionUI;
    public ulong startingCurrency = 100;

    public Action OnCurrencyUpdate;

    ulong totalCurrency;
    public ulong REMAINING_CURRENCY
    {
        get { return totalCurrency; }
    }

    Animator anim;
    IEnumerator runningCoroutine;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        if (PlayerPrefsX.GetBool(CURRENCY_NAME, true))
        {
            totalCurrency += startingCurrency;
            PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);
            PlayerPrefsX.SetBool(CURRENCY_NAME, false);
        }
        totalCurrency = PlayerPrefsX.GetuLong(CURRENCY_NAME, startingCurrency);
        currencyUI.text = CurrencyFormatter.FormattedRNACount(REMAINING_CURRENCY);

        anim = GetComponent<Animator>();

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

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void AddCurrency(ulong value, float animationDuration = 2)
    {
        IncreaseCurrency(value, animationDuration);
    }

    public void AddCurrency(ulong value, Vector3 worldSpacePoint, Transform uiParent, int uiCount, float animationDuration = 2, Action action = null)
    {
        StartCoroutine(CollectCurrencyIcon(value, worldSpacePoint, uiParent, uiCount, animationDuration, action));
    }

    IEnumerator CollectCurrencyIcon(ulong value, Vector3 worldSpacePoint, Transform uiParent, int uiCount, float animationDuration, Action action)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldSpacePoint);

        for (int i = 0; i < uiCount; i++)
        {
            int index = i;
            //GameObject go = FoxTools.poolManager.Instantiate(colletiveCurrencyPrefab, worldSpacePoint + FoxExtensions.FOXE_GetRandomDirection3D() * 200, Quaternion.identity, uiParent);
            GameObject go = FoxTools.poolManager.Instantiate(colletiveCurrencyPrefab, screenPoint, Quaternion.identity, uiParent);
            go.GetComponent<RectTransform>().position = screenPoint + FoxExtensions.FOXE_GetRandomDirection3D() * 200;
            go.transform.DOMove(iconImageUI.transform.position, animationDuration).OnComplete(() =>
            {
                Destroy(go);
                if (index == uiCount - 1)
                {
                    IncreaseCurrency(value, animationDuration);
                    action?.Invoke();
                }
            }).SetEase(Ease.InBack);
            yield return null;
        }
    }

    void IncreaseCurrency(ulong value, float animationDuration)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
        }
        runningCoroutine = CurrencyFormatter.CountUpAnimation(totalCurrency, totalCurrency + value, currencyUI, animationDuration);
        StartCoroutine(runningCoroutine);

        totalCurrency += value;
        PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);
        OnCurrencyUpdate?.Invoke();
    }

    public void DeductCurrency(ulong value, float animationDuration = 2)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
        }
        //currencyDeductionUI.text = "-" +CurrencyFormatter.FormattedRNACount(value);
        //anim.SetTrigger("Deduct");

        totalCurrency -= value;
        PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);
        OnCurrencyUpdate?.Invoke();

        currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

    public void ZoomInOut_Effect()
    {
        anim.SetTrigger("Zoom");
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}


public class CurrencyFormatter
{
    public static string FormattedRNACount(int rnaCount, int decimalPlaces = 2)
    {
        if (((long)rnaCount) >= 1000000000000000)
        {
            return $"{((double)rnaCount / 1000000000000000.0).ToString($"F{decimalPlaces}")}Q";
        }
        else if (((long)rnaCount) >= 1000000000000)
        {
            return $"{((double)rnaCount / 1000000000000.0).ToString($"F{decimalPlaces}")}T";
        }
        else if (rnaCount >= 1000000000)
        {
            return $"{((double)rnaCount / 1000000000.0).ToString($"F{decimalPlaces}")}B";
        }
        else if (rnaCount >= 1000000)
        {
            return $"{((double)rnaCount / 1000000.0).ToString($"F{decimalPlaces}")}M";
        }
        else if (rnaCount >= 1000)
        {
            return $"{((double)rnaCount / 1000.0).ToString($"F{decimalPlaces}")}K";
        }
        else
        {
            return rnaCount.ToString();
        }
    }

    public static string FormattedRNACount(ulong rnaCount, int decimalPlaces = 2)
    {
        if (rnaCount >= 1000000000000000)
        {
            return $"{((double)rnaCount / 1000000000000000.0).ToString($"F{decimalPlaces}")}Q";
        }
        else if (rnaCount >= 1000000000000)
        {
            return $"{((double)rnaCount / 1000000000000.0).ToString($"F{decimalPlaces}")}T";
        }
        else if (rnaCount >= 1000000000)
        {
            return $"{((double)rnaCount / 1000000000.0).ToString($"F{decimalPlaces}")}B";
        }
        else if (rnaCount >= 1000000)
        {
            return $"{((double)rnaCount / 1000000.0).ToString($"F{decimalPlaces}")}M";
        }
        else if (rnaCount >= 1000)
        {
            return $"{((double)rnaCount / 1000.0).ToString($"F{decimalPlaces}")}K";
        }
        else
        {
            return rnaCount.ToString();
        }
    }

    public static IEnumerator CountUpAnimation(int startVal, int endVal, TMP_Text textComponent, string preText, string postText, float duration, System.Action OnComplete = null)
    {
        int deltaVal = endVal - startVal;
        int perFrameUpdate = (int)(((float)deltaVal / (float)duration) * Time.deltaTime);
        if (perFrameUpdate < 1)
        {
            perFrameUpdate = 1;
            duration = Time.deltaTime / perFrameUpdate * deltaVal;
        }
        int currentVal = startVal;
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += perFrameUpdate;
            if (currentVal < endVal)
            {
                textComponent.text = $"{preText}{FormattedRNACount(currentVal)}{postText}";
            }
        }

        textComponent.text = $"{preText}{FormattedRNACount(endVal)}{postText}";

        OnComplete?.Invoke();
    }

    public static IEnumerator CountUpAnimation(ulong startVal, ulong endVal, TMP_Text textComponent, float duration, System.Action OnComplete = null)
    {
        ulong deltaVal = endVal - startVal;
        int perFrameUpdate = (int)(((double)deltaVal / (double)duration) * Time.deltaTime);
        if (perFrameUpdate < 1)
        {
            perFrameUpdate = 1;
            duration = Time.deltaTime / perFrameUpdate * deltaVal;
        }
        ulong currentVal = startVal;
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += (ulong)perFrameUpdate;
            if (currentVal < endVal)
            {
                textComponent.text = FormattedRNACount(currentVal);
            }
        }

        textComponent.text = FormattedRNACount(endVal);

        OnComplete?.Invoke();
    }

    public static IEnumerator CountUpTimeAnimation(int timeInSeconds, TMP_Text textComponent, float duration, System.Action OnComplete = null)
    {
        int perFrameUpdate = (int)(((float)timeInSeconds / duration) * Time.deltaTime);
        perFrameUpdate = perFrameUpdate >= 1 ? perFrameUpdate : 1;
        int currentVal = 0;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += perFrameUpdate;
            if (currentVal < timeInSeconds)
            {
                textComponent.text = FormattedTime(currentVal);
            }
        }

        textComponent.text = FormattedTime(timeInSeconds);

        OnComplete?.Invoke();
    }

    static string FormattedTime(int seconds)
    {
        int secs = seconds % 60;
        int mins = seconds / 60;
        int hrs = mins / 60;
        mins = mins % 60;

        return $"{hrs.ToString("00")}hrs : {mins.ToString("00")}mins : {secs.ToString("00")}secs";
    }
}
