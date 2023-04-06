using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PlayerState;
public class PlayerBase : MonoBehaviour
{
    
    //구본혁
    /*TODO
     * 특성카드 적용 구현
     */
    public Animator animator;

    #region 상태관련
    public bool isAttack { get; set; }
    public bool isMove { get; set; }
    public bool isDodge { get; set; }
    public bool isDamaged { get; set; }
    public bool isCast { get; set; } // 스킬 사용 중
    public bool isInteract { get; set; } // 특성 카드 선택 중
    public bool isDie { get; set; }
    public bool isJump { get; set; }
    public bool isLand { get; set; } // 착지
    public bool isDead { get; set; }
    #endregion

    #region 플레이어 스테이터스
    public float HpMax { get; set; }
    public float HpCur { get; set; }
    public float AttackPower { get; set; }
    private float moveSpeed; public float MoveSpeed { get; set; }
    public float AttackDelay { get; set; }
    public float AttackRange { get; set; }
    #endregion

    #region 점프 관련
    protected int jumpedCnt;
    protected float jumpPower { get; set; }
    protected int possibleJumpNum { get; set; }
    protected bool enableMultipleJump { get; set; }
    protected Rigidbody rbody { get; set; }
    #endregion

    protected float hDir, vDir;
    protected Vector3 moveDir;
    float xRot, yRot;

    #region 조작 키
    public bool runKey { get; private set; }
    public bool dodgeKey { get; set; }
    public bool jumpKey { get; private set; }
    public bool attackKey { get; private set; }
    public bool skillOneKey { get; private set; }
    public bool skillTwoKey { get; private set; }
    #endregion

    string floorTag = "Floor";

    
    #region 입력 받기
    public virtual void GetInput()
    {
        hDir = Input.GetAxisRaw("Horizontal");
        vDir = Input.GetAxisRaw("Vertical");

        runKey = Input.GetButton("Run");
        jumpKey = Input.GetButtonDown("Jump");

        attackKey = Input.GetMouseButtonDown(0);

        skillOneKey = Input.GetButtonDown("Skill1");
        skillTwoKey = Input.GetButtonDown("Skill2");
    }
    #endregion
    #region 행동:회전
    protected void CharacterRotate()
    {
        if(isAttack || isDodge || isJump) return;
        transform.LookAt(transform.position + moveDir);
    }
    #endregion
    #region 행동:이동
    public void Move()
    {
        //moveDir = new Vector3(hDir, 0f, vDir).normalized; // 플레이어가 움직일 방향 설정
        //if(runKey) MoveSpeed *= 2f; //뛰기 키를 누르면 2배
        //transform.position += moveDir * MoveSpeed * (runKey ? 2f : 1f) * Time.deltaTime;
        //animator.SetBool("Move", true);

        if(isAttack || isDodge || isCast) return;
        moveDir = new Vector3(hDir, 0, vDir).normalized;
        moveSpeed = runKey ? MoveSpeed * 2 : MoveSpeed;
        if(moveDir == Vector3.zero)
        {
            animator.SetBool("Move", false);
            isMove = false;
        }
        else
        {
            isMove = true;
            if(!isJump)
                animator.SetBool("Move", true);
            MoveSpeed = Mathf.Clamp(MoveSpeed, 0f, 2f);
            animator.SetFloat("MoveSpeed", moveSpeed, 0.0f, Time.deltaTime);
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

    }
    #endregion
    #region 행동:점프
    protected virtual void Jump()
    {
        if(isAttack || isDodge || isCast) return;
        if(!isJump) animator.SetBool("Jump", false);
        if(!enableMultipleJump)
        {
            if(!isJump && jumpKey)
            {
                if(isMove)
                {
                    isMove = false;
                    animator.SetBool("Move", false);
                }
                animator.SetTrigger("Jump");
                rbody.velocity = Vector3.up * jumpPower;
                isJump = true;
            }
        }
        else
        {
            if(jumpedCnt < possibleJumpNum && jumpKey)
            {
                if(isMove)
                {
                    isMove = false;
                    animator.SetBool("Move", false);
                }
                isJump = true;
                animator.SetBool("Jump", true);
                rbody.velocity = Vector3.up * jumpPower;
                ++jumpedCnt;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == floorTag)
        {
            //Land();
            isJump = false;
            jumpedCnt = 0;
            MoveSpeed = MoveSpeed;
        }
    }
    #endregion
    
    #region 행동:피격
    //protected void GetHit(float damage) {
    //    animator.SetTrigger("Hit");
    //    HpCur -= damage;
    //    HpCur = Mathf.Clamp(HpCur, 0, HpMax);
    //    if(HpCur <= 0) Die();
    //}
    #endregion

    #region 행동:사망
    //protected void Die()
    //{
    //    animator.SetTrigger("Die");
    //    isDead = true;
    //}
    #endregion
    public void GetCard()
    { // 선택한 특성카드 적용

    }
}
interface IAttack
{
    public void Attack();
}

interface ISkill
{
    public void UseSkill();
}

interface IDodge
{
    public void Dodge();
}
