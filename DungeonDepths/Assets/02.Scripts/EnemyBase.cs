using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour 
{
  
  #region Enemy Stats
  protected int damage;                   public int Damage { get; }
  protected float attackDistance;         public int AttackDistance { get; }
  protected float traceDistance;          public int TraceDistance { get; }
  protected float moveSpeed;              public int MoveSpeed { get; }
  protected int maxHp;                    public int MaxHp { get; }
  protected int curHp;                    public int CurHp { get { return curHp; } set { if (curHp <= 0) curHp = 0; } }
  #endregion


  #region state관련 변수
  public enum eEnemyState { Idle, Patroll, Trace, Attack, Die } // 접근 제한자 설정
  public StateMachine<EnemyBase> sm;
  public float prevTime = 0f;
  public float idleTime = 0f;
  public GameObject target;

  #endregion
  protected virtual void Awake()
  {
    #region state 설정
    sm = new StateMachine<EnemyBase>();
    sm.AddState((int)eEnemyState.Idle, new EmemyState.Idle());
    sm.AddState((int)eEnemyState.Patroll, new EmemyState.Patroll());
    sm.AddState((int)eEnemyState.Trace, new EmemyState.Trace());
    sm.AddState((int)eEnemyState.Attack, new EmemyState.Attack());
    sm.AddState((int)eEnemyState.Die, new EmemyState.Die());
    sm.InitState(this, sm.GetState((int)eEnemyState.Idle));
    #endregion
  }

  public abstract void Init();    // value 초기화 함수
  public abstract void GetHit();  // 맞기

  public void ExitIdle()
  {
    if(target != null)
      sm.ChangeState(sm.GetState((int)eEnemyState.Trace));
    else if (idleTime >= Random.Range(3f, 5f))
      sm.ChangeState(sm.GetState((int)eEnemyState.Patroll));
  }
}

