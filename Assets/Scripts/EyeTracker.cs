using UnityEngine;
using UnityEngine.Events;

public class EyeTracker : BaseGameBehaviour
{
    [SerializeField] private int requestedWidth = 320;
    [SerializeField] private int requestedHeight = 240;
    [SerializeField] private float sensitivity = 0.1f;

    public UnityEvent OnLookLeft;
    public UnityEvent OnLookRight;
    public UnityEvent OnLookUp;
    public UnityEvent OnLookDown;
    public UnityEvent OnBlink;

    private WebCamTexture webCam;
    private Color32[] pixels;
    private Vector2 faceCenter;
    private float lastDarkPixels;

    public override void Start()
    {
        base.Start();
        webCam = new WebCamTexture(requestedWidth, requestedHeight);
        webCam.Play();
        faceCenter = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        if (!webCam.isPlaying || !webCam.didUpdateThisFrame) return;

        pixels = webCam.GetPixels32();
        Vector2 newFace = DetectFacePosition();
        Vector2 movement = newFace - faceCenter;

        if (Mathf.Abs(movement.x) > sensitivity)
            (movement.x > 0 ? OnLookRight : OnLookLeft)?.Invoke();
        
        if (Mathf.Abs(movement.y) > sensitivity)
            (movement.y > 0 ? OnLookUp : OnLookDown)?.Invoke();

        float darkPixels = CountDarkPixels();
        if (darkPixels < lastDarkPixels * 0.7f)
            OnBlink?.Invoke();

        faceCenter = Vector2.Lerp(faceCenter, newFace, 0.3f);
        lastDarkPixels = darkPixels;
    }

    private Vector2 DetectFacePosition()
    {
        int w = webCam.width;
        int h = webCam.height;
        float totalX = 0, totalY = 0;
        int skinCount = 0;

        for (int i = 0; i < pixels.Length; i += 4)
        {
            if (IsSkinTone(pixels[i]))
            {
                totalX += (i % w);
                totalY += (i / w);
                skinCount++;
            }
        }

        return skinCount > 0 ? new Vector2(totalX / skinCount / w, totalY / skinCount / h) : faceCenter;
    }

    private bool IsSkinTone(Color32 c)
    {
        return c.r > 95 && c.g > 40 && c.b > 20 &&
               c.r > c.g && c.r > c.b &&
               Mathf.Abs(c.r - c.g) > 15;
    }

    private float CountDarkPixels()
    {
        int count = 0;
        for (int i = 0; i < pixels.Length; i += 10)
            if (pixels[i].r + pixels[i].g + pixels[i].b < 100)
                count++;
        return count;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (webCam != null) webCam.Stop();
    }
}
