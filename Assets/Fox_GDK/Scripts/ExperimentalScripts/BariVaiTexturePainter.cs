using UnityEngine;
using UnityEngine.UI;

public class BariVaiTexturePainter : MonoBehaviour
{
    public MeshRenderer mrend;
    public TexturePaint texturePaint;
    public LayerMask rayLayerMask;

    void Start()
    {
        texturePaint.Init(mrend);

    }

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    RaycastHit raycastHit = new RaycastHit();
        //    raycastHit.FOXE_GetRaycastFromScreenTouch(Vector3.zero, 1 << gameManager.constantManager.DESTINATION_LAYER);
        //    if (raycastHit.collider != null)
        //    {
        //        texturePaint.PaintWithCustomBrush(raycastHit.textureCoord);
        //        texturePaint.Apply();
        //    }
        //}
    }
}

[System.Serializable]
public class TexturePaint
{
    public int textureSize = 1024;
    public int brushSize = 10;
    public Color initialColor;
    public Color paintedColor;
    public Texture2D brushTexture;
    public string paintTexName = "_PaintTex";
    public int negetiveCount = 0, totalNegetivePixel = 0;
    public int positiveCount = 0, totalPositivePixel = 0;

    Material m;
    Texture2D paintData;
    Color32[] colors;
    int[] progressionMap;
    int[] pixelStatus;

    public void Init(MeshRenderer mrend)
    {
        m = MonoBehaviour.Instantiate(mrend.material);
        mrend.material = m;

        paintData = m.GetTexture(paintTexName) as Texture2D;
        colors = new Color32[textureSize * textureSize];
        progressionMap = new int[colors.Length];
        totalNegetivePixel = colors.Length;
        pixelStatus = new int[colors.Length];
        for (int i = 0; i < textureSize; i++)
        {
            for (int j = 0; j < textureSize; j++)
            {
                colors[j * textureSize + i] = paintData == null? initialColor : paintData.GetPixel(i, j);
                progressionMap[j * textureSize + i] = -1;
                pixelStatus[j * textureSize + i] = -1;
            }
        }
        paintData = new Texture2D(textureSize, textureSize);
        m.SetTexture(paintTexName, paintData);
        Apply();
    }

    public virtual void PaintWithCustomBrush(Vector2 texCoord)
    {
        float xMin = texCoord.x * textureSize - brushSize / 2;
        float xMax = texCoord.x * textureSize + brushSize / 2;

        float yMin = texCoord.y * textureSize - brushSize / 2;
        float yMax = texCoord.y * textureSize + brushSize / 2;

        int currentFrameNegetiveCount = 0;
        int currentFramPositiveCount = 0;
        int current_Positive_SkippingPixels = 0;
        int current_Negetive_SkippingPixels = 0;
        for (int x = (int)xMin; x < (int)xMax; x++)
        {
            if (x < 0 || x >= textureSize)
                continue;
            for (int y = (int)yMin; y < (int)yMax; y++)
            {
                if (y < 0 || y >= textureSize)
                    continue;

                float xInterpolation = (x - xMin) / (xMax - xMin);
                float yInterpolation = (y - yMin) / (yMax - yMin);

                Vector2 brushPixel = new Vector2(brushTexture.width * xInterpolation, brushTexture.width * yInterpolation);
                Color32 pixelColor = brushTexture.GetPixel((int)brushPixel.x, (int)brushPixel.y);

                if (pixelColor.a >= 1f)
                {
                    colors[y * textureSize + x] = pixelColor * paintedColor;
                    if (progressionMap[y * textureSize + x] == -1)
                    {
                        currentFrameNegetiveCount++;
                    }
                    else if (progressionMap[y * textureSize + x] == 1)
                    {
                        currentFramPositiveCount++;
                    }
                    else
                    {
                        if (pixelStatus[y * textureSize + x] == 1)
                            current_Positive_SkippingPixels++;
                        else if (pixelStatus[y * textureSize + x] == -1)
                            current_Negetive_SkippingPixels++;
                    }
                    progressionMap[y * textureSize + x] = 0;
                }
            }
        }
        positiveCount += currentFramPositiveCount;
        currentFramPositiveCount += current_Positive_SkippingPixels - current_Negetive_SkippingPixels;
        negetiveCount += Mathf.Clamp(currentFrameNegetiveCount - currentFramPositiveCount, 0, currentFrameNegetiveCount);
    }

    public void Apply()
    {
        paintData.SetPixels32(colors);
        paintData.Apply();
    }
}