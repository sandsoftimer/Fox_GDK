using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Storage_Use_Type
{
    MOMENTUM, // Player can give from hand stack until he is within collider
    ALL_IN, // player have to give all from hand even he is outside of collider
}

public enum Storage_Support_Type
{
    DEFAULT,
    CUSTOMER_WILL_SET
}

public class StorageController : FoxObject
{
    public LayerMask usersLayer;
    public Storage_Use_Type storage_UseType;
    public Storage_Support_Type storage_Support_Type;
    public Storage storage;

        
    GameObject currentUser;
    Collider _Colliders;

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();

        storage.OnResourceUpdate += OnResourceUpdate;
        OnResourceUpdate(storage, null);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        storage.OnResourceUpdate -= OnResourceUpdate;
    }

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        _Colliders = GetComponent<Collider>();
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

        //paymentStorage.IS_NOT_BUSY = holdingGuy == null;
    }

    private void FixedUpdate()
    {
        //if(refreshSensor)
        //    _Colliders.enabled = !_Colliders.enabled;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(May_I_Use(other) && currentUser == null)
            currentUser = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        I_Am_Leaving(other);
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    void OnResourceUpdate(Storage storage, GameObject go)
    {
        //if (storage_Support_Type.Equals(Storage_Support_Type.CUSTOMER_WILL_SET) && paymentStorage.IS_FULL)
        //    paymentStorage.Set_Empty_Full_Storage();
    }

    public bool May_I_Use(Collider collider)
    {
        bool result = (usersLayer == (usersLayer | (1 << collider.gameObject.layer)));
        
        return result;
    }

    void I_Am_Leaving(Collider collider)
    {
        if (currentUser == null)
            return;
        if (currentUser == collider.gameObject)
            currentUser = null;
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}
