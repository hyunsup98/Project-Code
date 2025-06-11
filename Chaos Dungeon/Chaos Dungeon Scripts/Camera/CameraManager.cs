using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] Transform target;

    public Transform minSize, maxSize;
    public float cameraPosZ = -10;
    public float smoothSpeed = 2;

    private Vector3 cameraVec;
    private float cameraHalfWidth, cameraHalfHeight;

    void Start()
    {
        //aspect => 해상도를 계산한 비율(Width/Height), orthographicSize => 카메라의 사이즈
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;

        target = GameManager.Instance.player.transform;
    }
    void LateUpdate()
    {
        if(target == null)
        {
            target = GameManager.GetPlayer().transform;
        }
        if(minSize == null || maxSize == null)
        {
            maxSize = GameManager.Map.camera_MaxSize;
            minSize = GameManager.Map.Camera_MinSize;
        }
        cameraVec = new Vector3(
            Mathf.Clamp(target.position.x, minSize.position.x + cameraHalfWidth, maxSize.position.x - cameraHalfWidth),
            Mathf.Clamp(target.position.y, minSize.position.y + cameraHalfHeight, maxSize.position.y - cameraHalfHeight),
            cameraPosZ);
        transform.position = Vector3.Lerp(transform.position, cameraVec, smoothSpeed * Time.deltaTime) ;
    }

    //카메라 이동 및 범위 제한 두기
    public void SetPosSize(Vector3 cameraPos, Transform maxSize, Transform minSize)
    {
        transform.position = cameraPos;
        this.maxSize = maxSize;
        this.minSize = minSize;
    }
}
