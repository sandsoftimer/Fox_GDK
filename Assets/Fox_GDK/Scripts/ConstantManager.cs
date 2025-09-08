using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(ConstantManagerOrder)]
public partial class ConstantManager : ScriptableObject
{
    #region Custom attributes of this game

    [Space(20)]
    [Header("SCENE LOOPING PROPERTIES")]
    public Fox_SceneLooping_Type SCENE_LOOPING_TYPE = Fox_SceneLooping_Type.SCENE_LOOPING;
    public int TOTAL_GAME_LEVELS = 1;
    public int SCENE_LOOPING_STARTING_INDEX = 1;

    [Space(20)]
    [Header("TUTORIAL PROPERTIES")]
    public int BASIC_TUTORIAL_LEVEL = -1;
    public int MARGE_TUTORIAL_LEVEL = -1;

    [Space(20)]
    [Header("FEATURE PROPERTIES")]
    public int HIDDEN_HOLES_TUTORIAL_LEVEL = -1;
    public int HIDDEN_BLOCK_TUTORIAL_LEVEL = -1;
    public int SQURE_HOLES_TUTORIAL_LEVEL = -1;
    public int MULTISTORIED_TUTORIAL_LEVEL = -1;
    public int FROZEN_TUTORIAL_LEVEL = -1;
    public int TRIANGLE_HOLES_TUTORIAL_LEVEL = -1;
    public int CONNECTED_HOLES_TUTORIAL_LEVEL = -1;

    [Space(20)]
    [Header("BOOSTER UNLOCK PROPERTIES")]
    public int MAGNET_UNLOCK_LEVEL_NUMBER = -1;
    public int DOCK_UNLOCK_LEVEL_NUMBER = -1;
    public int HAMMER_UNLOCK_LEVEL_NUMBER = -1;
    public int GOAL_UNLOCK_LEVEL_NUMBER = -1;

    #region Environment System
    [Space(20)]
    [Header("Environment Datas")]
    public int sceneSupportCount = 1;
    public EnvironmentData hardEnvironmentData;
    public List<EnvironmentData> regularEnvironmentDatas;
    public EnvironmentData Get_Environment(int currentLevelNumber, Difficulty_Type difficulty_Type)
    {
        EnvironmentData result = new();

        if (difficulty_Type.Equals(Difficulty_Type.HARD))
            result = hardEnvironmentData;
        else
        {
            int index = currentLevelNumber / sceneSupportCount;
            index = index % regularEnvironmentDatas.Count;
            result = regularEnvironmentDatas[index];
        }
        return result;
    }
    #endregion Environment System

    #endregion Custom attributes of this game

    // RuntimeExecution Order of FoxLibray Scripts.
    public const int ConstantManagerOrder = -500;
    public const int FoxToolOrder = -450;
    public const int FoxManagerOrder = -4000;
    public const int GameManager = -2000;
    public const int FoxBehaviourOrder = -1000;
    public const int BaseGameBehaviour = -500;

    public const float DEFAULT_ANIMATION_TIME = 1f;
    public const float ONE_HALF_TIME = DEFAULT_ANIMATION_TIME / 2;
    public const float ONE_FORTH_TIME = DEFAULT_ANIMATION_TIME / 4;
    public const float ONE_TENTH_TIME = DEFAULT_ANIMATION_TIME / 10;
    public const float SINGLE_TAP_TIME_THRESHOLD = 0.25f;
    public const float DRAGGING_DISTANCE_THRESHOLD = 0.1f;
    public const float TAP_N_HOLD_THRESHOLD = 0.1f;
    public const float SWIPPING_THRESHOLD = 0.5f;

