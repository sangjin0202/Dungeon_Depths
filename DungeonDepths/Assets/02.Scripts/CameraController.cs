using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
 
    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 2.0f, -7.0f);
    [SerializeField]
    GameObject _player = null;


    public float zoomSpeed = 10.0f;

    private Camera mainCamera;


    private Vector3 point = Vector3.zero;
    public Transform cameraTr;

    private float yRotateMove;
  

    public Vector3 prePos;




    [SerializeField, Range(0f, 100f)]
    private float hRotationSpeed = 50f;  // 좌우 회전 속도

    [SerializeField, Range(0f, 100f)]
    private float vRotationSpeed = 100f; // 상하 회전 속도

    [SerializeField, Range(-60f, 0f)]
    private float lookUpAngleLimit = -45f;  // 최소 회전각(올려다보기 제한)

    [SerializeField, Range(15f, 60f)]
    private float lookDownAngleLimit = 45f; // 최대 회전각(내려다보기 제한)
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        point = _player.transform.position;
        transform.position = _player.transform.position + _delta;
        prePos = _player.transform.position;
        cameraTr=GetComponent<Transform>();
        
    }



    void Update()
    {

        if (prePos != _player.transform.position)
        {
            var disPos = _player.transform.position - prePos;

            cameraTr.position = Vector3.MoveTowards(cameraTr.position, cameraTr.position + disPos, 5f);// player 속도와 동일
            prePos = _player.transform.position;
            
        }
        Zoom();
        Rotate();
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            mainCamera.fieldOfView += distance;
        }
    }

    private void Rotate()
    {



        float t = Time.deltaTime;

        // 마우스 움직임 감지
        float h = Input.GetAxisRaw("Mouse X") * hRotationSpeed * t;
        float v = -Input.GetAxisRaw("Mouse Y") * vRotationSpeed * t;

        // [1] 좌우 회전 : 월드 Y축 기준
        Quaternion hRot = Quaternion.AngleAxis(h, Vector3.up);
        transform.rotation = hRot * transform.rotation;

        // [2] 상하 회전 : 로컬 X축 기준
        // 다음 프레임 각도 예측
        float xNext = transform.eulerAngles.x + v;
        if (xNext > 180f)
            xNext -= 360f;

        // 상하 회전 각도 제한
        if (lookUpAngleLimit < xNext && xNext < lookDownAngleLimit)
        {
            transform.rotation *= Quaternion.AngleAxis(v, Vector3.right);
        }



    }

    
}
