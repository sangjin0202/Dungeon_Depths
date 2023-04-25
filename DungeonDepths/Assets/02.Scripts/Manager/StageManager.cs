using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    List<Map> mapInfoList = new List<Map>();
    List<GameObject> activedChestList = new List<GameObject>();
    GameObject portal;
    GameObject player;
    Map curMap;

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
        portal.transform.position = _targetPos + new Vector3(0,3,0);
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
    public void StartStageMap(Map _selectedMap)
    {
        UIManager.Instance.OffWindowWithResume();
        curMap.gameObject.SetActive(false);
        curMap = _selectedMap;
        player.transform.position = curMap.StartPosition.position;

        if(curMap.mapData.Type == MapType.BOSS || curMap.mapData.Type == MapType.FINALBOSS)
        {
            UIManager.Instance.OnWindow(Window.BOSSHPBAR);
        }
        else
        {
            MonsterManager.Instance.SpawnMonsters(EnumTypes.MonsterID.Chomper, curMap.gameObject.GetComponent<NormalMap>().GetWorldSpawnPoints(), curMap.mapData.TotalMonsterNum);
            SpawnBoxes();
        }
    }

    public void ClearStage() // TODO 추후 수정 : 이벤트 함수로 구현할지
    {
        curMap.IsClear = true;
        UIManager.Instance.ShowSelectCardInfo();

        if (curMap.mapData.Type == MapType.BOSS)
        {
            UIManager.Instance.OffWindow();
        }
        else
        {
            MonsterManager.Instance.DeactiveMonsterList();
            DeactiveChestList();
        }
    }
    private void DeactiveChestList()
    {
        foreach (var chest in activedChestList)
        {
            chest.SetActive(false);
        }
        activedChestList.Clear();
    }
    private void SpawnBoxes()
    {
        var _carMap = curMap.gameObject.GetComponent<NormalMap>();
        if (_carMap.boxSpawnPoints.Count < _carMap.mapData.TotalBoxNum)
        {
            // 스크립터블 오브젝트에서 _totalBoxNum 수정
            Debug.LogError("TotalBoxNum가 points.Count 초과");
            return;
        }
        int _curBoxNum = 0;
        bool[] _randomCount = new bool[_carMap.boxSpawnPoints.Count];
        while (_carMap.mapData.TotalBoxNum > _curBoxNum)
        {
            int _index = Random.Range(0, _carMap.boxSpawnPoints.Count);
            if (!_randomCount[_index])
            {
                _randomCount[_index] = true;
                var _newChest = PoolManager.Instance.Instantiate("Chest", _carMap.GetWorldBoxSpawnPoints()[_index], Quaternion.identity);
                activedChestList.Add(_newChest);
                _curBoxNum++;
            }
        }
    }
}
