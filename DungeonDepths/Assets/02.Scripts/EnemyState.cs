using UnityEngine;

namespace EmemyState 
{
  class Idle : State<EnemyBase>
  {
    public override void Enter(EnemyBase e) 
    {
      e.idleTime = Time.time - e.prevTime;
    }
    public override void Execute(EnemyBase e) 
    {
      e.ExitIdle();
    }
    public override void Exit(EnemyBase e) {}
  }

  class Patroll : State<EnemyBase> 
  {
    public override void Enter(EnemyBase e) 
    {
    }
    public override void Execute(EnemyBase e) 
    {
    }
    public override void Exit(EnemyBase e) 
    {
    }
  }

  class Trace : State<EnemyBase> 
  {
    public override void Enter(EnemyBase e) 
    {
    }
    public override void Execute(EnemyBase e) 
    {
    }
    public override void Exit(EnemyBase e) 
    {
    }
  }
  class Attack : State<EnemyBase> 
  {
    public override void Enter(EnemyBase e) 
    {
    }
    public override void Execute(EnemyBase e) 
    {
    }
    public override void Exit(EnemyBase e) 
    {
    }
  }
  class Die : State<EnemyBase> 
  {
    public override void Enter(EnemyBase e) 
    {
    }
    public override void Execute(EnemyBase e) 
    {
    }
    public override void Exit(EnemyBase e) 
    {
    }
  }
}