    /// <summary>
    /// Create an integer variable for script use
    /// and add that Name to dictonary below as well
    /// FoxConfigarator.cs will use this to modify Layer System.
    /// </summary>
    #region Collision Layer Setup
    // Collision Layer ids. 
    // These row's order must be maintained in Bottom Layer Array
    public const int LAYER_DEFAULT = 0;
    public const int LAYER_TRANSPARENT_FX = 1;
    public const int LAYER_IGNORE_RAYCAST = 2;
    public const int LAYER_WATER = 4;
    public const int LAYER_UI = 5;
    public const int LAYER_PLAYER = 8;
    public const int LAYER_PLAYER_WEAPON = 9;
    public const int LAYER_ENEMY = 10;
    public const int LAYER_ENEMY_WEAPON = 11;
    public const int LAYER_GROUND = 12;
    public const int LAYER_BOUNDARY = 13;
    public const int LAYER_PICKUPS = 14;
    public const int LAYER_DESTINATION = 15;
    public const int LAYER_SENSOR = 16;
    public const int LAYER_NON_RENDERING = 17;
    public const int LAYER_SHAPE_PIECE = 18;
    public const int LAYER_INPUT_SENSOR = 19;
    public const int LAYER_OUTPUT_SENSOR = 20;
    public const int LAYER_MASK = 21;
    public const int LAYER_SPIRAL_OBJECT = 22;
    public const int LAYER_UNDEFINED_23 = 23;
    public const int LAYER_UNDEFINED_24 = 24;
    public const int LAYER_UNDEFINED_25 = 25;
    public const int LAYER_UNDEFINED_26 = 26;
    public const int LAYER_UNDEFINED_27 = 27;
    public const int LAYER_UNDEFINED_28 = 28;
    public const int LAYER_UNDEFINED_29 = 29;
    public const int LAYER_UNDEFINED_30 = 30;
    public const int LAYER_UNDEFINED_31 = 31;

    public static readonly Dictionary<int, string> layerNames = new Dictionary<int, string>
    {
        { 0, "Default" },
        { 1, "TransparentFX" },
        { 2, "Ignore Raycast" },
        { 3, "" },
        { 4, "Water" },
        { 5, "UI" },
        { 6, "" },
        { 7, "" },
        { 8, $"{nameof(LAYER_PLAYER)}" },
        { 9, $"{nameof(LAYER_PLAYER_WEAPON)}" },
        { 10, $"{nameof(LAYER_ENEMY)}" },
        { 11, $"{nameof(LAYER_ENEMY_WEAPON)}" },
        { 12, $"{nameof(LAYER_GROUND)}" },
        { 13, $"{nameof(LAYER_BOUNDARY)}" },
        { 14, $"{nameof(LAYER_PICKUPS)}" },
        { 15, $"{nameof(LAYER_DESTINATION)}" },
        { 16, $"{nameof(LAYER_SENSOR)}" },
        { 17, $"{nameof(LAYER_NON_RENDERING)}" },
        { 18, $"{nameof(LAYER_SHAPE_PIECE)}" },
        { 19, $"{nameof(LAYER_INPUT_SENSOR)}" },
        { 20, $"{nameof(LAYER_OUTPUT_SENSOR)}" },
        { 21, $"{nameof(LAYER_MASK)}" },
        { 22, $"{nameof(LAYER_SPIRAL_OBJECT)}" },
        { 23, $"{nameof(LAYER_UNDEFINED_23)}" },
        { 24, $"{nameof(LAYER_UNDEFINED_24)}" },
        { 25, $"{nameof(LAYER_UNDEFINED_25)}" },
        { 26, $"{nameof(LAYER_UNDEFINED_26)}" },
        { 27, $"{nameof(LAYER_UNDEFINED_27)}" },
        { 28, $"{nameof(LAYER_UNDEFINED_28)}" },
        { 29, $"{nameof(LAYER_UNDEFINED_29)}" },
        { 30, $"{nameof(LAYER_UNDEFINED_30)}" },
        { 31, $"{nameof(LAYER_UNDEFINED_31)}" },
    };
    #endregion Collision Layer Setup
}
