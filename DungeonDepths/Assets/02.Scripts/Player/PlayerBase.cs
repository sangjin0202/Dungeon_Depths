using UnityEngine;
using EnumTypes;
using System.Collections;

public class PlayerBase : MonoBehaviour
{
    // 구본혁
    // 플레이어의 상위 클래스
    // 세가지 종류의 플레이어가 공통적으로 가지는 기능들과 데이터들을 가져야한다. 

    #region 상태관련
    public bool IsAttack { get; set; }
    public bool IsMove { get; set; }
    public bool IsDodge { get; set; }
    public bool IsCast { get; set; } // 스킬 사용 중
    public bool IsJump { get; set; }
    public bool somethingInFront;
    public bool moveKeyDown;
    #endregion
    #region 플레이어 스테이터스
    public float HpMax { get; set; }
    public float HpCur { get; set; }
    public float AttackPower { get; set; }
    public float Defense { get; set; }

    private float moveSpeed;
    public float MoveSpeed { get; set; }
    #endregion
    #region 특성카드
    public bool HasSniper { get; set; }
    public bool HasRebirth { get; set; } // 부활
  
    private float hasBarrier = 0f; // 보호막 
    public float HasBarrier { get { return hasBarrier; } set { hasBarrier = value; } }
    
    public bool HasLifeSteal { get; set; } // 흡혈

    public float lastHitTimer; 
    private float lastHealTimer;
    public bool HasRegen { get; set; } // 체력재생

    public bool HasAmplify { get; set; } // 데미지 증폭

    public bool HasBerserk { get; set; } // 특성카드 습득여부
    private bool onBerserk;             // 버서커 모드 활성화 여부

    public bool HasBossBonus { get; set; } // 보스 추가피해
    public bool HasPoison { get; set; } // 독 인챈트
    public bool HasCounter { get; set; } // 
    public bool EarthQuake { get; set; }
    

    #endregion 특성카드
    #region 점프 관련
    protected int jumpedCnt;
    protected float jumpPower { get; set; }
    public int possibleJumpNum { get; set; }
    protected Rigidbody rbody { get; set; }

    #endregion
    #region 스킬 쿨타임
    public float firstSkillCoolDown { get; set; }
    public float afterFirstSkill { get; set; }

    public float secondSkillCoolDown { get; set; }
    public float afterSecondSkill { get; set; }

    public float dodgeSkillCoolDown { get; set; }
    public float afterDodgeSkill { get; set; }
    #endregion

    public float attackStateDuration { get; set; } // 소드맨 콤보 공격 상태 지속 시간
    protected Vector3 moveDir;
    public Animator animator;
    public float takedDamage;
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
            
        if(HpCur < 100f && HasRegen && Time.time - lastHitTimer >= 5f)
        {
            if(Time.time - lastHealTimer >= 1f)
            {
                HpCur += 10f;
                lastHealTimer = Time.time;
                Mathf.Clamp(HpCur, 0f, HpMax);
            }
        }
        if(onBerserk && HpCur > HpMax * 0.3f)
        {
            onBerserk = false;
            AttackPower -= 10f;
        }
    }

    #region 플레이어 회전
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

    #region 플레이어 이동
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
            IsMove = true;
            if(IsJump)
                animator.SetBool("Move", !IsMove);
            else
                animator.SetBool("Move", IsMove);

            MoveSpeed = Mathf.Clamp(MoveSpeed, 0f, 5.5f);
            animator.SetFloat("MoveSpeed", moveSpeed, 0.0f, Time.deltaTime);
            if(!somethingInFront)
                transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

    }
    #endregion

    #region 플레이어 점프
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

    #region 플레이어 피격
    public void SetTakedDamage(float _damage)
    {
        if(_damage - Defense <= 0)
            takedDamage = 1f;
        else
            takedDamage += _damage - Defense;
    }
    void GetDamage()
    {
        float _takedDamage = takedDamage;
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
                if(HasRegen)
                    lastHitTimer = Time.time;
            }
            // 버서커 카드를 획득했다면 버서커 모드 여부를 검사한다.
            if(HasBerserk) CheckBerserkMode();
            Debug.Log("플레이어 체력 " + HpCur);
            if(HpCur <= 0)
            {
                HpCur = 0f;
                Die();
            }
        }
    }
    #endregion

    private void CheckBerserkMode()
    {
        // 현재 체력이 최대체력의 30프로 이하고, 아직 버서커 모드가 발동중이 아니라면
        if(HpCur <= HpMax * 0.3f && !onBerserk)
        {
            AttackPower += 10f;
            onBerserk = true;
        }
    }

    #region 플레이어 사망
    protected void Die()
    {
        animator.SetTrigger("Die");
        if(CanRebirth())
            return;
        else
            GameManager.Instance.IsGameOver = true;
    }
    #endregion

    protected bool CanRebirth()
    {
        if(HasRebirth)
        {
            HpCur = HpMax;
            HasRebirth = false;
            return true;
        }
        else return false;

    }
}
interface IPlayerActions
{
    public void Attack();
    public void UseSkill();
    public void Dodge();
}

