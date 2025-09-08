using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBar : MonoBehaviour
{

    public Material matInside;
    public float fill;
    bool fillStart;
    float fillingTime = 1;
    public float horizontalScale;
    public GameObject ball;
    public Vector3 loc;
    // Start is called before the first frame update
    void Start()
    {
        Material mat = GetComponent<Renderer>().material;
        matInside = new Material(mat);
        //matInside = mat;
        GetComponent<Renderer>().material = matInside;

        //matInside.SetFloat("colorFill", 0.5f);
        //matInside.SetFloat("center", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fillStart)
        {
            fill += Time.deltaTime*(1/fillingTime);
            matInside.SetFloat("colorFill", fill);
            if (fill >= 1)
            {
                fill = 1;
                fillStart = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)){
            FillIt(ball.transform) ;
        }
    }
    public void FillIt( Transform ballT  )
    {
        float c = 0;
        Vector3 localPos = transform.InverseTransformPoint(ballT.position) ;
        loc = localPos;
        if(horizontalScale == 0)
        {
            return;
        }
        float frac = localPos.x*10f / horizontalScale;
        //Debug.Log("frac " + frac);
        c = frac;

        matInside.SetFloat("colorFill", 0f);
        matInside.SetFloat("center", c);
        fillStart = true;

    }
}
