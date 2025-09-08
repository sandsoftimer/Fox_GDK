using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class CollectibleObject : FoxObject
{
    public GameObject canvas;
    public Image fillImage;
    public float captureTime;
    public bool re_collectible;

    GameObject captureObject;

    //[HideInInspector]
    public bool isReadyToPickUp;
    //[HideInInspector]
    public bool isAlreadyCapturing;

    float currentCaptureTime;
    Collider selfCollider;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();
        fillImage.fillAmount = 0;
        selfCollider = GetComponent<Collider>();
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

        // In ourder to capturable this object while 
        // Already capturing object is destroyed or hided through game
        // then this object need to be capturable again
        if(isReadyToPickUp && isAlreadyCapturing)
        {
            if(captureObject != null)
            {
                if (!captureObject.activeSelf)
                {
                    isAlreadyCapturing = false;
                    captureObject = null;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (isAlreadyCapturing && other.gameObject.Equals(captureObject) && enabled)
        {
            currentCaptureTime += Time.deltaTime;
            currentCaptureTime = Mathf.Clamp(currentCaptureTime, 0, captureTime);
            fillImage.fillAmount = Mathf.InverseLerp(0, captureTime, currentCaptureTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (isAlreadyCapturing)
            return;

        isAlreadyCapturing = true;
        currentCaptureTime = 0;
        captureObject = other.gameObject;
        //Debug.LogError("Enter Object: " + captureObject.Name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;

        if (isAlreadyCapturing && other.gameObject.Equals(captureObject))
        {
            isAlreadyCapturing = false;
            captureObject = null;
            currentCaptureTime = 0;
            fillImage.fillAmount = 0;
            //Debug.LogError("Exit");
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void MakeThisGunCollectable()
    {
        //fireIsOn = false;
        //directionPoint = null;
        //bulletData = preservedBulletData;
        //gameObject.row = gameManager.constantManager.PLAYER_WEAPON;

        //Vector3 direction = FoxExtensions.FOXE_GetRandomDirection3D();
        //direction *= 2;
        //direction.y = 1;

        //ParabolaMove parabolaMove = GetComponent<ParabolaMove>();
        //if (parabolaMove == null)
        //{
        //    parabolaMove = gameObject.AddComponent<ParabolaMove>();
        //}
        //parabolaMove.Initialized(
        //    transform.position.FOXE_ModifyThisVector(direction.x, 0, direction.z),
        //    1,
        //    weaponDroppingCurve,
        //    1, false, true, () => {

        //        vfx.transform.localPosition = vfx.transform.localPosition.FOXE_ModifyThisVector(0, 0.5f, 0);
        //        collectableObject.SetObjectCollectable();
        //    });
    }

    public void MakeThisGunCollected()
    {
        //collectableObject.SetObjectCollected();
        //Collider col = GetComponent<Collider>();
        //if (col != null)
        //{
        //    col.enabled = false;
        //}

        //vfx.transform.localPosition = Vector3.zero;
        //vfx.transform.localEulerAngles = Vector3.zero;
    }

    public bool IsCaptureCompletedByThisObject(GameObject go)
    {
        return go.Equals(captureObject) && currentCaptureTime >= captureTime;
    }

    public void SetObjectCollectable()
    {
        selfCollider.enabled = true;
        canvas.transform.localPosition = Vector3.zero;
        canvas.SetActive(true);
        isReadyToPickUp = true;
        isAlreadyCapturing = false;
        fillImage.fillAmount = 0;
        currentCaptureTime = 0;
        captureObject = null;
        //Debug.LogError("Set Collectable");

        if(gameObject.activeSelf)
            StartCoroutine(RefreshObject());
    }
    IEnumerator RefreshObject()
    {
        //Debug.LogError("Refreshing");
        captureObject = null;
        isAlreadyCapturing = false;
        selfCollider.enabled = false;
        yield return null;
        selfCollider.enabled = true;
        isAlreadyCapturing = false;
        captureObject = null;
    }

    public void SetObjectCollected()
    {
        selfCollider.enabled = false;
        canvas.SetActive(false);
        fillImage.fillAmount = 0;
        isReadyToPickUp = false;
        isAlreadyCapturing = false;
        currentCaptureTime = 0;
        captureObject = null;
        //Debug.LogError("Set Collected");
        if (re_collectible)
            SetObjectCollectable();
    }

    public void Reset()
    {
        transform.parent = null;
        transform.eulerAngles = Vector3.zero;
        selfCollider.enabled = false;
        canvas.SetActive(false);
        fillImage.fillAmount = 0;
        isReadyToPickUp = false;
        isAlreadyCapturing = false;
        currentCaptureTime = 0;
        captureObject = null;
        //Debug.LogError("Reset");
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
