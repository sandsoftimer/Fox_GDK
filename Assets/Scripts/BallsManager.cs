using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallsManager : BaseGameBehaviour
{
    public GameObject ballPrefab;
    public Material material;
    public Transform leftDoor, rightDoor;

    [Space(20)]
    public int numberOfBallPerPoint = 5;
    public Transform spawnPointsHolder;

    Vector3 leftCloseAngle = new(0, 0, 90), rightCloseAngle = new(0, 0, -90);
    Vector3 leftOpenAngle = new(0, 0, 20), rightOpenAngle = new(0, 0, -20);

    Dictionary<string, List<GameObject>> ballsTressure = new();
    RaycastHit raycastHit = new();

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        Initialize_Level();
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

        //if (Input.GetMouseButtonDown(0))
        //    OpenDoor();
        //else if (Input.GetMouseButtonUp(0))
        //    CloseDoor();

        if (Input.GetMouseButtonDown(0))
        {
            raycastHit.FOXE_GetRaycastFromScreenTouch(Vector3.zero, 1 << ConstantManager.LAYER_PICKUPS);
            if (raycastHit.collider)
            {
                List<GameObject> list = ballsTressure[raycastHit.collider.name];
                string target = raycastHit.collider.name;
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    Destroy(list[i]);
                }
                ballsTressure.Remove(target);
                if (ballsTressure.Count == 0)
                {
                    gameManager.GameOver(true);
                }
            }
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    private void Initialize_Level()
    {
        leftDoor.eulerAngles = leftCloseAngle;
        rightDoor.eulerAngles = rightCloseAngle;

        for (int i = 0; i < spawnPointsHolder.childCount; i++)
        {
            Color color = FoxExtensions.FOXE_GetRandomColor();
            Rigidbody hookBody = spawnPointsHolder.GetChild(i).GetComponent<Rigidbody>();
            Rigidbody previousBody = null;
            List<GameObject> connectedBalls = new();
            int y = (int)spawnPointsHolder.GetChild(i).position.y + numberOfBallPerPoint / 2;
            for (int j = 0; j < numberOfBallPerPoint; j++)
            {
                int x = (int)spawnPointsHolder.GetChild(i).position.x + -numberOfBallPerPoint / 2;
                for (int k = 0; k < numberOfBallPerPoint; k++)
                {
                    GameObject go = Instantiate(ballPrefab, new Vector3(x++, y, 0), Quaternion.identity);
                    ConfigurableJoint ballJoint = go.AddComponent<ConfigurableJoint>();
                    ConfigurableJoint hookJoint = spawnPointsHolder.GetChild(i).AddComponent<ConfigurableJoint>();
                    ballJoint.connectedBody = hookBody;
                    Rigidbody ballRb = go.GetComponent<Rigidbody>();
                    hookJoint.connectedBody = ballRb;
                    material = go.GetComponent<Renderer>().material = new Material(material);
                    material.color = color;
                    connectedBalls.Add(go);
                    go.name = $"{i}";
                    //if (previousBody)
                    //{
                    //    ConfigurableJoint preJoint = previousBody.AddComponent<ConfigurableJoint>();
                    //    ballJoint = go.AddComponent<ConfigurableJoint>();
                    //    preJoint.connectedBody = ballRb;
                    //    ballJoint.connectedBody = preJoint.GetComponent<Rigidbody>();
                    //}

                    //previousBody = ballRb;
                }
                y--;
            }
            ballsTressure[$"{i}"] = connectedBalls;
        }
    }

    public void OpenDoor()
    {
        leftDoor.DORotate(leftOpenAngle, ConstantManager.ONE_FORTH_TIME);
        rightDoor.DORotate(rightOpenAngle, ConstantManager.ONE_FORTH_TIME);
    }

    public void CloseDoor()
    {
        leftDoor.DORotate(leftCloseAngle, ConstantManager.ONE_FORTH_TIME);
        rightDoor.DORotate(rightCloseAngle, ConstantManager.ONE_FORTH_TIME);
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}
