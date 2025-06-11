using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    [SerializeField] Camera mainCamera;
    public float range = 2f;
    public Vector3 pos;
    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float size = mainCamera.orthographicSize;
        if (Mathf.Abs(Mathf.Abs(size)- Mathf.Abs(range)) > 0.001)
        {
            float move_size = range - size;
            if (move_size < 0 && move_size > -0.5f) move_size = -0.5f;
            if (move_size > 0 && move_size < 0.5f) move_size = 0.5f;

            mainCamera.orthographicSize += move_size * Time.deltaTime;
        }
        else
        {
            mainCamera.orthographicSize = range;
        }
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }
}
