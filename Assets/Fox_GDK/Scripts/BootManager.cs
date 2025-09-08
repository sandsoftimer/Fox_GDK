#if HOMA_SDK_INSTALLED
using HomaGames.HomaBelly;
#endif


public class BootManager : FoxManager
{
    public static BootManager Instance;

    public override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        OnSDKReady();
    }

    public void OnSDKReady()
    {
        FoxTools.functionManager.ExecuteAfterWaiting(1.5f, () =>
        {
            switch (constantManager.SCENE_LOOPING_TYPE)
            {
                case Fox_SceneLooping_Type.OBJECT_LOOPING:
                    NextLevel();
                    break;
                case Fox_SceneLooping_Type.SCENE_LOOPING:
                    int index = GetModedLevelNumber() + 1;
#if PUBLISHER_SDK_INSTALLED
                    index++;
#endif
                    FoxTools.sceneManager.LoadLevel(index);
                    break;
            }
        });
    }
}
