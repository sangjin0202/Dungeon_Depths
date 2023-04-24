using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
public class MonsterManager : MonoSingleton<MonsterManager>
{
    List<GameObject> aliveMonsterList = new List<GameObject>();
    //private float respawnTime = 5f;
    private float prevTime;
    private float curTime;

    public bool isGameClear;    // 추후 수정
    public bool isPlayerDead = false; // 플레이어 사망을 확인할 플래그 | 추후 수정
    public bool isRespawn = false;

    void Awake()
    {
        prevTime = Time.time;

    }

    public void SpawnMonsters(MonsterID _monsterID, List<Vector3> _points, int _totalMonsterNum)
    {
        if (_points.Count < _totalMonsterNum)
        {
            // 스크립터블 오브젝트에서 _totalMonsterNum 수정
            Debug.LogError("_totalMonsterNum가 points.Count 초과");
            return;
        }
        bool[] _randomCount = new bool[_points.Count];
        int _curMonsterNum = 0;
        while (_totalMonsterNum > _curMonsterNum)
        {
            int _index = Random.Range(0, _points.Count);
            if (!_randomCount[_index])
            {
                _randomCount[_index] = true;

                var _newMonster = PoolManager.Instance.Instantiate(_monsterID.ToString(), _points[_index], Quaternion.identity);
                _newMonster.transform.GetComponent<MonsterBase>().Init(StageManager.Instance.CurMap.mapData.Difficulty);
                _curMonsterNum++;
                aliveMonsterList.Add(_newMonster);
            }
            //Debug.Log("curMonsterNum " + _curMonsterNum);
            //Debug.Log("_totalMonsterNum " + _totalMonsterNum);
        }
    }



    public void DeactiveMonsterList()
    {
        foreach (var _monster in aliveMonsterList)
        {
            _monster.SetActive(false);
        }
        aliveMonsterList.Clear();
    }
}
