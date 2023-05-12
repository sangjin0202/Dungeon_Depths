using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BossBaseFSM : MonoBehaviour
{
    public enum BossStates { Idle, Trace, FastTrace, MeleeAttack, RangeAttack, Die };
    public StateMachine<BossBaseFSM> stateMachine;
    public Animator Animator { get; private set; }
    public Rigidbody Rbody { get; set; }
    #region 보스 스테이터스
    public float BossMaxHp { get; set; }
    public float BossCurHp { get; set; }
    public float MoveSpeed { get; set; }
    public float RotSpeed { get; set; }
    public float AttackDamage { get; set; }
    #endregion
    #region 추적 관련 변수
    public bool isSomethingAhead;
    public Transform TargetTransform { get; set; }
    public Transform BossTransform { get; set; }
    public float TraceRange { get; set; } // 추적 거리는 맵 사이즈 만큼 설정할 것
    #endregion  
    #region 공격 관련 변수
    public float MeleeAngle { get; set; } // 근접공격을 실행할 앵글
    public float RangeAngle { get; set; } // 원거리 공격을 실행할 앵글
    public float AttackDelay { get; set; }
    public float PrevAtkTime { get; set; }
    public float MeleeRange { get; set; }
    public float BeamRange { get; set; }
    #endregion
    public bool isDead;
    public float delayTime = 2.5f;
    public Vector3 targetPos; 
    protected void FixedUpdate()
    {
        LayerMask layer = 1 << 8;
        isSomethingAhead = Physics.Raycast(BossTransform.position, BossTransform.forward, 3f, layer);
    }
    protected virtual void Awake() // 상태추가 , 초기화
    {
        stateMachine = new StateMachine<BossBaseFSM>();
        Animator = GetComponent<Animator>();

        isDead = false;
        stateMachine.AddState((int)BossStates.Idle, new BossState.Idle());
        stateMachine.AddState((int)BossStates.Trace, new BossState.Trace());
        stateMachine.AddState((int)BossStates.FastTrace, new BossState.FastTrace());
        stateMachine.AddState((int)BossStates.MeleeAttack, new BossState.MeleeAttack());
        stateMachine.AddState((int)BossStates.RangeAttack, new BossState.RangeAttack());
        stateMachine.AddState((int)BossStates.Die, new BossState.Die());

        stateMachine.InitState(this, stateMachine.GetState((int)BossStates.Idle));
    }

    public abstract void GetHit(float _damage);

    // 보스 사망여부 확인
    protected void CheckAlive()
    {
        if(BossCurHp <= 0)
        {
            isDead = true;
            stateMachine.ChangeState(stateMachine.GetState((int)BossStates.Die));
        }
    }

    /// <summary>
    /// CheckDistance 메소드의 반환값이 true라면 빠른 추적
    /// false라면 일반 추적
    /// </summary>
    public void CheckTraceState()
    {
        if(CheckDistance(TraceRange / 2))
            stateMachine.ChangeState(stateMachine.GetState((int)BossStates.FastTrace));
        else
            stateMachine.ChangeState(stateMachine.GetState((int)BossStates.Trace));
    }

    /// <summary>
    /// 보스와 플레이어 사이의 거리를 측정해서 CheckPlayerInSight함수로 전달
    /// </summary>
    public void CheckAttackState()
    {
        float dist = (TargetTransform.position - BossTransform.position).magnitude;
        CheckPlayerInSight(dist);
    }

    public void Trace()
    {
        if(!isSomethingAhead)
            BossTransform.position = Vector3.MoveTowards(BossTransform.position, TargetTransform.position, MoveSpeed * Time.deltaTime);
    }
    public void Rotate()
    {
        //Vector3 dir = TargetTransform.position - BossTransform.position;
        //Quaternion rot = Quaternion.LookRotation(dir);
        //BossTransform.rotation = Quaternion.Lerp(BossTransform.rotation, rot, RotSpeed * Time.deltaTime);
        Vector3 dir = TargetTransform.position - BossTransform.position;
        dir.y = 0; // y축 방향을 제외한 나머지 축 방향을 0으로 설정한다.
        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir); // y축 회전값만 있는 쿼터니온을 생성합니다.
            Quaternion finalRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // y축 회전값만 유지하도록 합니다.
            BossTransform.rotation = Quaternion.Lerp(BossTransform.rotation, finalRotation, RotSpeed * Time.deltaTime);
        }
    }

    // 플레이어가 공격 사거리내에 들어왔을 경우
    // 플레이어가 시야에 포착됐는지 확인
    // 아니라면 보스는 회전한다.
    public void CheckPlayerInSight(float _dist)
    {
        RaycastHit hit;
        Vector3 dir = TargetTransform.position - BossTransform.position;
        if(_dist <= MeleeRange) // 플레이어가 근접 사거리 내에 있다면
        {
            if(Physics.Raycast(BossTransform.position, BossTransform.forward, out hit, MeleeRange) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("BlockArea")))
            {
                stateMachine.ChangeState(stateMachine.GetState((int)BossStates.MeleeAttack));
            }
            else
            {
                Rotate();
            }
        }
        else if(6 < _dist && _dist < 13)
        {
            stateMachine.ChangeState(stateMachine.GetState((int)BossStates.Trace));
        }
        else if(_dist <= BeamRange)
        {
            int random = Random.Range(0, 100);
            if(Physics.Raycast(BossTransform.position, BossTransform.forward, out hit, BeamRange) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("BlockArea")))
            {
                targetPos = TargetTransform.position;
                if(random <= 4 && Time.time - PrevAtkTime >= AttackDelay)
                {
                    stateMachine.ChangeState(stateMachine.GetState((int)BossStates.RangeAttack));
                }
            }
            else
            {
                Rotate();
            }
        }

    }
    //true를 반환한다면 빠른 추적
    //false를 반환한다면 일반 추적 
    public bool CheckDistance(float _slowDistance)
    {
        // 현재 보스와 플레이어 사이의 거리, 방향
        Vector3 dir = TargetTransform.position - BossTransform.position;
        float sqrSlowDistance = _slowDistance * _slowDistance;
        // 현재 보스와 플레이어 사이의 거리가 충분히 멀다면 
        if(dir.sqrMagnitude >= sqrSlowDistance)
            return true;
        return false;
    }

}
