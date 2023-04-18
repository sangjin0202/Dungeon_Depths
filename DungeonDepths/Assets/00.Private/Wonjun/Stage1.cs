using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stage1 : MonoBehaviour
{
	//public Transform[] spawnPoints; // spawn 
	//public Transform BMSpawnPoints;
	//public GameObject monsterPrefab;
	//public GameObject bossMonsterPrefab;
	//public float createTime;
	//public int maxMonster = 10;
	//public float dist; // 플레이어와 포탈과의 거리
	//public Transform playerTr;
	//public float portalRange = 3f; // 포탈생성하기 위한 허용범위
	//public Transform portalsTr;
	//// private int clearCnt;
	//public bool isStageClear = false;
	//public GameObject portal;
	//public ParticleSystem portalParticles;
	//public Transform thisTr;

	//public struct NodeInfo
	//{
	//	public bool isClear;

	//	public NodeInfo(bool isClear)
	//	{
	//		this.isClear = isClear;
	//	}
	//}


	//public NodeInfo newNode1 = new NodeInfo(false);
	//public NodeInfo newNode2 = new NodeInfo(false);
	//public NodeInfo newNode3 = new NodeInfo(false);
	//public NodeInfo newNode4 = new NodeInfo(false);


	//void Awake()
	//{
	//	playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
	//	portalParticles = GameObject.FindWithTag("Portal1").GetComponent<ParticleSystem>();
	//	portalsTr = GameObject.Find("Portal").GetComponent<Transform>();


	//	portalParticles.Stop();
	//	thisTr = GetComponent<Transform>();

	//}

	//void Update()
	//{
	//	//if (clearCnt == 4)
	//	//{
	//	//  CallBossMonster();

	//	//}

	//	// if (isStageClear)
	//	{
	//		CallPortal();
	//	}

	//	//if (Input.GetKeyDown(KeyCode.L))
	//	//{
	//	//	StageManager.Instance.Move(thisTr.position);
	//	//}
	//}
	//public void CallBossMonster()
	//{
	//	Instantiate(bossMonsterPrefab, BMSpawnPoints.position, BMSpawnPoints.rotation);
	//}

	//public void CallPortal()
	//{
	//	float dist = Vector3.Distance(playerTr.position, portalsTr.position);


	//	if (dist < portalRange)
	//	{
	//		StageManager.Instance.isAporPortal = true;// 포탈에 접근하면

	//		portalParticles.Play();
	//	}
	//	else
	//		portalParticles.Stop();

	//}


}
