using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    //카메라를 비추는 타겟
    public Transform target;

    [Header("카메라 포지션")]
    //카메라 위치, 회전값 변수
    public float cameraDistance;
    public float rotX, rotY;
    float zoom;

    [Header("마우스(카메라) 회전")]
    //회전감도, x축, y축 최소&최대 회전 각도
    public float rotSensitive, zoomSensitive;
    public float rotationMinX, rotationMaxX;
    public float zoomMin, zoomMax;
    [SerializeField] private bool fPersonView = false;

    bool isShake = false;

    //플레이어와 카메라 사이의 벽이 있는지 체크하는 레이캐스트 관련 변수
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;


    void Update()
    {
        if (GameManager.Instance.gameState != GameState.Play)
            return;

        if (Input.GetKeyDown(KeyCode.F5))
            fPersonView = !fPersonView;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isShake)
            SetTransform();

        CameraMove();
    }

    //타겟의 포지션에 따른 카메라 이동
    void CameraMove()
    {
        //카메라가 타겟을 따라다니게 하기
        if(target != null && !isShake)
        {
            Vector3 targetPos = target.position;
            targetPos.y += 1.55f;
            transform.position = targetPos;
            
            if(!fPersonView)
                transform.position += -(transform.forward * zoom);
        }
    }

    //마우스 입력에 따른 카메라 이동
    void SetTransform()
    {
        if (GameManager.Instance.gameState != GameState.Play)
            return;

        //카메라를 마우스 입력값에 따라 회전하기
        rotY += Input.GetAxisRaw("Mouse X") * rotSensitive;       //좌우 회전
        rotX += Input.GetAxisRaw("Mouse Y") * rotSensitive * -1;    //상하 회전
        rotX = Mathf.Clamp(rotX, rotationMinX, rotationMaxX);

        if (!fPersonView)
        {
            //카메라 줌인, 줌아웃
            zoom += Input.GetAxisRaw("Mouse ScrollWheel") * zoomSensitive * -1;

            Vector3 direction = transform.position - target.position;

            if (Physics.Raycast(target.position, direction, out hit, zoom, layerMask))
            {
                zoom = hit.distance - 0.3f;
            }

            zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        }
        
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }

    //카메라 흔들기 기능
    public IEnumerator Shake(float shakeAmount, float shakeTime)
    {
        float timer = 0;
        isShake = true;
        Vector3 cameraOriginPos = transform.position;

        while(timer <= shakeTime)
        {
            transform.position += Random.insideUnitSphere * shakeAmount;
            timer += Time.deltaTime;
            yield return null;
        }
        
        transform.position = cameraOriginPos;
        isShake = false;
    }

    public void StartSlowMotion(float slowMin, float slowTime)
    {
        StartCoroutine(SlowMotion(slowMin, slowTime));
    }

    //slowMin은 게임타임의 최소값, slowTime은 슬로우모션이 발동하는 시간
    private IEnumerator SlowMotion(float slowMin, float slowTime)
    {
        Time.timeScale = slowMin;

        while(Time.timeScale < 1)
        {
            Time.timeScale += (1 - slowMin) / slowTime * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, slowMin, 1f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            yield return null;
        }
    }
}
