using Com.FunFox.Utility;
using UnityEngine;
using UnityEngine.Purchasing;

[DefaultExecutionOrder(ConstantManager.FoxBehaviourOrder)]
public class FoxObject : MonoBehaviour, IHierarchyIcon
{
    [HideInInspector]
    public FoxManager foxManager;
    [HideInInspector]
    public FoxTools FoxTools;

    [HideInInspector]
    public GameplayData gameplayData;
    [ReadOnlyProperty] public GameState gameState = GameState.NONE;

    bool registeredForInput;

    public string EditorIconPath { get { return "FoxObjectIcon"; } }

    public virtual void Awake()
    {
        FoxTools = FoxTools.Instance;
        FoxManager.OnAddFoxBehaviour?.Invoke(this);
    }

    public virtual void Start() { }

    public virtual void OnEnable()
    {
        FoxManager.OnGameInitialize += OnGameInitializing;
        FoxManager.OnGameStart += OnGameStart;
        FoxManager.OnGameOver += OnGameOver;
        FoxManager.OnPauseGamePlay += OnPauseGamePlay;
        FoxManager.OnUnPauseGamePlay += OnUnPauseGamePlay;
        FoxManager.OnChangeGameState += OnChangeGameState;
        FoxManager.OnCompleteTask += OnCompleteTask;
        FoxManager.OnIncompleteTask += OnIncompleteTask;
        FoxManager.OnBoosterPreimplemented += OnBoosterPreimplemented;
        FoxManager.OnBoosterPurchased += OnBoosterPurchased;
        FoxManager.OnBoosterCanceled += OnBoosterCanceled;
        FoxManager.OnBoosterImplemented += OnBoosterImplemented;
        FoxManager.OnBoosterClickedWhileBusy += OnBoosterClickedWhileBusy;
        FoxManager.OnReviveGame += OnReviveGame;

        if (registeredForInput)
        {
            FoxManager.OnTap += ProccessInputTapping;
            FoxManager.OnDrag += OnDrag;
            FoxManager.OnSwip += ProcessInputSwipping;
            FoxManager.OnZoom += ProcessInputZooming;
        }
        FoxManager.OnAddFoxBehaviour?.Invoke(this);
        #region GAME SPECIFIC SPACE


        #endregion GAME SPECIFIC SPACE
    }

    public virtual void OnDisable()
    {
        FoxManager.OnGameInitialize -= OnGameInitializing;
        FoxManager.OnGameStart -= OnGameStart;
        FoxManager.OnGameOver -= OnGameOver;
        FoxManager.OnPauseGamePlay -= OnPauseGamePlay;
        FoxManager.OnUnPauseGamePlay -= OnUnPauseGamePlay;
        FoxManager.OnChangeGameState -= OnChangeGameState;
        FoxManager.OnCompleteTask -= OnCompleteTask;
        FoxManager.OnIncompleteTask -= OnIncompleteTask;
        FoxManager.OnBoosterPreimplemented -= OnBoosterPreimplemented;
        FoxManager.OnBoosterPurchased -= OnBoosterPurchased;
        FoxManager.OnBoosterCanceled -= OnBoosterCanceled;
        FoxManager.OnBoosterImplemented -= OnBoosterImplemented;
        FoxManager.OnBoosterClickedWhileBusy -= OnBoosterClickedWhileBusy;
        FoxManager.OnReviveGame -= OnReviveGame;

        if (registeredForInput)
        {
            FoxManager.OnTap -= ProccessInputTapping;
            FoxManager.OnDrag -= OnDrag;
            FoxManager.OnSwip -= ProcessInputSwipping;
            FoxManager.OnZoom -= ProcessInputZooming;
        }

        #region GAME SPECIFIC SPACE


        #endregion GAME SPECIFIC SPACE
    }

    public virtual void OnBoosterPreimplemented(Booster_Item booster_Item) { }

    public virtual void OnBoosterCanceled(Booster_Item booster_Item) { }

    public virtual void OnBoosterClickedWhileBusy(Booster_Item booster_Item) { }

    public virtual void OnBoosterImplemented(Booster_Item booster_Item) { }

    public virtual void OnBoosterPurchased(Booster_Item booster_Item) { }

    public virtual void OnInAppPurchsedDone(PurchaseEventArgs purchaseEvent, Product_ID product_ID) { }

    protected virtual void OnReviveGame() { }

    public virtual void OnTapStart(Vector3 tapOnWorldSpace) { }

    public virtual void OnTapAndHold(Vector3 tapOnWorldSpace) { }

    public virtual void OnTapEnd(Vector3 tapOnWorldSpace) { }

    public virtual void OnSingleTap(Vector3 tapOnWorldSpace) { }

    public virtual void OnDrag(Vector3 dragAmount) { }

    public virtual void OnSwipeUp() { }

    public virtual void OnSwipeDown() { }

    public virtual void OnSwipeLeft() { }

    public virtual void OnSwipeRight() { }

    public virtual void OnZoom(float zoomDelta) { }

    public virtual void OnChangeGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public virtual void OnIncompleteTask() { }

    public virtual void OnGameInitializing() { }

    public virtual void OnGameStart() { }

    public virtual void OnGameOver() { }

    public virtual void OnPauseGamePlay() { }

    public virtual void OnUnPauseGamePlay() { }

    public virtual void OnCompleteTask() { }

    public void Registar_For_Input_Callbacks()
    {
        registeredForInput = true;
    }

    void ProcessInputZooming(float zoom)
    {
        OnZoom(zoom);
    }

    void ProccessInputTapping(KV_TappingType inputType, Vector3 tapOnWorldSpace)
    {
        switch (inputType)
        {
            case KV_TappingType.NONE:
                break;
            case KV_TappingType.TAP_START:
                OnTapStart(tapOnWorldSpace);
                break;
            case KV_TappingType.TAP_END:
                OnTapEnd(tapOnWorldSpace);
                break;
            case KV_TappingType.SINGLE_TAP:
                OnSingleTap(tapOnWorldSpace);
                break;
            case KV_TappingType.TAP_N_HOLD:
                OnTapAndHold(tapOnWorldSpace);
                break;
        }
    }

    void ProcessInputSwipping(KV_SwippingType swippingType)
    {
        switch (swippingType)
        {
            case KV_SwippingType.SWIPE_UP:
                OnSwipeUp();
                break;
            case KV_SwippingType.SWIPE_DOWN:
                OnSwipeDown();
                break;
            case KV_SwippingType.SWIPE_LEFT:
                OnSwipeLeft();
                break;
            case KV_SwippingType.SWIPE_RIGHT:
                OnSwipeRight();
                break;
        }
    }

    public void SaveGame()
    {
        foxManager.SaveGame();
    }
}
