using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fox_Canvas_Animator : MonoBehaviour
{
    public RectTransform leftHolder;
    public RectTransform rightHolder;
    public RectTransform upHolder;
    public RectTransform downHolder;

    [Space]
    public Image centerBG;
    public Color centerBGColor = new Color32(0x35, 0x11, 0x11, 0xFA);
    public float transitionSpeed = ConstantManager.ONE_FORTH_TIME;
    public bool show = false;

    RectTransform canvas;
    Color hideColor;

    private void Awake()
    {
        canvas = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        if (centerBG)
        {
            hideColor = centerBGColor;
            hideColor.a = 0;
        }

        //StartCoroutine(Initialize());
    }

    void Start()
    {
        //yield return null;

        Set_Visual(show, 0);
    }

    public void Set_Visual(bool value, float animationSpeed = -1)
    {
        animationSpeed = animationSpeed == -1 ? transitionSpeed : animationSpeed;

        show = value;
        if (show)
        {
            leftHolder.transform.DOLocalMove(Vector2.zero, animationSpeed);
            rightHolder.transform.DOLocalMove(Vector2.zero, animationSpeed);
            upHolder.transform.DOLocalMove(Vector2.zero, animationSpeed);
            downHolder.transform.DOLocalMove(Vector2.zero, animationSpeed);

            if (centerBG)
            {
                centerBG.enabled = true;
                centerBG.DOColor(centerBGColor, animationSpeed);
            }
        }
        else
        {
            leftHolder.transform.DOLocalMove(new Vector2(-canvas.sizeDelta.x, 0), animationSpeed);
            rightHolder.transform.DOLocalMove(new Vector2(canvas.sizeDelta.x, 0), animationSpeed);
            upHolder.transform.DOLocalMove(new Vector2(0, canvas.sizeDelta.y), animationSpeed);
            downHolder.transform.DOLocalMove(new Vector2(0, -canvas.sizeDelta.y), animationSpeed);

            if (centerBG)
            {
                centerBG.DOColor(hideColor, animationSpeed).OnComplete(() => { centerBG.enabled = false; });
            }
        }
    }
}
