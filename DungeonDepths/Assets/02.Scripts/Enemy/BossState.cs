using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossState
{
    #region 상태 : 대기
    class Idle : State<BossBaseFSM>
    {
        float idleEnterTime;
        public override void Enter(BossBaseFSM b)
        {
            b.Animator.SetFloat("MoveSpeed", 0);
            idleEnterTime = Time.time;
        }
        public override void Execute(BossBaseFSM b)
        {
            if(Time.time - idleEnterTime >= b.delayTime)
                b.CheckTraceState();
        }
        public override void Exit(BossBaseFSM b)
        {

        }
    }
    #endregion
    #region 상태 : 일반속도 추적
    class Trace : State<BossBaseFSM>
    {
        public override void Enter(BossBaseFSM b)
        {
            b.Animator.SetFloat("MoveSpeed", b.MoveSpeed);
        }
        public override void Execute(BossBaseFSM b)
        {
            b.Rotate();
            b.CheckTraceState();
            b.Trace();
            b.CheckAttackState();
        }
        public override void Exit(BossBaseFSM b)
        {
        }
    }
    #endregion
    #region 상태 : 빠른속도 추적
    class FastTrace : State<BossBaseFSM>
    {
        float moveSpeed;
        public override void Enter(BossBaseFSM b)
        {
            moveSpeed = b.MoveSpeed * 2;
            b.Animator.SetFloat("MoveSpeed", moveSpeed);
        }
        public override void Execute(BossBaseFSM b)
        {
            b.Rotate();
            b.CheckTraceState();
            b.Trace();
            b.CheckAttackState();
        }
        public override void Exit(BossBaseFSM b)
        {
        }
    }
    #endregion
    #region 상태 : 근접공격
    class MeleeAttack : State<BossBaseFSM>
    {
        float setTime;
        int attackIndex;
        public override void Enter(BossBaseFSM b)
        {
            setTime = Time.time;

            attackIndex = Random.Range(0, 2);
            b.Animator.SetInteger("MeleeAttackIndex", attackIndex);
            b.Animator.SetTrigger("MeleeAttack" + attackIndex);
            b.PrevAtkTime = Time.time;
            //Debug.Log("근접 공격 기록 : " + b.PrevAtkTime);
        }
        public override void Execute(BossBaseFSM b)
        {
            //Debug.Log("MeleeAttack Execute");
            //if(!b.GetDelayTime(setTime)) return;
            b.stateMachine.ChangeState(b.stateMachine.GetState((int)BossBaseFSM.BossStates.Idle));
        }
        public override void Exit(BossBaseFSM b) 
        {
        }
    }
    #endregion
    #region 상태 : 원거리 공격
    class RangeAttack : State<BossBaseFSM>
    {
        float setTime = 0;
        int attackIndex;
        public override void Enter(BossBaseFSM b)
        {
            setTime = Time.time;
            attackIndex = Random.Range(0, 2);
            b.Animator.SetInteger("RangeAttackIndex", attackIndex); 
            b.Animator.SetTrigger("RangeAttack" + attackIndex);
            b.PrevAtkTime = Time.time;
            //Debug.Log("원거리 공격 기록 : " + b.PrevAtkTime);
        }
        public override void Execute(BossBaseFSM b)
        {
            //if(!b.GetDelayTime(setTime)) return;
            b.stateMachine.ChangeState(b.stateMachine.GetState((int)BossBaseFSM.BossStates.Idle));
        }
        public override void Exit(BossBaseFSM b) 
        {
        }
    }
    #endregion
    #region 상태 : 사망
    class Die : State<BossBaseFSM>
    {
        public override void Enter(BossBaseFSM b)
        {
            b.isDead = true;
            b.Animator.SetTrigger("Die");
        }
        public override void Execute(BossBaseFSM b)
        {

        }
        public override void Exit(BossBaseFSM b)
        {

        }
    }
    #endregion
}
