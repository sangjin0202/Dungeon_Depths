using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    string playerTag = "Player";
	[SerializeField]
	ParticleSystem[] ps = new ParticleSystem[4];
    private void Awake()
    {
		// 파이클 배열에 옮겨담기
	}

	private void OnEnable()
	{
		// 맵 테마 받아와서 테마에 맞는 색의 포탈 활성화 시켜주기
	}

	private void OnDisable()
	{
		// 비활성화	
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(playerTag))
		{
			//UI.띄우기 (매니저에서 함수호출)
		}
	}
	
}
