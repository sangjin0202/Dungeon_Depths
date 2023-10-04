using UnityEngine;

namespace MonsterState
{
    class Idle : State<MonsterBase>
    {
        public override void Enter(MonsterBase _monster)
        {
            _monster.PrevIdleTime = Time.time;
        }
        public override void Execute(MonsterBase _monster)
        {
            _monster.IdleTime = Time.time - _monster.PrevIdleTime;
            _monster.SearchTarget();
            if (_monster.CurTarget != null)     // target 잡히면 추적하도록
                _monster.ChangeState(MonsterBase.eMonsterStates.Trace);
            else if (_monster.IdleTime >= Random.Range(3f, 5f))     // 안잡히면 텀을 가지고 순찰
                _monster.ChangeState(MonsterBase.eMonsterStates.Patrol);
        }
        public override void Exit(MonsterBase _monster) { }
    }

    class Patrol : State<MonsterBase>
    {
        public override void Enter(MonsterBase _monster)
        {
            _monster.Anim.SetBool("IsWalk", true);
            _monster.WayPoints.MoveNextPoint();
        }
        public override void Execute(MonsterBase _monster)
        {
            _monster.MoveAndRotate(_monster.WayPoints.destinationPoint);
            _monster.SearchTarget();
            if (_monster.CurTarget != null && (_monster.CheckRadius(_monster.CurTarget.transform.position, _monster.TraceDistance)))
                _monster.ChangeState(MonsterBase.eMonsterStates.Trace);
            else if (_monster.WayPoints.CheckDestination(_monster.StopDistance, _monster.transform.position))
            {
                _monster.ChangeState(MonsterBase.eMonsterStates.Idle);
            }
        }
        public override void Exit(MonsterBase _monster) 
        {
            _monster.Anim.SetBool("IsWalk", false);
        }
    }

    class Trace : State<MonsterBase>
    {
        public override void Enter(MonsterBase _monster) 
        {
            _monster.Anim.SetBool("IsRun", true);
        }
        public override void Execute(MonsterBase _monster)
        {
            _monster.SearchTarget();
            if (_monster.CurTarget == null)
                _monster.ChangeState(MonsterBase.eMonsterStates.Idle);
            else if (_monster.CheckRadius(_monster.CurTarget.transform.position, _monster.AttackDistance))
            {
                _monster.ChangeState(MonsterBase.eMonsterStates.Attack);
            }
            else if (_monster.CheckRadius(_monster.CurTarget.transform.position, _monster.TraceDistance))
            {
                _monster.MoveAndRotate(_monster.CurTarget.transform.position);
            }
        }
        public override void Exit(MonsterBase _monster) 
        {
            _monster.Anim.SetBool("IsRun", false);
        }
    }

    class Attack : State<MonsterBase>
    {
        //Attack범위를 벗어나면 : trace상태로 change
        public override void Enter(MonsterBase _monster) 
        {
            _monster.Anim.SetBool("IsAttack", true);
        }
        public override void Execute(MonsterBase _monster)
        {
            if (_monster.CheckRadius(_monster.CurTarget.transform.position, _monster.AttackDistance)) // 공격 거리 체크
            {
                if (Time.time > _monster.LastAttackTime) // 공격 주기 체크
                {
                    Vector3 dir = _monster.CurTarget.transform.position - _monster.transform.position;
                    _monster.transform.rotation = Quaternion.LookRotation(dir);
                    _monster.LastAttackTime = Time.time + _monster.stat.AttackSpeed;
                }
            }
            else
                _monster.ChangeState(MonsterBase.eMonsterStates.Idle);
        }
        public override void Exit(MonsterBase _monster) 
        { 
            _monster.Anim.SetBool("IsAttack", false);
        }
    }

    class Die : State<MonsterBase>
    {
        float delay;
        public override void Enter(MonsterBase _monster) 
        {
            _monster.Anim.SetTrigger("DieTrigger");
            _monster.IsDead = true;
            _monster.MonsterRigidbody.isKinematic = true;
            _monster.MonsterCollider.enabled = false;
            delay = Time.time;
        }
        public override void Execute(MonsterBase _monster) 
        {
            if(Time.time - delay >= 3f)
            {
                _monster.gameObject.SetActive(false);
            }
        }
        public override void Exit(MonsterBase _monster) 
        {
            _monster.MonsterCollider.enabled = true;
        }
    }
}