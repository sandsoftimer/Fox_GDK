using Unity.VisualScripting;
using UnityEngine;

public class Tutorial_Hand_Tapping : MonoBehaviour
{
    public Transform handCursor;
    public Animator anim;
    public Transform hidingCanvas;
    Vector3 cursourPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        hidingCanvas.AddComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Is_Finger_Down", Input.GetMouseButton(0));
    }

    private void FixedUpdate()
    {
        handCursor.position = Vector3.Lerp(handCursor.position, cursourPosition, 0.5f);
    }

    private void LateUpdate()
    {
        cursourPosition = Input.mousePosition;
    }
}
