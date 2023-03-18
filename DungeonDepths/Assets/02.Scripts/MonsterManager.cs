using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterManagerState;
public class MonsterManager : MonoSingleton<MonsterManager>{
    /*
     * Wait : 스테이지(노드)시작 전 대기 상태
     * Spawn : 스테이지(노드)시작 후 몬스터 배치
     * Regeneration : 몬스터 리스폰
     * Finish : 플레이어 사망으로 인한 게임오버 / 스테이지 클리어
     */
    public enum MonsterManagerStates { Wait, Spawn, Finish };

    public StateMachine<MonsterManager> stateMachine;

    public int totalMeleeNum; // 생성할 근거리 몬스터 최대 수
    public int totalRangeNum; // 생성할 원거리 몬스터 최대 수

    public GameObject spawnPointsGroup;

    //몬스터 스폰 지점들
    public List<Transform> spawnPoints = new List<Transform>();

    //살아있는 근접 몬스터 리스트
    public List<GameObject> meleeMonsters = new List<GameObject>();

    //살아있는 원거리 몬스터 리스트
    public List<GameObject> rangeMonsters = new List<GameObject>();

    public List<float> respawnTimes = new List<float>();
    //public List<GameObject> aliveMonsters = new List<GameObject>();

    public float respawnTime = 5f;
    public float prevTime;
    public float curTime;

    //public float rangeSpawnTime;
    //public bool isStageStart;
    //public bool isGameOver = false;

    public List<Vector3> deadSpots; // 몬스터가 죽은 위치를 저장할 리스트

    public bool isGameOver;
    public bool isPlayerDead;
    public bool isRespawn = false;
    void Awake() {

        prevTime = Time.time;
        //rangeSpawnTime = new WaitForSeconds(2.0f);

        stateMachine = new StateMachine<MonsterManager>();

        //사용할 상태들 추가
        stateMachine.AddState((int)MonsterManagerStates.Wait, new MonsterManagerState.Wait());
        stateMachine.AddState((int)MonsterManagerStates.Spawn, new MonsterManagerState.Spawn());
        stateMachine.AddState((int)MonsterManagerStates.Finish, new MonsterManagerState.Finish());

        //첫 상태 설정
        stateMachine.InitState(this, stateMachine.GetState((int)MonsterManagerStates.Wait));
        totalMeleeNum = 20;
        //totalRangeNum = 10;
        var spawnPointsGroup = GameObject.Find("SpawnPoints");
        if(spawnPointsGroup != null) {
            spawnPointsGroup.GetComponentsInChildren<Transform>(spawnPoints);
        }
        spawnPoints.RemoveAt(0);
    }

    private void Start() 
    {
        stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Spawn));
    }
    private void Update() {
        if (Time.time - prevTime >= respawnTime && isRespawn) {
            stateMachine.Execute();
        }
        
        CheckFinish();
        if (stateMachine.currentState == stateMachine.GetState((int)MonsterManagerStates.Finish)) {
            stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Wait));
        }
    }
    void CheckFinish() {
        if (isGameOver || isPlayerDead) {
            Debug.Log(stateMachine.currentState);
            stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Finish));
            Debug.Log(stateMachine.currentState + "sasdasd");
        }
    }
  
    // 몬스터 클래스에서 해당 객체가 죽으면 호출
    public void RemoveMonster(GameObject monster) {
        prevTime = Time.time;
        isRespawn = true;
        //살아있는 근접 몬스터의 리스트중 죽은 객체가 존재한다면
        if (meleeMonsters.Contains(monster)) {
            
            //해당 객체가 죽은 위치의 정보를 List에 담는다.
            deadSpots.Add(monster.transform.position);
            
            //해당 객체를 List에서 제거한다.
            meleeMonsters.Remove(monster);
        }
    }
}