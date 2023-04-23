using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BossBaseFSM : MonoBehaviour
{
    public enum BossStates { Idle, Trace, FastTrace, MeleeAttack, RangeAttack, Die };
    public StateMachine<BossBaseFSM> stateMachine;
    public Animator Animator { get; private set; }
    protected Rigidbody rbody;
    #region 보스 스테이터스
    public float BossMaxHp { get; set; }
    public float BossCurHp { get; set; }
    public float MoveSpeed { get; set; }
    public float RotSpeed { get; set; }
    public float AttackDamage { get; set; }
    #endregion
    #region 추적 관련 변수
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
        BossTransform.position = Vector3.MoveTowards(BossTransform.position, TargetTransform.position, MoveSpeed * Time.deltaTime);
    }
    public void Rotate()
    {
        Vector3 dir = TargetTransform.position - BossTransform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        BossTransform.rotation = Quaternion.Lerp(BossTransform.rotation, rot, RotSpeed * Time.deltaTime);
    }

    //public void MoveAndRotate()
    //{
    //    Vector3 dir = TargetTransform.position - BossTransform.position;
    //    BossTransform.position = Vector3.MoveTowards(BossTransform.position, TargetTransform.position, MoveSpeed * Time.deltaTime);
    //    Quaternion rot = Quaternion.LookRotation(dir);
    //    BossTransform.rotation = Quaternion.Lerp(BossTransform.rotation, rot, RotSpeed * Time.deltaTime);
    //}

    //public bool CheckRadius(float _radius)
    //{
    //    Vector3 dir = TargetTransform.position - BossTransform.position;
    //    float radiusSqr = _radius * _radius;

    //    // dir 벡터 크기의 제곱을 반환한다.
    //    // 플레이어와 보스 사이의 거리의 제곱이 전달받은 특정 반경의 제곱보다 작다면
    //    if(dir.sqrMagnitude < radiusSqr)
    //        return true;
    //    return false;
    //}

    // 플레이어가 공격 사거리내에 들어왔을 경우
    // 플레이어가 시야에 포착됐는지 확인
    // 아니라면 보스는 회전한다.
    public void CheckPlayerInSight(float _dist)
    {
        RaycastHit hit;
        Vector3 dir = TargetTransform.position - BossTransform.position;
        if(_dist <= MeleeRange) // 플레이어가 근접 사거리 내에 있다면
        {
            if(Physics.Raycast(BossTransform.position, BossTransform.forward, out hit, MeleeRange) && hit.collider.CompareTag("Player"))
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
            if(Physics.Raycast(BossTransform.position, BossTransform.forward, out hit, BeamRange) && hit.collider.name == "PlayerSwordMan")
            {
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

    //public bool GetDelayTime(float _time)
    //{
    //    if(Time.time - _time >= 3f)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
