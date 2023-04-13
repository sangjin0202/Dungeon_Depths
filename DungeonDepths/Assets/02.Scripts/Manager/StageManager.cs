using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{

    public int stageIdx = 0; //  스테이지 인덱스
    public GameObject[] stages; // 스테이지
    //[HideInInspector]
    //public string registery; // 노드 속성
    //[HideInInspector]
    //public bool isClear = false;
    //Stage1 stage1;

    public GameObject _player = null;
    public bool isAporPortal = false;// 포탈에 접근 했는지
    public GameObject portal;

    public void Awake()
    {
        

    }

    public void MoveStage() // 플레이어에서 호출 OntrigerrEnter를 이용 태그가 포탈이라면
	{
        // 조건문으로 클리어한 인덱스 검사한 뒤 클리어 하지 못한 노드라면
        // 스테이지의 현재 인덱스의 게임오브젝트(스테이지 맵)을 비활성화 한뒤
        // 클릭한 노드로 이동 그 노드가 연결된 스테이지의 인덱스를 찾아 맵 활성화
        // 포탈을 이용한 플레이어 위치 재설정
	}


}
