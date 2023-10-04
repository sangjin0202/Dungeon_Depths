using UnityEngine;
using EnumTypes;
using System.Collections;

public abstract class MonsterBase : MonoBehaviour
{
    #region Monster Stats
    public MonsterStatData stat;
    private float damage;
    private float maxHP;
    private float curHP;
    private float attackDistance;
    private float traceDistance;
    private float rotSpeed = 90f;
    private float searchSpeed = 1f;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    public float CurHP
    {
        get => curHP;
        set => curHP = Mathf.Max(0, value);
    }
    public float MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }
    public bool IsDead { get; set; }
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
    public float StopDistance { get; private set; } = 0.2f;
    #endregion
    private WayPoints wayPoints;
    private GameObject target;
    private GameObject curTarget;
    protected Animator anim;
    protected GameObject hitBox;
    public WayPoints WayPoints { get => wayPoints; }
    public GameObject CurTarget { get => curTarget; }
    public Collider MonsterCollider { get; private set; }
    public Rigidbody MonsterRigidbody { get; private set; }
    #region state관련 변수
    public enum eMonsterStates { Idle, Patrol, Trace, Attack, Die }
    private StateMachine<MonsterBase> stateMachine;
    public float PrevIdleTime { get; set; } = 0f;   //idle 상태 시작 시간
    public float IdleTime { get; set; } = 0f;       //idle 상태 지속 시간
    public float LastAttackTime { get; set; } = 0f;
    public float LastSearchTime { get; set; } = 0f;
    public Animator Anim { get => anim; }
    #endregion
    protected virtual void Awake()
    {
        target = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        wayPoints = GetComponent<WayPoints>();
        MonsterCollider = GetComponent<Collider>();
        MonsterRigidbody = GetComponent<Rigidbody>();
        hitBox = transform.GetChild(0).gameObject;
        hitBox.SetActive(false);
        #region state 설정
        stateMachine = new StateMachine<MonsterBase>();
        stateMachine.AddState((int)eMonsterStates.Idle, new MonsterState.Idle());
        stateMachine.AddState((int)eMonsterStates.Patrol, new MonsterState.Patrol());
        stateMachine.AddState((int)eMonsterStates.Trace, new MonsterState.Trace());
        stateMachine.AddState((int)eMonsterStates.Attack, new MonsterState.Attack());
        stateMachine.AddState((int)eMonsterStates.Die, new MonsterState.Die());
        #endregion
    }
    protected virtual void Update()
    {
        stateMachine.Execute();
    }
    public virtual void Init(MapDifficulty _mapDifficulty)
    {
        wayPoints.SetWayPoints();
        stateMachine.InitState(this, stateMachine.GetState((int)eMonsterStates.Idle));
        IsDead = false;
    }
    public void ChangeState(eMonsterStates _newState)
    {
        stateMachine.ChangeState(stateMachine.GetState((int)_newState));
    }
    public void GetDamage(float _damage)
    {
        CurHP -= _damage;
        if (CurHP <= 0)
        {
            CurHP = 0;
            stateMachine.ChangeState(stateMachine.GetState((int)eMonsterStates.Die));
        }
        Debug.Log(CurHP);
    }
    public void GetDotDamage()
    {
        if (!isCurrupted)
            StartCoroutine(SetDotDamage());
    }
    bool isCurrupted;
    IEnumerator SetDotDamage()
    {
        isCurrupted = true;
        for (int i = 0; i < 5; i++)
        {
            CurHP -= MaxHP * 0.01f;
            yield return new WaitForSeconds(1.0f);
        }
        isCurrupted = false;
    }
    public void OnAttackCollision()
    {
        hitBox.SetActive(true);
    }
    public void OffAttackCollision()
    {
        hitBox.SetActive(false);
    }
    public void MoveAndRotate(Vector3 _targetPos)
    {
        Vector3 dir = _targetPos - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, stat.MoveSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
    }
    public bool CheckRadius(Vector3 _targetPoint, float _radius)  //타겟 위치, 비교 거리
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
        if (CheckRadius(target.transform.position, traceDistance))
        {
            curTarget = target;
        }
        else curTarget = null;
        //Debug.Log("Current Target : " + curTarget);
    }

}