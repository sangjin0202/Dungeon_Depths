using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PlayerState;
public abstract class PlayerBase : MonoBehaviour
{

    //구본혁
    /*TODO
     * 특성카드 적용 구현
     * 카메라가 바라보는 방향이 캐릭터의 z축이 되게끔
     * 카메라가 바라보는 방향이 플레이어의 forward 방향이 아니라면 플레이어의 축을 바꿔줌
     */

    public Animator animator;
    #region 상태관련
    public bool isAttack { get; set; }
    public bool isMove { get; set; }
    public bool isDodge { get; set; }
    public bool isDamaged { get; set; }
    public bool isCast { get; set; } // 스킬 사용 중
    public bool isInteract { get; set; } // 특성 카드 선택 중
    public bool isJump { get; set; }
    public bool isLand { get; set; } // 착지
    public bool isDead { get; set; }
    #endregion

    #region 플레이어 스테이터스
    public float HpMax { get; set; }
    public float HpCur { get; set; }
    public float AttackPower { get; set; }

    private float moveSpeed;
    public float MoveSpeed { get; set; }
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
    protected virtual void GetInput()
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
    /// <summary>
    /// 함수가 호출되면 카메라의 local좌표계와 플레이어의 local좌표계를 맞춰준다.
    /// </summary>
    protected void CharacterAxisRotate()
    {
        // 카메라의 회전값을 가져온다
        Quaternion cameraRot = Camera.main.transform.rotation;
        // y축 회전값만 필요하므로 x,z 회전량은 0으로 만든다.
        cameraRot.x = cameraRot.z = 0;

        //캐릭터의 회전값에 카메라의 회전값을 대입한다.
        transform.rotation = cameraRot;

        moveDir = transform.TransformDirection(moveDir);
        //transform.LookAt(transform.position + moveDir);
    }
    #endregion
    #region 행동:이동
    public void Move()
    {
        if(isAttack || isDodge || isCast) return;

        // x축과 z축의 입력값을 방향으로 설정한다.
        moveDir = new Vector3(hDir, 0, vDir).normalized;

        if(vDir != 0 || hDir != 0)
            CharacterAxisRotate();

        // 플레이어 캐릭터는 키보드로 받은 방향을 바라본다.
        transform.LookAt(transform.position + moveDir);
        moveSpeed = runKey ? MoveSpeed * 2 : MoveSpeed;

        if(moveDir == Vector3.zero) // 정지상태라면
        {
            isMove = false;
            animator.SetBool("Move", false);
        }
        else // 정지상태가 아니라면
        {
            if(!isJump)
            {
                isMove = true;
                animator.SetBool("Move", true);
            }
            MoveSpeed = Mathf.Clamp(MoveSpeed, 0f, 3.5f);
            animator.SetFloat("MoveSpeed", moveSpeed, 0.0f, Time.deltaTime);
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
    #endregion
    #region 행동:점프
    protected virtual void Jump()
    {
        if(isAttack || isDodge || isCast) return;

        if(!isJump) animator.SetBool("Jump", false); // 점프 상태가 아니라면 점프 애니메이션 끄기

        if(!enableMultipleJump) // 다중 점프가 불가능한 상태라면
        {
            if(!isJump && jumpKey) // 점프중이 아닌데 점프키가 눌렸다면
            {
                rbody.velocity = Vector3.up * jumpPower;
                isJump = true;
            }
        }
        else
        {
            if(jumpedCnt < possibleJumpNum && jumpKey)
            {
                animator.SetTrigger("JumpTrigger");
                rbody.velocity = Vector3.up * jumpPower;
                ++jumpedCnt;
            }
        }
    }

    protected void CheckPlayerLand()
    {
        RaycastHit groundHit;
        if(GroundCheck(out groundHit)) // 밑으로 Raycast를 쏘아서 땅을 한번더 확인
        {
            isJump = false;
            jumpedCnt = 0;
            MoveSpeed = MoveSpeed;
        }
        else
            isJump = true;
    }
    bool GroundCheck(out RaycastHit hit)
    {
        //CharacterController의 isGrounded는 이전 프레임 때 Move등의 함수로 땅 쪽으로 향하였을 때 접지가 되어야만 true를 리턴한다.
        //이는 경사로를 내려가거나 울퉁불퉁한 지면을 다닐때 수시로 false값을 반환하므로 Raycast로 땅을 한번 더 체크하여 안정성을 강화한다.
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f);
    }
    #endregion

    #region 행동:피격
    public abstract void GetHit(float _damage);
    #endregion

    #region 행동:사망
    protected abstract void Die();
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
