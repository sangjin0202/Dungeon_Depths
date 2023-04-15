using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 _delta = new Vector3(0f, 2f, 5f);    // 카메라 offset 값 : 위치 조절용
    public GameObject _player = null;
    public Camera mainCamera;
    Vector3 offset;
    float zoomSpeed = 10f;
    float rotationX = 0.0f;         // X축 회전값
    float rotationY = 0.0f;         // Y축 회전값
    float speed = 100.0f;           // 회전속도

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        _player = GameObject.FindWithTag("Player").gameObject;
    }
    void Start()
    {
        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);
        offset = transform.position - _player.transform.position;
    }
    void Update()
    {
        transform.position = _player.transform.position + offset;
        Rotate();
        Zoom();
        offset = transform.position - _player.transform.position;
    }
    void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            mainCamera.fieldOfView += distance;
        }
    }
    void Rotate()
    {
        // 마우스가 눌러지면,

        // 마우스 변화량을 얻고, 그 값에 델타타임과 속도를 곱해서 회전값 구하기
        rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
        rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

        // 각 축으로 회전
        // Y축은 마우스를 내릴때 카메라는 올라가야 하므로 반대로 적용
        transform.RotateAround(_player.transform.position, Vector3.right, -rotationY);
        transform.RotateAround(_player.transform.position, Vector3.up, rotationX);

        // 회전후 타겟 바라보기
        transform.LookAt(_player.transform.position);
    }
}