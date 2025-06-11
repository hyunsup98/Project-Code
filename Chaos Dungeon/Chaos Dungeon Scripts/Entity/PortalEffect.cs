using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    [SerializeField] PixelShader shader;

    float time = 0;

    private void OnEnable()
    {
        time = 0;
    }

    private void OnDisable()
    {
        shader.pixelate = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(shader == null)
        {
            shader = Camera.main.GetComponent<PixelShader>();
        }
        time += GameManager.deltaTime;
        if(time > 1.5)
        {
            gameObject.SetActive(false);
        }
        else if (time > 1)
        {
            shader.pixelate -= GameManager.deltaTime * 150;
            if (shader.pixelate <= 3)
            {
                shader.pixelate = 3;
            }
        }
        else
        {
            shader.pixelate += GameManager.deltaTime * 100;
        }
    }
}
