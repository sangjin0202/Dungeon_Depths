using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
	#region Enemy Stats
	protected int damage;				public int Damage { get; }
	protected float attackDistance;		public int AttackDistance { get; }
	protected float traceDistance;		public int TraceDistance { get; }
	protected float traceDisOffset = 2f;
	protected float stopDistance = 0.2f;
	protected float moveSpeed;			public int MoveSpeed { get; }
	protected float rotSpeed;			public int RotSpeed { get; }
	protected float attackSpeed = 2f;	public int AttackSpeed { get; }    // 공격 쿨
	protected float searchSpeed = 1f;	public int SearchSpeed { get; }	// 탐색 쿨
	protected int maxHp;				public int MaxHp { get; }
	protected int curHp;				public int CurHp { get { return curHp; } set { if (curHp <= 0) curHp = 0; } }
	#endregion
	#region state관련 변수
	public enum eEnemyState { Idle, Patroll, Trace, Attack, Die } // 접근 제한자 설정
	protected StateMachine<EnemyBase> sm;
	public WayPoints wayPoints;
	protected float prevTime = 0f;			public float PrevTime { get; set; }
	protected float idleTime = 0f;			public float IdleTime { get; set; }
	protected float lastAttackTime = 0f;	public int LastAttackTime { get; set; }
	protected float lastSearchTime = 0f;	public int LastSearchTime { get; set; }
	protected GameObject target;        
	protected GameObject curTarget;			public GameObject CurTarget { get; set; }
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
		wayPoints = GetComponent<WayPoints>();
		target = GameObject.Find("Player");
	}
	public abstract void Init();	
	public abstract void GetHit();	// 맞는거
	public void CheckIdleState()
	{
		if (target != null)
			sm.ChangeState(sm.GetState((int)eEnemyState.Trace));
		else if (idleTime >= Random.Range(3f, 5f))
			sm.ChangeState(sm.GetState((int)eEnemyState.Patroll));
	}
	public void CheckPatrolState()
	{
		Search();
		if (target != null)
			sm.ChangeState(sm.GetState((int)eEnemyState.Trace));
		else if (wayPoints.CheckDestination(stopDistance))
			sm.ChangeState(sm.GetState((int)eEnemyState.Idle));
	}
	public void CheckTraceState()
	{
		if (CheckRadius(target.transform.position, attackDistance))
			sm.ChangeState(sm.GetState((int)eEnemyState.Attack));
		else if (CheckRadius(target.transform.position, traceDistance))
		{ 
			sm.ChangeState(sm.GetState((int)eEnemyState.Attack));
			MoveAndRotate(target.transform.position);
		}
		else if(!CheckRadius(target.transform.position, traceDistance))
			sm.ChangeState(sm.GetState((int)eEnemyState.Idle));
	}
	public void CheckAttackState()
	{
		if (CheckRadius(target.transform.position, attackDistance))	// 공격 거리 체크
		{
			if (Time.time > lastAttackTime)	// 공격 주기 체크
			{
				//ToDo 주은. Player가 Damage받는거 호출해오기
				Vector3 dir = target.transform.position - transform.position;
				transform.rotation = Quaternion.LookRotation(dir);
				lastAttackTime = Time.time + attackSpeed;
			}
		}
		else 
			sm.ChangeState(sm.GetState((int)eEnemyState.Trace));
	}
	public void MoveAndRotate(Vector3 _targetPos)
	{
		Vector3 dir = _targetPos - transform.position;
		transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime);
		Quaternion rot = Quaternion.LookRotation(dir);
		Quaternion.Lerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
	}
	bool CheckRadius(Vector3 _point, float _radius)  //타겟 위치, 비교 거리
	{
		Vector3 dir = _point - transform.position;
		float radiusSqr = _radius * _radius;		// r^2 : 반경 넓이 비교
		if (dir.sqrMagnitude < radiusSqr)
			return true;
		return false;
	}
	public void Search()	//목표물 탐색 및 설정
	{
		if (Time.time < lastSearchTime) return;
		lastSearchTime = Time.time + searchSpeed;

		// 거리 범위 내의 플레이어 검색
		if (CheckRadius(target.transform.position, traceDistance + traceDisOffset))
		{
			curTarget = target;
		}
		else curTarget = null;
	}
}

