using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbow : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    float r = 1, g = 1, b = 1;

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = new Color(1,1,1);
    }
}
