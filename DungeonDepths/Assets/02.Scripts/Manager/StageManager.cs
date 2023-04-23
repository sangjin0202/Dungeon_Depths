using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    List<Map> mapInfoList = new List<Map>();
    Map curMap;
    GameObject portal;
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
        portal = GameObject.FindWithTag("Portal").gameObject;
    }
    public void Start()
    {
        InitMapList();

    }
    public void MovePortal(Vector3 _targetPos, Quaternion _rot)
    {
        portal.transform.position = _targetPos;
        portal.transform.rotation = _rot;
    }

    void InitMapList()
    {
        var _map = GameObject.Find("Map").transform;
        for(int i = 0; i < _map.transform.childCount; i++)
        {
            mapInfoList.Add(_map.GetChild(i).GetComponent<Map>());
            if(i != 0)
                mapInfoList[i].gameObject.SetActive(false);
        }
        curMap = mapInfoList[0];
    }
    public void StartBossStage(Map _selectedMap)
    {

    }
    public void StartStageMap(Map _selectedMap)
    {
        UIManager.Instance.OffWindow(Window.MAP);
        curMap.gameObject.SetActive(false);
        curMap = _selectedMap;
        player.transform.position = curMap.StartPosition.position;

        if(curMap.mapData.Type == MapType.BOSS)
        {
            UIManager.Instance.OnWindowWithoutPause(Window.BOSSHPWINDOW);
        }
        else
        {
            MonsterManager.Instance.SpawnMonsters(EnumTypes.MonsterID.Chomper, curMap.gameObject.GetComponent<NormalMap>().GetWorldSpawnPoints(), curMap.mapData.TotalMonsterNum);
            curMap.gameObject.GetComponent<NormalMap>().SpawnBoxes();
        }
    }

    public void ClearStage() // TODO 추후 수정 : 이벤트 함수로 구현할지
    {
        if(curMap.mapData.Type == MapType.BOSS)
        {
            UIManager.Instance.OffWindowWithoutResume(Window.BOSSHPWINDOW);
        }
        else
        {
            //UIManager.Instance.OffWindow(Window.BOSSHPWINDOW);
            MonsterManager.Instance.DeactiveMonsterList();
        }
    }
}
