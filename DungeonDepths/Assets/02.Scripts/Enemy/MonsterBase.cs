using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    #region Monster Stats
    protected MonsterStats stats;
    private float attackDistance;
    private float traceDistance;
    private float traceDisOffset = 2f;
    private float stopDistance = 0.2f;
    private float rotSpeed = 90f;
    private float searchSpeed = 1f;
    public float TraceDistance
    {
        get => traceDistance;
        set => traceDistance = value;
    }
    public float AttackDistance
    {
        get => attackDistance;
        set => attackDistance = value;
    }
    #endregion
    public WayPoints wayPoints;
    private GameObject target;
    protected GameObject curTarget;
    protected Animator anim;
    #region state관련 변수
    public enum eMonsterState { Idle, Patrol, Trace, Attack, Die }
    private StateMachine<MonsterBase> sm;

    public float PrevTime { get; set; } = 0f;
    public float IdleTime { get; set; } = 0f;
    public float LastAttackTime { get; set; } = 0f;
    public float LastSearchTime { get; set; } = 0f;
    public Animator Anim 
    {
        get => anim; 
    }
       
    #endregion
    protected virtual void Awake()
    {
        target = GameObject.FindWithTag("Player");
        wayPoints = GetComponent<WayPoints>();
        anim = GetComponent<Animator>();
        #region state 설정
        sm = new StateMachine<MonsterBase>();
        sm.AddState((int)eMonsterState.Idle, new MonsterState.Idle());
        sm.AddState((int)eMonsterState.Patrol, new MonsterState.Patrol());
        sm.AddState((int)eMonsterState.Trace, new MonsterState.Trace());
        sm.AddState((int)eMonsterState.Attack, new MonsterState.Attack());
        sm.AddState((int)eMonsterState.Die, new MonsterState.Die());
        sm.InitState(this, sm.GetState((int)eMonsterState.Idle));
        #endregion

    }
    protected virtual void Update()
    {
        sm.Execute();
    }
    public abstract void Init();
    public void CheckIdleState()
    {
        if (curTarget != null)
        { 
            sm.ChangeState(sm.GetState((int)eMonsterState.Trace));
        }
        else if (IdleTime >= Random.Range(3f, 5f))
            sm.ChangeState(sm.GetState((int)eMonsterState.Patrol));
    }
    public void CheckPatrolState()
    {
        SearchTarget();
        if (curTarget != null && (CheckRadius(curTarget.transform.position, traceDistance)))
            sm.ChangeState(sm.GetState((int)eMonsterState.Trace));
        else if (wayPoints.CheckDestination(stopDistance,transform.position))
        { 
            sm.ChangeState(sm.GetState((int)eMonsterState.Idle));
        }
    }
    public void CheckTraceState()
    {
        if (CheckRadius(curTarget.transform.position, attackDistance))
        { 
            sm.ChangeState(sm.GetState((int)eMonsterState.Attack));
        }
        else if (CheckRadius(curTarget.transform.position, traceDistance))
        {
            MoveAndRotate(curTarget.transform.position);
        }
        else if (!CheckRadius(curTarget.transform.position, traceDistance) && curTarget == null)
        { 
            sm.ChangeState(sm.GetState((int)eMonsterState.Idle));
        }
    }
    public void CheckAttackState()
    {
        if (CheckRadius(curTarget.transform.position, attackDistance)) // 공격 거리 체크
        {
            if (Time.time > LastAttackTime) // 공격 주기 체크
            {
                //ToDo 주은. Player가 Damage받는거 호출해오기
                Vector3 dir = curTarget.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(dir);
                LastAttackTime = Time.time + stats.AttackSpeed;
            }
            else
                sm.ChangeState(sm.GetState((int)eMonsterState.Trace));
        }
        
    }
    public void MoveAndRotate(Vector3 _targetPos)
    {
        Vector3 dir = _targetPos - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, stats.MoveSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
    }
    bool CheckRadius(Vector3 _targetPoint, float _radius)  //타겟 위치, 비교 거리
    {
        Vector3 dir = _targetPoint - transform.position;
        float radiusSqr = _radius * _radius;        // r^2 : 반경 넓이 비교
        if (dir.sqrMagnitude < radiusSqr)
            return true;
        return false;
    }
    public void SearchTarget()    //목표물 탐색 및 설정
    {
        if (Time.time < LastSearchTime) return;
        LastSearchTime = Time.time + searchSpeed;

        // 거리 범위 내의 플레이어 검색
        if (CheckRadius(target.transform.position, traceDistance + traceDisOffset))
        {
            curTarget = target;
        }
        else curTarget = null;
        //Debug.Log("Current Target : " + curTarget);
    }
}