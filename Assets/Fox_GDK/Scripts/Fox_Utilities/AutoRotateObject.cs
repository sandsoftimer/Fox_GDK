using UnityEngine;

public class AutoRotateObject : BaseGameBehaviour
{
    public Transform objectHolder;
    public Transform autoRotationHolder;
    public float rotateThreshold = 3.5f;
    public float autoRotateSpeed = 10f;

    public float rotationSpeed = 5f;
    public float smoothTime = 0.2f;

    private Vector2 currentVelocity;
    private Vector2 currentRotation;
    private Vector2 targetRotation;
    Transform insideObject;
    float autoRotatateAfter;


    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        Registar_For_Input_Callbacks();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        insideObject = objectHolder.GetChild(0);
        autoRotatateAfter = rotateThreshold;

        autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.up, Vector3.zero.y);
        autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.right, Vector3.zero.x);
        autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.forward, Vector3.zero.z);
    }

    void Update()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (Input.GetMouseButton(0))
        {
            autoRotatateAfter = rotateThreshold;
        }
    }

    private void FixedUpdate()
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED) || gameplayData.tutorial_Tap_Blocker)
            return;

        if (autoRotatateAfter > 0)
        {
            autoRotatateAfter -= Time.deltaTime;

            currentRotation = Vector2.SmoothDamp(currentRotation, targetRotation, ref currentVelocity, smoothTime);

            autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.up, currentVelocity.y * rotationSpeed * Time.deltaTime);
            autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.right, currentVelocity.x * rotationSpeed * Time.deltaTime);
        }
        else
        {
            autoRotationHolder.RotateAround(autoRotationHolder.position, Camera.main.transform.up, autoRotateSpeed * Time.deltaTime);
        }

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS
    Vector3 value = Vector3.zero;
    public override void OnDrag(Vector3 dragAmount)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        base.OnDrag(dragAmount);

        if (gameManager.centerInputDetector)
        {
            targetRotation += new Vector2(dragAmount.y, -dragAmount.x) * rotationSpeed * Time.deltaTime;
        }
    }

    public override void OnTapStart(Vector3 tapOnWorldSpace)
    {
        base.OnTapStart(tapOnWorldSpace);

    }

    public override void OnTapEnd(Vector3 tapOnWorldSpace)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        base.OnTapEnd(tapOnWorldSpace);

        //insideObject.parent = null;
        //objectHolder.eulerAngles = Vector3.zero;
        //insideObject.parent = objectHolder;
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS
}
