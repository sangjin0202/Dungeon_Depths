using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMap : MapEntity
{
    public List<Vector3> boxSpawnPoints = new List<Vector3>();    // 보물 상자 스폰 Points
    public List<Vector3> EnemySpawnPoints = new List<Vector3>();  // 몬스터 스폰 Points    
    [SerializeField]
    private MapCore core; public MapCore Core { get; private set; }//코어

    public override void Awake()
    {
        base.Awake();
        core = GetComponentInChildren<MapCore>();
        /* TODO 주은
         * 상자, 몬스터 스폰 위치 설정 및 파싱
         * 시작 위치 설정 및 파싱
         * core 파괴 이벤트 설정
        */
        core.OnEvent += () => { IsClear = true; Debug.Log("Map 이벤트 호출 : " + IsClear); };
    }
    // TODO 주은 상자 스폰 함수 구현?
}
