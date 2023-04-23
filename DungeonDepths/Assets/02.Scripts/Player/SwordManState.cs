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
            p.IsAttack = false;
            p.prevAtkTime = 0;
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
            p.IsAttack = true;
            p.attackClick = false;
            p.attackIndex = 1;
            p.stateDuration = 0.7f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
        }
        public override void Execute(PlayerSwordMan p)
        {
            if(Time.time - p.prevAtkTime > 0.7)
            {
                Debug.Log("콤보 유지 실패!");
                //p.numOfClicks = 0;
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
            else if(p.attackClick)
            {
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
            p.IsAttack = true;
            p.attackClick = false;
            p.attackIndex = 2;
            p.stateDuration = 1f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
        }
        public override void Execute(PlayerSwordMan p)
        {
            if(Time.time - p.prevAtkTime > p.stateDuration)
            {
                //p.numOfClicks = 0;
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
            else if(p.attackClick)
            {
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
            p.IsAttack = true;
            p.attackClick = false;
            p.attackIndex = 3;
            p.stateDuration = 1.5f;
            p.animator.SetTrigger("Attack" + p.attackIndex);
            p.prevAtkTime = Time.time;
        }
        public override void Execute(PlayerSwordMan p)
        {
            p.animator.SetFloat("MoveSpeed", 1f);
            if(Time.time - p.prevAtkTime >= p.stateDuration)
            {
                //p.numOfClicks = 0;
                p.stateMachine.ChangeState(p.stateMachine.GetState((int)PlayerSwordMan.SwordManStates.None));
            }
        }
        public override void Exit(PlayerSwordMan p)
        {

        }
    }
    #endregion
}
