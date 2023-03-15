using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordMan : PlayerBase , IAttack, ISkill, IDodge{

    //구본혁
    //TODO: 공격, 스킬, 방어(회피) 애니메이션 적용, 히트박스 적용
    Animator animator;
    void Awake()
    {
        HpMax = 100f;
        HpCur = 100f;
        AttackPower = 5f;
        MoveSpeed = 5f;
        AttackDelay = 1f;
        AttackRange = 2f;
        AttackWidth = 1f;
        JumpPower = 10f;
        PossibleJumpNum = 2; //점프 최대 2번
        EnableMultipleJump = true; // 다중 점프 가능
        Rbody = GetComponent<Rigidbody>();

    }   

    void Update()
    {
        GetInput();
        CharacterRotate();
        Move();
        Jump();
        Attack();
        UseSkill();
        Dodge();
    }

    public void Attack() {
        if (AttackKey) {
            Debug.Log("공격하기"); 
        }
    }

    public void UseSkill() {
        if (SkillOneKey) {
            Debug.Log("스킬1 쓰기");
        }else if (SkillTwoKey) {
            Debug.Log("스킬2 쓰기");
        }
    }

    public void Dodge() {
        if (DodgeKey) {
            Debug.Log("방어(회피)하기");
        }
    }
}
