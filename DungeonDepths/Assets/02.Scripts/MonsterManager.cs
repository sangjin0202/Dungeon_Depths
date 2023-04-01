using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterManagerState;
public class MonsterManager : MonoSingleton<MonsterManager>
{

    //ToDo: 몬스터가 특정 지점에서 죽으면 가장 가까운 스폰지점에서 부활
    // or 그냥 아무 스폰지점에서 부활
    // 스테이지 시작시 몬스터를 생성할 지점들과 리스폰지점을 분리할지 생각

    /*
     * Wait : 스테이지(노드)시작 전 대기 상태
     * Spawn : 스테이지(노드)시작 후 몬스터 생성 , 리스폰
     * Finish : 플레이어 사망으로 인한 게임오버 / 스테이지 클리어
     * 상태기계를 통해 상태를 바꿀경우 바로 해당 상태에 돌입한다(Enter 실행)
     */
    public enum MonsterManagerStates { Wait, Spawn, Finish };

    public StateMachine<MonsterManager> stateMachine;

    public int totalMeleeNum; // 생성할 근거리 몬스터 최대 수
    public int totalRangeNum; // 생성할 원거리 몬스터 최대 수

    public GameObject spawnPointsGroup;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> meleeMonsters = new List<GameObject>();
    public List<GameObject> rangeMonsters = new List<GameObject>();


    public float respawnTime = 5f;
    public float prevTime;
    public float curTime;

    //public float rangeSpawnTime;
    //public bool isStageStart;
    //public bool isGameOver = false;

    public List<Vector3> deadSpots; // 몬스터가 죽은 위치를 저장할 리스트

    public bool isGameClear;
    public bool isPlayerDead = false; // 플레이어 사망을 확인할 플래그
    public bool isRespawn = false;
    void Awake()
    {
        
        prevTime = Time.time;
        //rangeSpawnTime = new WaitForSeconds(2.0f);

        stateMachine = new StateMachine<MonsterManager>();

        stateMachine.AddState((int)MonsterManagerStates.Wait, new MonsterManagerState.Wait());
        stateMachine.AddState((int)MonsterManagerStates.Spawn, new MonsterManagerState.Spawn());
        stateMachine.AddState((int)MonsterManagerStates.Finish, new MonsterManagerState.Finish());

        //첫 상태 설정
        stateMachine.InitState(this, stateMachine.GetState((int)MonsterManagerStates.Wait));
        totalMeleeNum = 20;
        //totalRangeNum = 10;
        var spawnPointsGroup = GameObject.Find("SpawnPoints");
        if(spawnPointsGroup != null)
        {
            spawnPointsGroup.GetComponentsInChildren<Transform>(spawnPoints);
        }
        spawnPoints.RemoveAt(0);
    }

    private void Start()
    {
        //몬스터들을 스폰
        stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Spawn));
    }
    private void Update()
    {
        // 게임시작 후 
        if(Time.time - prevTime >= respawnTime && isRespawn)
        {
            stateMachine.Execute();
        }

        CheckFinish();
        if(stateMachine.CurrentState == stateMachine.GetState((int)MonsterManagerStates.Finish))
        {
            stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Wait));
        }
    }
    void CheckFinish()
    {
        if(isGameClear || isPlayerDead)
        {
            Debug.Log(stateMachine.CurrentState);
            //Finish상태로 변경
            stateMachine.ChangeState(stateMachine.GetState((int)MonsterManagerStates.Finish));
            Debug.Log(stateMachine.CurrentState + "sasdasd");
        }
    }

    /*
     * 몬스터 객체가 죽으면 호출 
     * 죽은 시간과 위치를 기록
     * 몬스터 리스트에서 죽은 몬스터를 제외시킴
     */
    public void RemoveMonster(GameObject monster)
    {
        prevTime = Time.time; // 몬스터가 사망한 시간 기록
        isRespawn = true;

        //살아있는 근접 몬스터의 리스트중 죽은 객체가 존재한다면
        if(meleeMonsters.Contains(monster))
        {

            //해당 객체가 죽은 위치의 정보를 List에 담는다.
            deadSpots.Add(monster.transform.position);

            //해당 객체를 List에서 제거한다.
            meleeMonsters.Remove(monster);
        }
    }
}