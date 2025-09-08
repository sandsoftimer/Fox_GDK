using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransitResource))]
public class Storage : FoxObject
{
    public Action<Storage, GameObject> OnResourceUpdate;
    public List<GameObject> supportedStorageObjects;
    public bool vanishAfterCollect = false;
    public bool unlimitedCapacity = false;
    public bool randomStackingRotation = false;
    public bool allowStacking = false;
    public float stackingGap = 0.25f;
    public int capacity = 20;


    [ReadOnlyProperty] public List<GameObject> resources = new();
    TransitResource transitResource;
    bool isNotBusy;
    int transitioning;
    int stackIndex;
    [ReadOnlyProperty] public int totalTaken;
    [ReadOnlyProperty] public int resourceTaken;

    public bool IS_BUSY
    {
        get { return transitioning > 0; }
    }

    public bool IS_FULL
    {
        get { return totalTaken >= capacity && !unlimitedCapacity; }
    }

    public bool IS_EMPTY
    {
        get { return resources.Count <= 0; }
    }

    public int VISUAL_RESOURCE_TAKEN
    {
        get { return resourceTaken; }
        set { resourceTaken = value; }
    }

    public int TOTAL_TAKEN
    {
        get { return totalTaken; }
        set { totalTaken = Mathf.Clamp(value, 0, value); }
    }

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        stackIndex = -1;
        totalTaken = 0;
        transitResource = GetComponent<TransitResource>();

        if (transform.childCount < 1)
        {
            GameObject go = new GameObject("StroagePoint");
            go.transform.parent = transform;
            go.transform.FOXE_Reset();
        }
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

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public GameObject Release_A_Matching_Resource(GameObject aType)
    {
        GameObject go = null;
        if (resources.Count > 0)
        {
            go = Get_An_Item_Then_Suffle(aType);
            if (!unlimitedCapacity && go != null)
            {
                TOTAL_TAKEN--;
                OnResourceUpdate?.Invoke(this, go);
            }
        }
        return go;
    }

    public GameObject Release_A_Stack_Top_Resource()
    {
        GameObject go = null;
        if (resources.Count > 0)
        {
            if (!unlimitedCapacity)
                TOTAL_TAKEN--;
            stackIndex = (stackIndex - 1) < 0 ? -1 : (stackIndex - 1);
            go = resources[resources.Count - 1];
            resources.RemoveAt(resources.Count - 1);
            VISUAL_RESOURCE_TAKEN--;
            OnResourceUpdate?.Invoke(this, go);
        }
        return go;
    }


    GameObject Get_An_Item_Then_Suffle(GameObject aType)
    {
        GameObject go = null;
        int index = -1;

        if (aType)
        {
            for (int i = resources.Count - 1; i >= 0; i--)
            {
                if (resources[i].CompareTag(aType.tag) && index == -1)
                {
                    index = i;
                    go = resources[index];
                    resources.RemoveAt(index);
                    VISUAL_RESOURCE_TAKEN--;
                    stackIndex = (stackIndex - 1) < 0 ? -1 : (stackIndex - 1);
                    for (int j = i; j < resources.Count; j++)
                    {
                        resources[j].transform.localPosition = resources[j].transform.localPosition.FOXE_ModifyThisVector(0, transform.childCount > 1 ? 0 : -stackingGap, 0);
                    }
                    break;
                }
            }
        }

        return go;
    }

    public bool Store_This_Resource(Transform obj, Action actionAfterTransition = null)
    {
        bool result = Can_I_Store_This(obj.gameObject);
        if (result)
        {
            if (!unlimitedCapacity)
                TOTAL_TAKEN++;
            stackIndex = (stackIndex + 1) >= transform.childCount ? 0 : (stackIndex + 1);
            obj.parent = transform.GetChild(stackIndex);
            Vector3 moveTo = transform.GetChild(stackIndex).position.FOXE_ModifyThisVector(0, resources.Count / transform.childCount * stackingGap, 0);
            Vector3 rotateTo = transform.GetChild(stackIndex).eulerAngles + (randomStackingRotation ? FoxExtensions.FOXE_GetRandomDirection3D() * 180 : Vector3.zero);

            if (!vanishAfterCollect)
            {
                resources.Add(obj.gameObject);
                VISUAL_RESOURCE_TAKEN++;
            }
            transitioning++;
            transitResource.TransitThisResource(obj, moveTo, rotateTo, vanishAfterCollect, () =>
            {

                transitioning--;
                actionAfterTransition?.Invoke();

            });
            OnResourceUpdate?.Invoke(this, obj.gameObject);
        }
        return result;
    }

    public bool Store_This_Resource(Transform obj, ParabolaMove parabolaMove)
    {
        bool result = Can_I_Store_This(obj.gameObject);
        if (result)
        {
            if (!unlimitedCapacity)
                TOTAL_TAKEN++;
            stackIndex = (stackIndex + 1) >= transform.childCount ? 0 : (stackIndex + 1);
            obj.parent = transform.GetChild(stackIndex);
            Vector3 moveTo = transform.GetChild(stackIndex).position.FOXE_ModifyThisVector(0, allowStacking ? resources.Count / transform.childCount * stackingGap : 0, 0);
            Vector3 rotateTo = transform.GetChild(stackIndex).eulerAngles;

            if (!vanishAfterCollect)
            {
                resources.Add(obj.gameObject);
                VISUAL_RESOURCE_TAKEN++;
            }
            transitioning++;
            parabolaMove.action += () =>
            {
                transitioning--;
                if (vanishAfterCollect)
                    FoxTools.poolManager.Destroy(obj.gameObject);
            };
            transitResource.TransitThisResource(obj, parabolaMove);
            OnResourceUpdate?.Invoke(this, obj.gameObject);
        }
        return result;
    }

    public GameObject Can_I_Store_This(GameObject go)
    {
        GameObject returnObj = null;
        if (IS_FULL)
            return returnObj;

        for (int i = 0; i < supportedStorageObjects.Count; i++)
        {
            if (supportedStorageObjects[i].tag.Equals(go.tag))
            {
                returnObj = go;
                break;
            }
        }
        return returnObj;
    }


    public void Set_Storage_Type(GameObject go, bool removePreviousSupoortType = false)
    {
        if (removePreviousSupoortType)
        {
            supportedStorageObjects.Clear();
        }
        int index = -1;
        for (int i = 0; i < supportedStorageObjects.Count; i++)
        {
            if (supportedStorageObjects[i].tag.Equals(go.tag))
            {
                index = i;
                break;
            }
        }
        if (index == -1)
            supportedStorageObjects.Add(go);
    }

    public void Set_Empty_Full_Storage(bool alsoStorageTypeClear = false)
    {
        while (!IS_EMPTY)
        {
            FoxTools.poolManager.Destroy(Release_A_Stack_Top_Resource());
        }

        if (alsoStorageTypeClear)
        {
            supportedStorageObjects.Clear();
        }
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}