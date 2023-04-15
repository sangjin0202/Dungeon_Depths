using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalBossState
{
    #region 상태 : 대기
    class Idle : State<FinalBoss>
    {
        float stateDuration;
        float stateEnterTime;

        public override void Enter(FinalBoss f)
        {
            Debug.Log("대기 상태 시작");
            if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.AttackIdle))
                stateDuration = 1f;
            else
                stateDuration = 3f;
            stateEnterTime = Time.time;
            f.animator.SetFloat("MoveSpeed", 0);
            for(int i = 1; i <= 3; i++)
                f.animator.SetBool("Combo" + i, false);


        }
        public override void Execute(FinalBoss f)
        {
            Debug.Log("대기 상태 중");
            if(Time.time - stateEnterTime < stateDuration) return;
            //f.Rotation();
            //f.Trace();
            f.CheckTraceState();
        }
        public override void Exit(FinalBoss f)
        {
            //Debug.Log("대기 상태 종료");
        }
    }
    #endregion

    #region 후속타 결정 상태
    class AttackIdle : State<FinalBoss>
    {
        int comboIndex;
        float firstAtkTime;
        float decisionTime;
        public override void Enter(FinalBoss f)
        {
            firstAtkTime = Time.time;
            if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack1))
                decisionTime = 0.8f;
            //else if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack2))
            else
                decisionTime = 0.5f;
        }
        public override void Execute(FinalBoss f)
        {
            // 0.7초 + @ 동안 딜레이
            if(Time.time - firstAtkTime < decisionTime) return;

            //f.Rotation();

            // 콤보 공격을 할수 있는 시간 내에
            // 여전히 플레이어가 사거리 안에 있고
            // 이를 보스가 포착했다면
            if(Time.time - firstAtkTime <= f.comboDuration && f.ShouldCombo(out comboIndex))
            {
                //if(f.ShouldCombo(out comboIndex))
                //{
                //if(f.precedingAttacks[comboIndex - 1])
                //{

                //Debug.Log("콤보 공격 : " + comboIndex);
                //f.animator.SetTrigger("Combo" + comboIndex);
                if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack1))
                    f.animator.SetBool("Combo1", true);
                else if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack2))
                    f.animator.SetBool("Combo2", true);
                //else if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack3))
                else if(f.stateMachine.PreviousState == f.stateMachine.GetState((int)FinalBoss.FinalBossStates.MeleeAttack3))
                    f.animator.SetBool("Combo3", true);
                else
                    f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.Idle));
                //f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.Idle));
                //f.precedingAttacks[comboIndex - 1] = false;
                //}
                //}
            }
            else
            {
                Debug.Log("콤보 유지 실패");
                f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.Idle));
            }
        }
        public override void Exit(FinalBoss f)
        {

        }
    }
    #endregion
    class Trace : State<FinalBoss>
    {
        public override void Enter(FinalBoss f)
        {
            Debug.Log("추격상태 시작");
            f.animator.SetFloat("MoveSpeed", 6.5f);
        }
        public override void Execute(FinalBoss f)
        {
            //Debug.Log("추격상태 도중");
            f.Rotation();
            f.CheckAttackState();
            f.Trace();
        }
        public override void Exit(FinalBoss f)
        {
            //Debug.Log("추격상태 종료");
        }
    }
    /// <summary>
    /// 보스의 공격은 모두 연속공격 형식
    /// 플레이어가 사거리내에 있다고 판단되면 공격을 실행 후
    /// 다시 플레이어와의 거리를 판단하고 여전히 플레이어가 사거리
    /// 내에 있다면 후속공격을 실행한다.
    /// 공격상태 -> 거리 체크 -> 후속공격 or 추적상태 변환
    /// </summary>
    class MeleeAttack1 : State<FinalBoss>
    {
        //int comboIndex;
        //float firstAtkTime;
        public override void Enter(FinalBoss f)
        {
            //Debug.Log("공격1 시작");
            // 공격 모션 실행
            f.animator.SetTrigger("Attack1Trigger");
            // 첫타 공격모션이 실행된 시간을 기록한다.
            // firstAtkTime = Time.time;
            //f.precedingAttacks[0] = true;
        }
        public override void Execute(FinalBoss f)
        {
            //Debug.Log("공격1 실행중");
            f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.AttackIdle));
            //if(Time.time - firstAtkTime <= f.comboDuration)
            //{
            //    if(f.ShouldCombo(out comboIndex))
            //        Debug.Log("콤보 공격 : " + comboIndex);
            //        f.animator.SetTrigger("Combo" + comboIndex);
            //}
            //f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.Idle));
        }

        public override void Exit(FinalBoss f)
        {
            //Debug.Log("공격1 종료");
        }
    }
    class MeleeAttack2 : State<FinalBoss>
    {
        //int comboIndex;
        public override void Enter(FinalBoss f)
        {
            //Debug.Log("공격2 시작");
            f.animator.SetTrigger("Attack2Trigger");
            //f.precedingAttacks[1] = true;
        }
        public override void Execute(FinalBoss f)
        {
            //if(f.ShouldCombo(out comboIndex))
            //    f.animator.SetTrigger("Combo" + comboIndex);
            //else f.animator.SetBool("isAttack2", false);
            f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.AttackIdle));
        }
        public override void Exit(FinalBoss f)
        {
            //Debug.Log("공격2 종료");
        }
    }
    class MeleeAttack3 : State<FinalBoss>
    {
        public override void Enter(FinalBoss f)
        {
            f.animator.SetTrigger("Attack3Trigger");
        }
        public override void Execute(FinalBoss f)
        {
            f.stateMachine.ChangeState(f.stateMachine.GetState((int)FinalBoss.FinalBossStates.AttackIdle));
        }
        public override void Exit(FinalBoss f)
        {

        }
    }


    class Die : State<FinalBoss>
    {
        public override void Enter(FinalBoss f)
        {

        }
        public override void Execute(FinalBoss f)
        {
        }
        public override void Exit(FinalBoss f)
        {

        }
    }
}
