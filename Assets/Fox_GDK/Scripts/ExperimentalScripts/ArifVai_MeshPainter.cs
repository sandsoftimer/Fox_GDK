using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArifVai_MeshPainter : MonoBehaviour
{
    public MeshRenderer mrend;
    public LayerMask rayLayerMask;

    public TexPaint texPaint;


    void Start()
    {
        texPaint.Init(mrend);

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(r, out rch, 100, rayLayerMask))
            {
                texPaint.Paint(rch.textureCoord);
                texPaint.Apply();
            }
        }
    }


}
[System.Serializable]
public class TexPaint
{
    public int textureSize = 1024;
    public int brushSize = 10;
    public Color initialColor;
    public Color paintedColor;
    public string paintTexName = "_PaintTex";
    Material m;
    Texture2D paintData;
    Color[] colors;
    public void Init(MeshRenderer mrend)
    {
        m = MonoBehaviour.Instantiate(mrend.material);
        mrend.material = m;
        paintData = new Texture2D(textureSize, textureSize);
        colors = new Color[textureSize * textureSize];
        for (int i = 0; i < textureSize * textureSize; i++)
        {
            colors[i] = initialColor;
        }
        m.SetTexture(paintTexName, paintData);
        Apply();
    }

    public virtual bool Paint(Vector2 texCoord)
    {
        int pcount = 0;
        int c = Mathf.RoundToInt(texCoord.x * textureSize);
        int r = Mathf.RoundToInt(texCoord.y * textureSize);
        for (int ri = 0; ri < textureSize; ri++)
        {
            for (int ci = 0; ci < textureSize; ci++)
            {
                int rdif = Mathf.Abs(ri - r);
                int cdif = Mathf.Abs(ci - c);

                bool inbound = ((rdif * rdif + cdif * cdif) < brushSize * brushSize);

                if (inbound)
                {
                    pcount++;
                    colors[ri * textureSize + ci] = paintedColor;
                }
            }
        }
        return pcount > 0;
    }

    public void Apply()
    {
        paintData.SetPixels(colors);
        paintData.Apply();
    }
}