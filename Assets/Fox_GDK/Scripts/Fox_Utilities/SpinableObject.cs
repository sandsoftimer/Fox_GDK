using UnityEngine;

public class SpinableObject : FoxObject
{
    public Transform movingTransfrom;
    public Vector3 initialAngle;
    public Vector3 negetiveMove, positiveMove;
    public Vector3 moveSpeed;

    int xMovingDirection, yMovingDirection, zMovingDirection;
    Vector3 basePosition, currentPosition, projectilePosition;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        negetiveMove.x = Mathf.Abs(negetiveMove.x);
        negetiveMove.y = Mathf.Abs(negetiveMove.y);
        negetiveMove.z = Mathf.Abs(negetiveMove.z);

        positiveMove.x = Mathf.Abs(positiveMove.x);
        positiveMove.y = Mathf.Abs(positiveMove.y);
        positiveMove.z = Mathf.Abs(positiveMove.z);

        xMovingDirection = Random.Range(1, 3) == 1 ? 1 : -1;
        yMovingDirection = Random.Range(1, 3) == 1 ? 1 : -1;
        zMovingDirection = Random.Range(1, 3) == 1 ? 1 : -1;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        basePosition = movingTransfrom.localEulerAngles;
        currentPosition = movingTransfrom.localEulerAngles + initialAngle;
        //movingTransfrom.localEulerAngles += initialAngle;
    }

    void FixedUpdate()
    {
        //if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
        //    return;

        projectilePosition = currentPosition + new Vector3(moveSpeed.x * xMovingDirection * Time.fixedDeltaTime, moveSpeed.y * yMovingDirection * Time.fixedDeltaTime, moveSpeed.z * zMovingDirection * Time.fixedDeltaTime);

        if (xMovingDirection < 0)
        {
            if (projectilePosition.x <= basePosition.x - negetiveMove.x)
            {
                xMovingDirection *= -1;
            }
        }
        else if (xMovingDirection > 0)
        {
            if (projectilePosition.x >= basePosition.x + positiveMove.x)
            {
                xMovingDirection *= -1;
            }
        }

        if (yMovingDirection < 0)
        {
            if (projectilePosition.y <= basePosition.y - negetiveMove.y)
            {
                yMovingDirection *= -1;
            }
        }
        else if (yMovingDirection > 0)
        {
            if (projectilePosition.y >= basePosition.y + positiveMove.y)
            {
                yMovingDirection *= -1;
            }
        }

        if (zMovingDirection < 0)
        {
            if (projectilePosition.z <= basePosition.z - negetiveMove.z)
            {
                zMovingDirection *= -1;
            }
        }
        else if (zMovingDirection > 0)
        {
            if (projectilePosition.z >= basePosition.z + positiveMove.z)
            {
                zMovingDirection *= -1;
            }
        }

        //float _moveSpeed = animationCurve.Evaluate(FoxExtensions.InverseLerp(basePosition - negetiveMove, basePosition + positiveMove, currentPosition));
        currentPosition += new Vector3(moveSpeed.x * xMovingDirection * Time.fixedDeltaTime, moveSpeed.y * yMovingDirection * Time.fixedDeltaTime, moveSpeed.z * zMovingDirection * Time.fixedDeltaTime);

        movingTransfrom.localEulerAngles = currentPosition;
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS


    #endregion ALL SELF DECLARE FUNCTIONS

}
