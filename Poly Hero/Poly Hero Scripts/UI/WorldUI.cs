using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    protected Transform target;
    protected float posY;

    private void Update()
    {
        SetTransform();
    }

    public virtual void SetUIData(Transform target, float posY)
    {
        this.target = target;
        this.posY = posY;
        SetTransform();
    }

    private void SetTransform()
    {
        Vector3 pos = target.transform.position;
        pos.y += posY;
        transform.position = pos;
    }

    private void OnDisable()
    {
        target = null;
    }
}
