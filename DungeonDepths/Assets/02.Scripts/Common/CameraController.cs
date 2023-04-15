using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 delta = new Vector3(0f, 2f, 5f);    // 카메라 offset 값 : 위치 조절용
	public GameObject player = null;
	public Camera mainCamera;
	public Transform cameraPivotTr;
	Vector3 offset;
	float zoomSpeed = 10f;
	float rotationX = 0.0f;         // X축 회전값
	float rotationY = 0.0f;         // Y축 회전값
	float speed = 100.0f;           // 회전속도

	void Awake()
	{
		player = GameObject.FindWithTag("Player").gameObject;
		
	}
	void Start()
	{
		transform.position = player.transform.position + delta;
		transform.LookAt(player.transform.position + Vector3.up * 1f);
		offset = transform.position - player.transform.position;
	}
	void LateUpdate()
	{
		transform.position = player.transform.position + offset;
		Rotate();
		Zoom();
		offset = transform.position - player.transform.position;
	}
	void Zoom()
	{
		float distance = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		float newDeltaMagnitude = delta.magnitude - distance;
		newDeltaMagnitude = Mathf.Clamp(newDeltaMagnitude, 2f, 10f);

		delta = delta.normalized * newDeltaMagnitude;
	}
	void Rotate()
	{
		// 마우스가 눌러지면,
		// 마우스 변화량을 얻고, 그 값에 델타타임과 속도를 곱해서 회전값 구하기
		float _rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
		float _rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

		// 회전량에 현재 x축 회전값(rotationY)을 더하여 최소값과 최대값을 제한
		rotationX += _rotationX;
		rotationY -= _rotationY;
		rotationY = Mathf.Clamp(rotationY, 5f, 70f);

		// 각 축으로 회전
		transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

		// 카메라 위치 조정
		Vector3 _negDistance = new Vector3(0.0f, 0.0f, -delta.magnitude);
		Vector3 _position = transform.rotation * _negDistance + player.transform.position;
		transform.position = _position + Vector3.up * 1f;
	}
}
