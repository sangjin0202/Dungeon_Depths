using UnityEngine;
public class PlayerBase : MonoBehaviour
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
    protected Rigidbody rbody { get; set; }
    #endregion

    protected float hDir, vDir;
    protected Vector3 moveDir;

    protected virtual void Update()
    {
        Move();
        Jump();
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

        hDir = Input.GetAxisRaw("Horizontal");
        vDir = Input.GetAxisRaw("Vertical");

        // x축과 z축의 입력값을 방향으로 설정한다.
        moveDir = new Vector3(hDir, 0, vDir).normalized;

        if(vDir != 0 || hDir != 0)
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
        if(rbody.velocity.y < 0)
            CheckPlayerLand(); 
        if(isAttack || isDodge || isCast) return;

        if(Input.GetButtonDown("Jump") && jumpedCnt < possibleJumpNum) // 점프중이 아닌데 점프키가 눌렸다면
        {
            isJump = true;
            rbody.velocity = Vector3.up * jumpPower;
            animator.SetTrigger("JumpTrigger");
            ++jumpedCnt;
        }
    }

    protected void CheckPlayerLand()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.3f)) // 밑으로 Raycast를 쏘아서 땅을 한번더 확인
        {
            isJump = false;
            jumpedCnt = 0;
        }

    }
    #endregion
    
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

