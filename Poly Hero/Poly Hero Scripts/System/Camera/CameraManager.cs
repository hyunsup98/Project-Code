using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    //ī�޶� ���ߴ� Ÿ��
    public Transform target;

    [Header("ī�޶� ������")]
    //ī�޶� ��ġ, ȸ���� ����
    public float cameraDistance;
    public float rotX, rotY;
    float zoom;

    [Header("���콺(ī�޶�) ȸ��")]
    //ȸ������, x��, y�� �ּ�&�ִ� ȸ�� ����
    public float rotSensitive, zoomSensitive;
    public float rotationMinX, rotationMaxX;
    public float zoomMin, zoomMax;
    [SerializeField] private bool fPersonView = false;

    bool isShake = false;

    //�÷��̾�� ī�޶� ������ ���� �ִ��� üũ�ϴ� ����ĳ��Ʈ ���� ����
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

    //Ÿ���� �����ǿ� ���� ī�޶� �̵�
    void CameraMove()
    {
        //ī�޶� Ÿ���� ����ٴϰ� �ϱ�
        if(target != null && !isShake)
        {
            Vector3 targetPos = target.position;
            targetPos.y += 1.55f;
            transform.position = targetPos;
            
            if(!fPersonView)
                transform.position += -(transform.forward * zoom);
        }
    }

    //���콺 �Է¿� ���� ī�޶� �̵�
    void SetTransform()
    {
        if (GameManager.Instance.gameState != GameState.Play)
            return;

        //ī�޶� ���콺 �Է°��� ���� ȸ���ϱ�
        rotY += Input.GetAxisRaw("Mouse X") * rotSensitive;       //�¿� ȸ��
        rotX += Input.GetAxisRaw("Mouse Y") * rotSensitive * -1;    //���� ȸ��
        rotX = Mathf.Clamp(rotX, rotationMinX, rotationMaxX);

        if (!fPersonView)
        {
            //ī�޶� ����, �ܾƿ�
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

    //ī�޶� ���� ���
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

    //slowMin�� ����Ÿ���� �ּҰ�, slowTime�� ���ο����� �ߵ��ϴ� �ð�
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
