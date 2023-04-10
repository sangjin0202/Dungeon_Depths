using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordManState
{
    #region 공격을 하지않는 상태
    class None : State<PlayerSwordMan>
    {
        public override void Enter(PlayerSwordMan p)
        {
            p.isAttack = false;
            p.numOfClicks = 0;
            p.stateDuration = 0;
            p.prevAtkTime = 0;
            //p.shouldCombo = false;
            p.attackIndex = 0;
        }
        public override void Execute(PlayerSwordMan p)
        {

        }
        public override void Exit(PlayerSwordMan p)
        {

        }
    }
    #endregion

    #region 1타
    class Start : State<PlayerSwordMan>
    {
        public override void Enter(PlayerSwordMan p)
        {
            p.isAttack = true;
            p.attackIndex = 1;
            p.stateDuration = 1f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
            //Debug.Log("플레이어 공격" + p.attackIndex + "실행!");
        }
        public override void Execute(PlayerSwordMan p)
        {
            if(Time.time - p.prevAtkTime >= p.stateDuration)
            {
                p.numOfClicks = 0;
                p.isAttack = false;
                //Debug.Log(p.numOfClicks);
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
            else if(p.numOfClicks >= 2)
            {
                //Debug.Log(p.numOfClicks);
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.Combo));
            }
        }
        public override void Exit(PlayerSwordMan p)
        {
            
        }
    }
    #endregion

    #region 2타
    class Combo : State<PlayerSwordMan>
    {
        public override void Enter(PlayerSwordMan p)
        {
            p.isAttack = true;
            p.attackIndex = 2;
            p.stateDuration = 1.5f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
            //Debug.Log("플레이어 공격" + p.attackIndex + "실행!");
        }
        public override void Execute(PlayerSwordMan p)
        {
            if(Time.time - p.prevAtkTime >= p.stateDuration)
            {
                p.numOfClicks = 0;
                //Debug.Log(p.numOfClicks);
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
            else if(p.numOfClicks == 3)
            {
                //Debug.Log(p.numOfClicks);
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.Finish));
            }
        }
        public override void Exit(PlayerSwordMan p)
        {
            
        }
    }
    #endregion

    #region 3타
    class Finish : State<PlayerSwordMan>
    {
        public override void Enter(PlayerSwordMan p)
        {
            p.isAttack = true;
            p.attackIndex = 3;
            p.stateDuration = 3.5f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
            //Debug.Log("플레이어 공격" + p.attackIndex + "실행!");
        }
        public override void Execute(PlayerSwordMan p)
        {
            if(Time.time - p.prevAtkTime >= p.stateDuration)
            {
                p.numOfClicks = 0;
                //Debug.Log(p.numOfClicks);
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
        }
        public override void Exit(PlayerSwordMan p)
        {
           
        }
    }
    #endregion
}
