using System;
using System.Collections.Generic;
using UnityEngine;

public class Mask_Manager : BaseGameBehaviour
{
    public Renderer maskRenderer;

    public List<Mask_Data> mask_Datas;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        gameManager.maskManager = this;

        maskRenderer.gameObject.SetActive(false);
        //gameObject.SetActive(gameplayData.currentLevelNumber == gameManager.constantManager.BASIC_TUTORIAL_LEVEL);
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

    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnBoosterPurchased(Booster_Item booster_Item)
    {
        base.OnBoosterPurchased(booster_Item);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                gameObject.SetActive(false);
                break;
            case Booster_Type.MAGNET:
                break;
        }
    }

    public override void OnBoosterPreimplemented(Booster_Item booster_Item)
    {
        base.OnBoosterPreimplemented(booster_Item);

        switch (booster_Item.type)
        {
            case Booster_Type.HAMMER:
            case Booster_Type.GOAL:
            case Booster_Type.DOCK:
                gameObject.SetActive(false);
                break;
            case Booster_Type.MAGNET:
                Set_Off_Mask();
                break;
        }

    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Initialize_Next_Masking(int index)
    {
        gameObject.SetActive(true);
        maskRenderer.gameObject.SetActive(true);
        if (index >= mask_Datas.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        maskRenderer.material.SetVector("_Mask_Size", mask_Datas[index].sizes);
        maskRenderer.material.SetVector("_Mask_Position", mask_Datas[index].position);
    }

    public void Set_Off_Mask()
    {
        maskRenderer.gameObject.SetActive(false);
    }

    #endregion ALL SELF DECLARE FUNCTIONS
}

[Serializable]
public class Mask_Data
{
    public string Identity;
    public Vector2 position, sizes;
}
