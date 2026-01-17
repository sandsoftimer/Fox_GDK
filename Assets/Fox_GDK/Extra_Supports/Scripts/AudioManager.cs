using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public BackgroundAudioController backgroundAudioController;

    [Header("Game Specific Sounds")]
    public AudioSource redAlertSource;
    public AudioSource slowDownSource, speedUpSource;

    [Space]
    public AudioSource activityTapSound;
    public AudioSource ButtonTapSound;
    public AudioSource gameFailSound;
    public AudioSource gameSuccessSound;
    public AudioSource taskCompleteSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
