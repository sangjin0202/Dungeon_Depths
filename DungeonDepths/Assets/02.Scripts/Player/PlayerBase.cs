using UnityEngine;
using EnumTypes;
using System.Collections;

public class PlayerBase : MonoBehaviour
{
    //구본혁
    // 플레이어의 상위 클래스
    // 세가지 종류의 플레이어가 공통적으로 가지는 기능들과 데이터들을 가져야한다. 

    public Animator animator;
    #region 상태관련

    public bool IsAttack { get; set; }
    public bool IsMove { get; set; }
    public bool IsDodge { get; set; }
    public bool IsDamaged { get; set; }
    public bool IsCast { get; set; } // 스킬 사용 중
    public bool IsInteract { get; set; } // 특성 카드 선택 중
    public bool IsJump { get; set; }
    // 특성카드
    public bool IsRebirth { get; set; }

    private float hasBarrier = 0f;
    public float HasBarrier { get { return hasBarrier; } set { hasBarrier = value; } }
    public bool IsLifeSteal { get; set; }

    public float lastHitTimer;
    private float lastHealTimer;
    public bool IsRegen { get; set; }

    public bool Amplify { get; set; }
    public bool IsBerserk { get; set; } // 특성카드 습득여부
    private bool onBerserk;             // 버서커 모드 활성화 여부

    public bool BossBonus { get; set; }
    public bool HasPoison { get; set; }
    public bool CanCounter { get; set; }
    public bool EarthQuake { get; set; }
    // 특성카드
    public bool somethingInFront;
    public bool IsDead { get; set; }

    public bool moveKeyDown;

    #endregion

    #region 플레이어 스테이터스
    public float HpMax { get; set; }
    public float HpCur { get; set; }
    public float AttackPower { get; set; }
    public float Defense { get; set; }

    private float moveSpeed;
    public float MoveSpeed { get; set; }
    public float AttackDelay { get; set; }
    public float AttackRange { get; set; }
    #endregion
    public float takedDamage;
    #region 점프 관련
    protected int jumpedCnt;
    protected float jumpPower { get; set; }
    public int possibleJumpNum { get; set; }
    protected Rigidbody rbody { get; set; }

    #endregion

    public float stateDuration { get; set; } // 소드맨 콤보 공격 상태 지속 시간
    protected Vector3 moveDir;

    //Q스킬 전체 쿨타임
    public float firstSkillCoolDown { get; set; }
    public float afterFirstSkill { get; set; }

    //E스킬 전체 쿨타임
    public float secondSkillCoolDown { get; set; }
    public float afterSecondSkill { get; set; }

    public float dodgeSkillCoolDown { get; set; }
    public float afterDodgeSkill { get; set; }
    protected PlayerUI playerUI;
    protected virtual void Awake()
    {
        playerUI = UIManager.Instance.WindowList[(int)Window.PLAYERSTATE].GetComponent<PlayerUI>();
    }
    protected virtual void Update()
    {
        

        playerUI.UpdatePlayerUI(this);
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            moveKeyDown = true;

        Move();
        Jump();
        GetDamage();

        if(HpCur < 100f && IsRegen && Time.time - lastHitTimer >= 5f)
        {
            if(Time.time - lastHealTimer >= 1f)
            {
                HpCur += 10f;
                lastHealTimer = Time.time;
                Mathf.Clamp(HpCur, 0f, HpMax);
            }
        }
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
        if(IsDodge || IsCast || !moveKeyDown) return;

        // x축과 z축의 입력값을 방향으로 설정한다.
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(moveDir != Vector3.zero)
            CharacterAxisRotate();

        // 플레이어 캐릭터는 키보드로 입력 받은 방향을 바라본다.
        transform.LookAt(transform.position + moveDir);
        moveSpeed = Input.GetButton("Run") ? MoveSpeed * 2 : MoveSpeed;

        if(moveDir == Vector3.zero) // 정지상태라면
        {
            IsMove = false;
            moveKeyDown = false;
            animator.SetBool("Move", IsMove);
        }
        else// 정지상태가 아니라면
        {
            //if(!IsMove) return;
            IsMove = true;
            if(IsJump)
                animator.SetBool("Move", !IsMove);
            else
                animator.SetBool("Move", IsMove);

            MoveSpeed = Mathf.Clamp(MoveSpeed, 0f, 5.5f);
            animator.SetFloat("MoveSpeed", moveSpeed, 0.0f, Time.deltaTime);
            if(!somethingInFront)
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            //rbody.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
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
                IsJump = false;
                jumpedCnt = 0;
            }
        }
        if(IsAttack || IsDodge || IsCast) return;

        // 점프 버튼이 눌렸고, 점프 가능한 횟수보다 점프한 횟수가 적다면
        if(Input.GetButtonDown("Jump") && jumpedCnt < possibleJumpNum)
        {
            IsJump = true;
            rbody.velocity = Vector3.up * jumpPower;
            animator.SetTrigger("JumpTrigger");
            ++jumpedCnt;
        }
    }
    #endregion
    public void SetTakedDamage(float _damage)
    {
        if(_damage - Defense <= 0)
            takedDamage = 0f;
        else
            takedDamage += _damage - Defense;
    }
    void GetDamage()
    {
        float _takedDamage = takedDamage;
        //_takedDamage -= Defense;
        takedDamage = 0;
        if(_takedDamage > 0)
        {
            if(hasBarrier > _takedDamage)
            {
                hasBarrier -= _takedDamage;
            }
            else
            {
                _takedDamage -= hasBarrier;
                HpCur -= hasBarrier;
                hasBarrier = 0;
                HpCur -= _takedDamage;
                lastHitTimer = Time.time;
            }
            CheckBerserkMode();
            Debug.Log("플레이어 체력 " + HpCur);
            if(HpCur <= 0)
            {
                HpCur = 0f;
                Die();
            }
        }
    }

    private void CheckBerserkMode()
    {
        if(IsBerserk && HpCur <= HpMax * 0.25f)
        {
            if(!onBerserk)
            {
                AttackPower += 10f;
                onBerserk = true;
            }
            else
            {
                AttackPower -= 10f;
                onBerserk = false;
            }
        }
    }
    protected void Die()
    {
        animator.SetTrigger("Die");
        if(CanRebirth())
            return;
        else
            GameManager.Instance.IsGameOver = true;

    }
    protected bool CanRebirth()
    {
        if(IsRebirth)
        {
            HpCur = HpMax;
            IsRebirth = false;
            return true;
        }
        else return false;

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

