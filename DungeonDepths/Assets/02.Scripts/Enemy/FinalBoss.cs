using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinalBoss : MonoBehaviour
{
    public enum FinalBossStates { Idle, AttackIdle, Trace, MeleeAttack1, MeleeAttack2, MeleeAttack3, RangeAttack, Die };
    public StateMachine<FinalBoss> stateMachine;
    public Animator animator;
    public float comboDuration = 2.5f;
    [SerializeField] float moveSpeed = 9f;
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] Transform finalBossTransform;
    [SerializeField] Transform targetTransform;
    public bool isDead;
    public Rigidbody rbody;
    public CapsuleCollider collider;

    public bool isSecondPhase;
    public float prevRangeAtkTime;
    float bossRangeAtkDelay = 15f;

    float meleeAttackRange = 3f;
    float meleeAttackAngle = 60f;

    public float hpMax = 100, hpCur;
    readonly int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    void Awake()
    {
        hpCur = hpMax;
        finalBossTransform = GetComponent<Transform>();
        targetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        stateMachine = new StateMachine<FinalBoss>();
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        
        isSecondPhase = false;

        stateMachine.AddState((int)FinalBossStates.Idle, new FinalBossState.Idle());
        stateMachine.AddState((int)FinalBossStates.AttackIdle, new FinalBossState.AttackIdle());
        stateMachine.AddState((int)FinalBossStates.Trace, new FinalBossState.Trace());
        stateMachine.AddState((int)FinalBossStates.MeleeAttack1, new FinalBossState.MeleeAttack1());
        stateMachine.AddState((int)FinalBossStates.MeleeAttack2, new FinalBossState.MeleeAttack2());
        stateMachine.AddState((int)FinalBossStates.MeleeAttack3, new FinalBossState.MeleeAttack3());
        stateMachine.AddState((int)FinalBossStates.RangeAttack, new FinalBossState.RangeAttack());
        stateMachine.AddState((int)FinalBossStates.Die, new FinalBossState.Die());

        stateMachine.InitState(this, stateMachine.GetState((int)FinalBossStates.Idle));
    }

	private void OnEnable()
	{
        animator.SetTrigger("BossEnter");
        stateMachine.InitState(this, stateMachine.GetState((int)FinalBossStates.Idle));
    }
    private void Update()
    {
        //Debug.Log("최종보스 현재 상태 : " + stateMachine.CurrentState);
        stateMachine.Execute();
    }

    public void GetHit(float _damage)
    {
        hpCur -= _damage;
        Debug.Log("최종 보스 현재 체력 : " + hpCur);
        if(hpCur <= hpMax * 0.6 && !isSecondPhase)
        {
            animator.SetTrigger("Stun");
            isSecondPhase = true;
        }
        else if(hpCur <= 0)
        {
            hpCur = 0f;
            isDead = true;
            stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.Die));
        }
    }

    //보스와 플레이어 사이의 거리를 구해서 반환
    public float CheckDistance()
    {
        //Debug.Log("플레이어와의 거리 구하기");
        return (finalBossTransform.position - targetTransform.position).magnitude;
    }

    // 보스의 상태를 Trace 상태로 변환한다.
    public void CheckTraceState()
    {
        //Debug.Log("추적 확인");
        stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.Trace));
    }
    // 보스의 좌표를 플레이어 좌표쪽으로 이동시킨다.
    public void Trace()
    {
        //Debug.Log("추적 시작");
        finalBossTransform.position = Vector3.MoveTowards(finalBossTransform.position, targetTransform.position, moveSpeed * Time.deltaTime);
    }

    // 보스가 플레이어쪽을 바라보게끔 회전시킨다.
    public void Rotation()
    {
        //Debug.Log("회전");
        Vector3 dir = targetTransform.position - finalBossTransform.position;
        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir); // y축 회전값만 있는 쿼터니온을 생성합니다.
            Quaternion finalRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // y축 회전값만 유지하도록 합니다.
            finalBossTransform.rotation = Quaternion.Lerp(finalBossTransform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 플레이어와의 거리를 확인한후 이를 공격반경 검사 함수에 전달한다.
    public void CheckAttackState()
    {
        Debug.Log("공격상태 확인");
        //if(Time.time - prevAttackTime < bossAttackDelay) return;
        float dist = CheckDistance();
        int attackIndex;
        //Debug.Log("플레이어와의 거리 : " + dist);
        if(IsPlayerInAttackSight(dist, out attackIndex))
        {
            //prevAttackTime = Time.time;
            if(attackIndex == 1)
                stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.MeleeAttack1));
            else if(attackIndex == 2)
                stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.MeleeAttack2));
            else if(attackIndex == 3)
                stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.MeleeAttack3));
            else if(attackIndex == 4)
                stateMachine.ChangeState(stateMachine.GetState((int)FinalBossStates.RangeAttack));
        }
        else Rotation();
    }

    //보스의 공격반경을 검사한다.
    public bool IsPlayerInAttackSight(float _dist, out int _attackIndex)
    {
        //Debug.Log("보스 공격 시야 확인");
        //RaycastHit hit;
        //Ray ray = new Ray(finalBossTransform.position, finalBossTransform.forward);
        Vector3 dir = (targetTransform.position - finalBossTransform.position).normalized;
        // 근접공격 사거리 내에 들어와있다면
        if(_dist <= meleeAttackRange)
        {
            float dot = Vector3.Dot(finalBossTransform.forward, dir);
            float theta = Mathf.Acos(dot);
            float angle = Mathf.Rad2Deg * theta;

            //if(Physics.Raycast(ray, out hit, meleeAttackRange) && hit.collider.CompareTag("Player"))
            if(angle <= meleeAttackAngle / 5)
            {
                _attackIndex = 1;
                return true;
            }
            else if(angle <= meleeAttackAngle && isSecondPhase)
            {
                _attackIndex = 3;
                return true;
            }
            else if(angle <= meleeAttackAngle * 2)
            {
                _attackIndex = 2;
                return true;
            }
            else Rotation();
        }
        else if(_dist < 10f)
        {
            _attackIndex = 0;
            return false;
        }
        else if(_dist <= meleeAttackRange * 5 && isSecondPhase)
        {
            Debug.Log("원거리 공격 확인");
            if(Time.time - prevRangeAtkTime >= bossRangeAtkDelay)
            {
                _attackIndex = 4;
                return true;
            }
        }
        _attackIndex = 0;
        return false;
    }

    public bool ShouldCombo(out int comboIndex)
    {
        Debug.Log("콤보어택 검사");
        float dist = CheckDistance();
        if(IsPlayerInAttackSight(dist, out comboIndex))
        {
            if(comboIndex == 0) return false;
            return true;
        }
        return false;
    }
}
