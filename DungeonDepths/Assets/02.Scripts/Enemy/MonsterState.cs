using UnityEngine;

namespace MonsterState
{
    class Idle : State<MonsterBase>
    {
        public override void Enter(MonsterBase m)
        {
            m.PrevTime = Time.time;
        }
        public override void Execute(MonsterBase m)
        {
            m.IdleTime = Time.time - m.PrevTime;
            m.SearchTarget();
            m.CheckIdleState();
        }
        public override void Exit(MonsterBase m) { }
    }
    class Patrol : State<MonsterBase>
    {
        public override void Enter(MonsterBase m)
        {
            m.Anim.SetBool("IsWalk", true);
            m.wayPoints.MoveNextPoint();
        }
        public override void Execute(MonsterBase m)
        {
            m.MoveAndRotate(m.wayPoints.destinationPoint);
            m.CheckPatrolState();
        }
        public override void Exit(MonsterBase m) 
        {
            m.Anim.SetBool("IsWalk", false);
        }
    }
    class Trace : State<MonsterBase>
    {
        public override void Enter(MonsterBase m) 
        {
            m.Anim.SetBool("IsRun", true);
        }
        public override void Execute(MonsterBase m)
        {
            m.CheckTraceState();
        }
        public override void Exit(MonsterBase m) 
        {
            m.Anim.SetBool("IsRun", false);
        }
    }
    class Attack : State<MonsterBase>
    {
        //Attack범위를 벗어나면 : trace상태로 change
        public override void Enter(MonsterBase m) 
        {
            m.Anim.SetBool("IsAttack", true);
        }
        public override void Execute(MonsterBase m)
        {
            m.CheckAttackState();
            // 공격하는 거
        }
        public override void Exit(MonsterBase m) 
        { 
            m.Anim.SetBool("IsAttack", false);
        }
    }
    class Die : State<MonsterBase>
    {
        float delay;
        public override void Enter(MonsterBase m) 
        {
            m.Anim.SetTrigger("DieTrigger");
            m.IsDead = true;
            m.Col.enabled = false;
            Debug.Log("죽음");
            delay = Time.time;
        }
        public override void Execute(MonsterBase m) 
        {
            if(Time.time - delay >= 3f)
            {
                m.gameObject.SetActive(false);
            }
        }
        public override void Exit(MonsterBase m) 
        {
            m.Col.enabled = true;
        }
    }
}