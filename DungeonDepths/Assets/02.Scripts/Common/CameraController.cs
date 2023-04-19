using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 delta = new Vector3(0f, 2f, 5f);    // 카메라 offset 값 : 위치 조절용
	public GameObject player = null;
	//public Camera mainCamera;
	Vector3 offset;
	float zoomSpeed = 10f;
	float rotationX = 0.0f;         // X축 회전값
	float rotationY = 0.0f;         // Y축 회전값
	float speed = 100.0f;           // 회전속도
	string playerTag = "Player";
	void Awake()
	{
		//mainCamera = GetComponent<Camera>();
		player = GameObject.FindWithTag(playerTag).gameObject;
		
		transform.position = player.transform.position + delta;
		transform.LookAt(player.transform.position + Vector3.up * 1f);
		offset = player.transform.position - transform.position;
	}
	//void Start()
	//{
	//	transform.position = player.transform.position + delta;
	//	transform.LookAt(player.transform.position + Vector3.up * 1f);
	//	offset = transform.position - player.transform.position;
	//}
	void LateUpdate()
	{
        // offset 만큼 거리를 두고 플레이어를 쫓아간다.
        transform.position = player.transform.position + offset;
		Rotate();
		Zoom();
		//offset = transform.position - player.transform.position;
		offset = player.transform.position - transform.position;


		Vector3 direction = (player.transform.position - transform.position);
		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized + Vector3.up * 1.2f, direction.magnitude,
							1 << LayerMask.NameToLayer("EnvironmentObject"));

		//Debug.DrawRay(transform.position, direction + Vector3.up * 1.2f, Color.red, 1f);

		for (int i = 0; i < hits.Length; i++)
		{
			TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

			for (int j = 0; j < obj.Length; j++)
			{
				//Debug.Log("Transparent배열 : "+obj[j].name);
				obj[j]?.BecomeTransparent();
			}
		}

		//RaycastHit hit;
		//if(Physics.Raycast(transform.position, offset, out hit) && !hit.collider.CompareTag(playerTag))
		//{
		//	Debug.Log("카메라가 찍는 물체의 태그 : " + hit.collider.name);
		//	transform.position = player.transform.position + new Vector3(0, 1.5f, -0.8f);
		//}
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
		// x,y축 방향으로의 마우스 회전변화량을 얻고, 그 값에 델타타임과 속도를 곱한다.
		float _rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
		float _rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

		// 각각 카메라의 x축 회전, y축 회전 값을 저장할 변수에 변화량을 더해준다.
		rotationX += _rotationX;
		rotationY -= _rotationY;
		// y축 방향으로의 카메라 회전에 상한값과 하한값을 추가한다.
		rotationY = Mathf.Clamp(rotationY, 5f, 70f);

		// 카메라의 회전을 반영해준다. 
		transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

		// 카메라 위치 조정
		Vector3 _negDistance = new Vector3(0.0f, 0.0f, -delta.magnitude);
		Vector3 _position = transform.rotation * _negDistance + player.transform.position;
		transform.position = _position + Vector3.up * 1f;
	}
}
