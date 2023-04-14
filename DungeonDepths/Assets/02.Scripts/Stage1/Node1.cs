//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Node1 : Stage1
//{
//    void Start()
//    {

//        spawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

//        newNode1.isClear = false;
//        if (spawnPoints.Length > 0)
//        {
//            //몬스터 생성 코루틴 함수 호출
//            StartCoroutine(this.CreateMonster());
//        }

//    }

//    IEnumerator CreateMonster()
//    {
//        //게임 종료 시까지 무한 루프
//        while (!newNode1.isClear)
//        {
//            //현재 생성된 몬스터 개수 산출
//            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;
//            if (monsterCount < maxMonster)
//            {
//                //몬스터의 생성 주기 시간만큼 대기
//                yield return new WaitForSeconds(createTime);

//                //불규칙적인 위치 산출
//                int idx = Random.Range(1, spawnPoints.Length);
//                //몬스터의 동적 생성
//                Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
//            }
//            else
//            {
//                yield return null;
//            }
//        }
//    }

//    void Update()
//    {
      
//    }
//    public void Clear()
//    {
//        newNode1.isClear = true;
//        CallPortal();
//    }

//    public void call()
//   {

//    }

//}
