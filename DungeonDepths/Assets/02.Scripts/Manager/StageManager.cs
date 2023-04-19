using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    List<Map> mapInfoList = new List<Map>();
    Map curMap;
    GameObject player;

    public Map CurMap
    {
        get => curMap;
    }
    public List<Map> GetMapInfoList()
    {
        return mapInfoList;
    }
    public void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
    }

    public void Start()
    {
        InitMapList();

    }

    void InitMapList()
    {
        var _map = GameObject.Find("Map").transform;
        for (int i = 0; i < _map.transform.childCount; i++)
        {
            mapInfoList.Add(_map.GetChild(i).GetComponent<Map>());
            if (i != 0)
                mapInfoList[i].gameObject.SetActive(false);
        }
        curMap = mapInfoList[0];
    }

    public void MoveStage() // 플레이어에서 호출 OntrigerrEnter를 이용 태그가 포탈이라면
	{
        // 조건문으로 클리어한 인덱스 검사한 뒤 클리어 하지 못한 노드라면
        // 스테이지의 현재 인덱스의 게임오브젝트(스테이지 맵)을 비활성화 한뒤
        // 클릭한 노드로 이동 그 노드가 연결된 스테이지의 인덱스를 찾아 맵 활성화
        // 포탈을 이용한 플레이어 위치 재설정
	}

    public void StartStageMap(Map _selectedMap)
    {
        UIManager.Instance.OffWindow(Window.MAP);
        curMap.gameObject.SetActive(false);
        curMap = _selectedMap;
        player.transform.position = curMap.StartPosition.position;

        MonsterManager.Instance.SpawnMonsters(EnumTypes.MonsterID.Chomper, curMap.gameObject.GetComponent<NormalMap>().GetWorldSpawnPoints(), curMap.mapData.TotalMonsterNum);

        curMap.gameObject.GetComponent<NormalMap>().SpawnBoxes();
    }

    public void ClearStage() // TODO 추후 수정 : 이벤트 함수로 구현할지
    {
        MonsterManager.Instance.DeactiveMonsterList();
    }
}
