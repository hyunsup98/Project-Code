using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BloadShader : MonoBehaviour
{
    [Range(0, 1)] public float rgbSplit;
    public Material shadow;

    private void Update()
    {
        rgbSplit -= GameManager.deltaTime;
        if(rgbSplit <= 0)
        {
            rgbSplit = 0;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (GameManager.Speed > 0)
        {
            shadow.SetFloat("_RGBSplit", rgbSplit);
            Graphics.Blit(source, destination, shadow);
        }
    }
}
