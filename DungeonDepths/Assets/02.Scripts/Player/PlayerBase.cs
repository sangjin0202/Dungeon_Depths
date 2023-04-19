using UnityEngine;
public class PlayerBase : MonoBehaviour
{
    //구본혁
    // 플레이어의 상위 클래스
    // 세가지 종류의 플레이어가 공통적으로 가지는 기능들과 데이터들을 가져야한다. 

    public Animator animator;
    #region 상태관련
    public bool isAttack { get; set; }
    public bool isMove { get; set; }
    public bool isDodge { get; set; }
    public bool isDamaged { get; set; }
    public bool isCast { get; set; } // 스킬 사용 중
    public bool isInteract { get; set; } // 특성 카드 선택 중
    public bool isJump { get; set; }
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
    public float takedDamage;
    #region 점프 관련
    protected int jumpedCnt;
    protected float jumpPower { get; set; }
    protected int possibleJumpNum { get; set; }
    protected Rigidbody rbody { get; set; }
    #endregion

    protected Vector3 moveDir;

    protected virtual void Update()
    {
        Move();
        Jump();
        GetDamage();
    }

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
    }
    #endregion

    #region 행동:이동
    public void Move()
    {
        if(isAttack || isDodge || isCast) return;

        // x축과 z축의 입력값을 방향으로 설정한다.
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(moveDir != Vector3.zero)
            CharacterAxisRotate();

        // 플레이어 캐릭터는 키보드로 받은 방향을 바라본다.
        transform.LookAt(transform.position + moveDir);
        moveSpeed = Input.GetButton("Run") ? MoveSpeed * 2 : MoveSpeed;

        if(moveDir == Vector3.zero) // 정지상태라면
        {
            isMove = false;
            animator.SetBool("Move", isMove);
        }
        else // 정지상태가 아니라면
        {
            if(!isJump)
            {
                isMove = true;
                animator.SetBool("Move", isMove);
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
        // 플레이어가 떨어지는 상태라면 플레이어와 바닥사이의 거리를 Raycast를 통해 측정한다.
        if(rbody.velocity.y < 0)
        {
            RaycastHit groundHit;
            if(Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.3f)) // 밑으로 Raycast를 쏴서 땅을 한번더 확인
            {
                isJump = false;
                jumpedCnt = 0;
            }
        }
        if(isAttack ||  isDodge || isCast) return;

        // 점프 버튼이 눌렸고, 점프 가능한 횟수보다 점프한 횟수가 적다면
        if(Input.GetButtonDown("Jump") && jumpedCnt < possibleJumpNum)
        {
            isJump = true;
            rbody.velocity = Vector3.up * jumpPower;
            animator.SetTrigger("JumpTrigger");
            ++jumpedCnt;
        }
    }
    #endregion
    public void SetTakedDamage(float _damage)
    {
        takedDamage += _damage;
    }
    void GetDamage()
    {
        float _takedDamage = takedDamage;
        takedDamage = 0;
        if (_takedDamage > 0)
        { 
            HpCur -= _takedDamage;
            Debug.Log(HpCur);
            if (HpCur <= 0)
            { 
                Die();
                //UI띄우고 게임 pause
            }
        }
    }
    protected void Die()
    {
        Debug.Log("플레이어 사망!");
        isDead = true;
    }
    public void GetCard()
    { // 선택한 특성카드 적용

    }
}
interface IPlayerActions
{
    public void Attack();
    public void UseSkill();
    public void Dodge();
}

