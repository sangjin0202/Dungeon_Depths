using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossBase : MonoBehaviour
{
    // 필요한 데이터: 보스 체력, 시야각, 이동속도, 모션별 공격력, 공격 텀, 애니메이터, 플레이어 위치 탐색, 어떤 공격이 나갈지 확률 설정, 공격 사거리
    /* 계획
     * 보스는 보스룸에 플레이어와 단 둘이 존재하므로 보스전에 돌입하면 언제나 Trace 상태
     * 보스는 근접공격과 원거리공격 두개의 반경을 가진다.
     * 원거리 공격은 거리가 긴 만큼 각도를 좁게, 근거리 공격은 거리가 짧은대신 조금 넓게 
     * 플레이어와의 거리가 일정량 이상 벌어지면 빠르게 추격하도록 할 것 
     */

    public enum BossStates { Idle, Trace, FastTrace, MeleeAttack, RangeAttack, Die };
    public BossStates state;

    public Animator Animator { get; set; }
    public NavMeshAgent Agent { get; set; }

    #region 보스 스테이터스
    public float BossHp { get; set; }
    public float MoveSpeed { get; set; }
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

    [SerializeField] bool isDead = false;
    protected void CheckAlive() // 보스 사망 여부 확인
    {
        if(BossHp <= 0)
        {
            isDead = true;
            state = BossStates.Die;
        }
    }
    protected void CheckState(float _dist)
    {
        Debug.Log("BossState : " + state);
        if(isDead) return;
        
        if(_dist <= TraceRange * 0.5f) // 추적
        {
            if(_dist <= MeleeRange)
            {
                CheckPlayerInSight(_dist);
            }
            else if(_dist <= BeamRange)
            {
                int rand = Random.Range(0, 10);
                if(rand <= 1) // 20프로 확률로 원거리 공격을 준비
                    CheckPlayerInSight(_dist);
            }
            else
                state = BossStates.Trace;
        }
        else // 빠른 추적
        {
            state = BossStates.FastTrace;
        }
    }

    protected void CheckPlayerInSight(float _dist)
    {
        RaycastHit hit;
        Ray ray = new Ray(BossTransform.position, BossTransform.forward);
        Vector3 dir = (TargetTransform.position - BossTransform.position).normalized;

        if(_dist <= MeleeRange) // 근접 공격사거리까지 다가왔다면
        {
            if(Vector3.Angle(BossTransform.forward, dir) < MeleeAngle && Physics.Raycast(ray, out hit, MeleeRange))
            {
                if(hit.collider.CompareTag("Player"))
                    state = BossStates.MeleeAttack;
            }

        }
        else
        {
            if(Vector3.Angle(BossTransform.forward, dir) < RangeAngle && Physics.Raycast(ray, out hit, BeamRange))
            {
                if(hit.collider.CompareTag("Player"))
                    state = BossStates.RangeAttack;
            }
        }
    }

    protected void BossAction()
    {
        switch(state)
        {
            case BossStates.Idle:
                // 대기(보스룸 입장시 짧은 시간동안 실행)
                break;
            case BossStates.Trace:
                // 추적
                Agent.isStopped = false;
                TraceTarget(MoveSpeed);
                break;
            case BossStates.FastTrace:
                // 빠른추적
                Agent.isStopped = false;
                TraceTarget(MoveSpeed * 2f);
                break;
            case BossStates.MeleeAttack:
                Agent.isStopped = true;
                //BossTransform.Translate(Vector3.zero);
                if(Time.time - PrevAtkTime < AttackDelay) break;
                MeleeAttack();
                break;
            case BossStates.RangeAttack:
                Agent.isStopped = true;
                //BossTransform.Translate(Vector3.zero);
                RangeAttack();
                //PrevAtkTime = Time.time;
                break;
            case BossStates.Die:
                Agent.isStopped = true;
                Die();
                break;
        }
    }

    //public Vector3 MeleeCirclePoint(float angle)
    //{
    //    angle += transform.eulerAngles.y;
    //    return new Vector3(Mathf.Sign(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    //}
    //public Vector3 RangeCirclePoint(float angle)
    //{
    //    angle += transform.eulerAngles.y;
    //    return new Vector3(Mathf.Sign(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    //}
    //_dist는 플레이어와 보스 사이의 거리

    protected void TraceTarget(float _speed)
    {
        Agent.destination = TargetTransform.position;
        Animator.SetFloat("MoveSpeed", _speed);
        if(_speed > MoveSpeed)
        {
            Animator.SetBool("FastTrace", true);
            Agent.speed = _speed;
        }
        else
        {
            Animator.SetBool("Trace", true);
            Agent.speed = _speed;
        }
        //Search();
        //Rotate();
        //Move();
    }

    void Search()
    {

    }

    void Rotate()
    {

    }

    void Move()
    {

    }

    protected void MeleeAttack()
    {
        Debug.Log("보스 근접 공격");
        Animator.SetInteger("MeleeAttackIndex", Random.Range(0, 2));
        Animator.SetTrigger("MeleeAttack");
        PrevAtkTime = Time.time;
    }

    protected void RangeAttack()
    {
        Animator.SetTrigger("RangeAttack");
    }

    protected void GetDamage()
    {
        Debug.Log("보스 피격");
        Animator.SetTrigger("GeDamage");
    }
    protected void Die()
    {
        Debug.Log("보스 사망");
        Animator.SetTrigger("Die");
    }
}
