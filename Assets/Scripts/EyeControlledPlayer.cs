using UnityEngine;

public class EyeControlledPlayer : BaseGameBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private EyeTracker eyeTracker;

    private Vector3 moveDirection;

    public override void Start()
    {
        base.Start();
        
        if (eyeTracker == null)
            eyeTracker = FindAnyObjectByType<EyeTracker>();

        if (eyeTracker != null)
        {
            eyeTracker.OnLookLeft.AddListener(MoveLeft);
            eyeTracker.OnLookRight.AddListener(MoveRight);
            eyeTracker.OnLookUp.AddListener(MoveUp);
            eyeTracker.OnLookDown.AddListener(MoveDown);
            eyeTracker.OnBlink.AddListener(Jump);
        }
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime * 2f);
    }

    private void MoveLeft() => moveDirection = Vector3.left;
    private void MoveRight() => moveDirection = Vector3.right;
    private void MoveUp() => moveDirection = Vector3.forward;
    private void MoveDown() => moveDirection = Vector3.back;
    private void Jump() => Debug.Log("Blink detected - Jump!");

    public override void OnDisable()
    {
        base.OnDisable();
        
        if (eyeTracker != null)
        {
            eyeTracker.OnLookLeft.RemoveListener(MoveLeft);
            eyeTracker.OnLookRight.RemoveListener(MoveRight);
            eyeTracker.OnLookUp.RemoveListener(MoveUp);
            eyeTracker.OnLookDown.RemoveListener(MoveDown);
            eyeTracker.OnBlink.RemoveListener(Jump);
        }
    }
}
