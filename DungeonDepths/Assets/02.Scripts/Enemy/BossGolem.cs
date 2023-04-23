using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossGolem : BossBaseFSM
{
    [HideInInspector] public GameObject MeleeHitBox1;
    [HideInInspector] public GameObject MeleeHitBox2;
    protected override void Awake()
    {
        base.Awake();
        rbody = GetComponent<Rigidbody>();
        MeleeHitBox1 = transform.GetChild(2).gameObject;
        MeleeHitBox2 = transform.GetChild(3).gameObject;
        MeleeHitBox1.SetActive(false);
        MeleeHitBox2.SetActive(false); 
        
        BossMaxHp = 500;
        BossCurHp = BossMaxHp;
        MoveSpeed = 3.5f;
        RotSpeed = 3f;
        AttackDamage = 30f;
        
        TargetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        BossTransform = GetComponent<Transform>();
        TraceRange = 50f; // 보스방 크기와 동일
        
        AttackDelay = 5f;
        MeleeRange = 3.5f;
        BeamRange = 20f;

    }
    private void OnEnable()
    {
    }

    public void OnMeleeAttackOneCollision()
    {
        MeleeHitBox1.SetActive(true);
    }

    public void OnMeleeAttackTwoCollision()
    {
        MeleeHitBox2.SetActive(true);
    }

    public override void GetHit(float _damage)
    {
        BossCurHp -= _damage;
        BossCurHp = Mathf.Clamp(BossCurHp, 0, BossMaxHp);
        Debug.Log("보스 현재 체력 : " + BossCurHp);
        CheckAlive();
    }
    void Update()
    {
        //Debug.Log(stateMachine.CurrentState);
        stateMachine.Execute();
        //rbody.velocity = Vector3.zero;
        //rbody.angularVelocity = Vector3.zero;
    }
}
