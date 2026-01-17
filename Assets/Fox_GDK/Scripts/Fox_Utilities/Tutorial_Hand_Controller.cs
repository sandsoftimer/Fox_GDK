using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial_Hand_Controller : MonoBehaviour
{
    public TutorialHandType tutorialHandType;
    public Transform handCursor;
    public Animator anim;
    public Transform hidingCanvas;

    Vector3 cursorPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hidingCanvas)
        {
            Cursor.visible = false;
            hidingCanvas.AddComponent<CanvasGroup>().alpha = 0;
        }


        switch (tutorialHandType)
        {
            case TutorialHandType.NONE:
                break;
            case TutorialHandType.AUTO_TAPPING:
                anim.SetTrigger("Tapping");
                break;
            case TutorialHandType.DOWN:
                anim.SetTrigger("Down");
                break;
            case TutorialHandType.UP:
            case TutorialHandType.FOLLOW_MOUSE:
                anim.SetTrigger("Up");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (tutorialHandType)
        {
            case TutorialHandType.NONE:
                break;
            case TutorialHandType.AUTO_TAPPING:
                break;
            case TutorialHandType.DOWN:
                break;
            case TutorialHandType.UP:
                break;
            case TutorialHandType.FOLLOW_MOUSE:
                if (Input.GetMouseButton(0))
                    anim.SetTrigger("Down");
                else
                    anim.SetTrigger("Up");
                break;
        }

    }

    private void FixedUpdate()
    {
        switch (tutorialHandType)
        {
            case TutorialHandType.NONE:
                break;
            case TutorialHandType.AUTO_TAPPING:
                break;
            case TutorialHandType.DOWN:
                break;
            case TutorialHandType.UP:
                break;
            case TutorialHandType.FOLLOW_MOUSE:
                handCursor.position = Vector3.Lerp(handCursor.position, cursorPosition, 0.5f);
                break;
        }
    }

    private void LateUpdate()
    {
        cursorPosition = Input.mousePosition;
    }

    public void Move_Cursor(Vector3 startScreenPosition, Vector3 endScreenPosition, float time)
    {
        handCursor.position = startScreenPosition;
        handCursor.gameObject.SetActive(true);
        anim.SetTrigger("Up");
        this.Delay_Call_After(ConstantManager.ONE_FORTH_TIME, () =>
        {
            anim.SetTrigger("Down");
            handCursor.DOMove(endScreenPosition, time).OnComplete(() =>
            {
                handCursor.gameObject.SetActive(false);
            });
        });
    }
}

public enum TutorialHandType
{
    NONE,
    AUTO_TAPPING,
    DOWN,
    UP,
    FOLLOW_MOUSE,
}