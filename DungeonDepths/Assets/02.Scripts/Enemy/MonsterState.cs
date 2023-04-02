using UnityEngine;

namespace MonsterState
{
    class Idle : State<MonsterBase>
    {
        public override void Enter(MonsterBase m)
        {
            m.PrevTime = Time.time;
            m.SearchTarget();
        }
        public override void Execute(MonsterBase m)
        {
            m.IdleTime = Time.time - m.PrevTime;
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
        public override void Exit(MonsterBase m) { }
    }
    class Die : State<MonsterBase>
    {
        public override void Enter(MonsterBase m) 
        {
            m.Anim.SetBool("IsDie",true);
        }
        public override void Execute(MonsterBase m) { }
        public override void Exit(MonsterBase m) { }
    }
}