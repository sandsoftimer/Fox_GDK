public enum LoadSceneType
{
    LOAD_BY_NAME,
    LOAD_BY_INDEX
}

public enum Fox_SceneLooping_Type
{
    OBJECT_LOOPING,
    SCENE_LOOPING
}

public enum GameState
{
    NONE,
    GAME_INITIALIZED, // After loading SAVED DATA, this state will be called, Game will be ready to play with previous boxData
    GAME_PLAY_STARTED,
    GAME_PLAY_ENDED,
    GAME_PLAY_PAUSED,
    GAME_PLAY_UNPAUSED
}

public enum AcceptedValueType
{
    BOTH,
    ONLY_POSITIVE,
    ONLY_NEGETIVE,
    NONE
}

public enum KV_TappingType
{
    NONE,
    TAP_START,
    TAP_END,
    SINGLE_TAP,
    TAP_N_HOLD
}

public enum KV_SwippingType
{
    SWIPE_UP,
    SWIPE_DOWN,
    SWIPE_LEFT,
    SWIPE_RIGHT
}

public enum WeaponType
{
    MELEE,
    GUN
}

public enum UpdateMethod
{
    UPDATE,
    FIXED_UPDATE,
    LATE_UPDATE
}

public enum KV_Axis
{
    ALL,
    X,
    Y,
    Z
}

public enum MeshUsingType
{
    ORDINARY,
    CUTTING,
    DEFORMING
}

public enum Pivot
{
    TOP_LEFT,
    TOP_CENTER,
    TOP_RIGHT,
    MIDDLE_LEFT,
    MIDDLE_CENTER,
    MIDDLE_RIGHT,
    BOTTOM_LEFT,
    BOTTOM_CENTER,
    BOTTOM_RIGHT
}

public enum MovingMethod
{
    TRANSFORM_POSITION,
    RIGIDBODY,
    TRAVEL_PATH
}

public enum KV_Scene_Transition_Type
{
    FADE,
    TEXTURE_ANIMATION,
    SHATTER_ANIMATION
}

public enum PlayerUpgradeType
{
    PLAYER_HEALTH,
    PLAYER_DAMAGE,
    PLAYER_SPEED,
    PLAYER_CARRY_CAPACITY,
    HR_HIRE_STUFF,
    STUFF_SPEED,
    STUFF_CARRY_CAPACITY
}
