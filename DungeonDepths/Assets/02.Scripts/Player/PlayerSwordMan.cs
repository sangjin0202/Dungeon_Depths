using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordMan : PlayerBase, IAttack, ISkill, IDodge
{

    // 구본혁
    // TODO: 히트박스 적용
    // 더블점프 모션, 피격 모션 해결해야함
    // 찌르기 모션 , 방어 모션, 점프모션 찾을수 있다면 찾기 
    //Animator animator;
    public float nextFireTime = 0f;
    static int numberOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1f;
    void Awake()
    {
        Debug.Log("시작");
        HpMax = 100f;
        HpCur = 100f;
        AttackPower = 5f;
        MoveSpeed = 1f;
        //moveSpeed = MoveSpeed;
        AttackDelay = 1f;
        AttackRange = 2f;
        jumpPower = 7f;
        possibleJumpNum = 2; //점프 최대 2번
        enableMultipleJump = true; // 다중 점프 가능
        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        CharacterRotate();
        Move();
        Jump();
        CheckTrigger();
        CheckAttack();
        UseSkill();
        Dodge();
    }
    public override void GetInput()
    {
        base.GetInput();
        dodgeKey = Input.GetMouseButton(1);
    }

    //protected override void Land()
    //{
    //    isLand = true;
    //    animator.SetTrigger("Land");
    //    StartCoroutine(FinishLand());
    //}

    //IEnumerator FinishLand()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    isLand = false;
    //}

    // 첫번째 공격 모션이 끝나기도 전에 클릭 카운트는 3이 될수도 있음
    // 공격키를 눌러도 아무런 반응이 없는 상황이 발생
    // 이런경우 대부분 Idle모션을 유지하므로 바로 공격 애니메이터 bool값과 클릭카운트를 초기화 시켜줌 
    private void CheckTrigger()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if(animator.GetBool("Hit1"))
            {
                numberOfClicks = 0;
                animator.SetBool("Hit1", false);
            }
            else if(animator.GetBool("Hit2"))
            {
                numberOfClicks = 0;
                animator.SetBool("Hit2", false);
            }
            else if(animator.GetBool("Hit3"))
            {
                numberOfClicks = 0;
                animator.SetBool("Hit3", false);
            }
        }
    }

    private void CheckAttack()
    {
        //현재 실행중인 애니메이션의 길이중 7할을 넘었고, 그 애니메이션의 이름이 hit1 이라면
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            isAttack = false;
            Debug.Log("공격1 끝");
            animator.SetBool("Hit1", false);

        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            isAttack = false;
            Debug.Log("공격2 끝");
            animator.SetBool("Hit2", false);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
        {
            isAttack = false;
            Debug.Log("공격3 끝");
            animator.SetBool("Hit3", false);
            numberOfClicks = 0; // 마지막 공격이 실행됐으므로 초기화
        }
        // 마지막 클릭시간으로부터 너무 오래 지났다면
        if(Time.time - lastClickedTime > maxComboDelay)
            numberOfClicks = 0;

        // 다음번 공격가능한 시간이 됐다면
        if(Time.time > nextFireTime)
        {
            if(attackKey)
            { //입력 확인
                Debug.Log("공격키 확인");
                Debug.Log(numberOfClicks);
                Attack(); // 공격 실행
            }
        }
    }

    // 공격 실행
    public void Attack()
    {
        if(isMove || isJump || isDodge || isCast) return;
        lastClickedTime = Time.time;
        numberOfClicks++;
        numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);

        if(numberOfClicks == 1)
        {
            Debug.Log("공격1 실행");
            isAttack = true;

            animator.SetBool("Hit1", true);
        }
        else if(numberOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            Debug.Log("공격2 실행");
            isAttack = true;
            animator.SetBool("Hit2", true);
        }
        else if(numberOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            Debug.Log("공격3 실행");
            isAttack = true;
            animator.SetBool("Hit3", true);
        }
    }

    public void UseSkill()
    {
        if(isMove || isJump || isDodge || isCast || isAttack) return;

        if(skillOneKey)
        {
            isCast = true;
            animator.SetTrigger("SkillOne");
            StartCoroutine(OffCast("SkillOne"));
        }
        else if(skillTwoKey)
        {
            isCast = true;
            animator.SetTrigger("SkillTwo");
            StartCoroutine(OffCast("SKillTwo"));
        }
    }

    IEnumerator OffCast(string skillTag)
    {
        if(skillTag == "SkillOne")
        {
            yield return new WaitForSeconds(3.0f);
            isCast = false;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            isCast = false;
        }
    }


    public void Dodge()
    {
        if(isAttack || isJump || isCast) return; // 공격중이거나 점프중이라면

        if(dodgeKey)
        {
            if(isMove) // 움직이는 중이였다면
            {
                animator.SetBool("Move", false);
                isMove = false;
            }
            isDodge = true;
            animator.SetBool("Block", true);
        }
        else
        {
            isDodge = false;
            animator.SetBool("Block", false);
        }
    }

}
