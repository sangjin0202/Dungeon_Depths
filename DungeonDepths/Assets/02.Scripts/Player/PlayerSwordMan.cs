using System.Collections;
using UnityEngine;

public class PlayerSwordMan : PlayerBase, IPlayerActions
{
    // 구본혁
    // TODO:
    // 히트박스 적용
    // 더블점프 모션, 피격 모션 해결해야함
    // 공격모션에서 Collider On/Off 구현
    // 찌르기 모션 , 방어 모션, 점프모션 찾을수 있다면 찾기 

    public enum SwordManStates { None, Start, Combo, Finish };
    public StateMachine<PlayerSwordMan> stateMachine;

    [Tooltip("기본 공격 히트 박스")] public GameObject hitBox;
    [Tooltip("스킬1 히트 박스")] public GameObject earthQuakeHitBox;
    [Tooltip("스킬1 히트 박스")] public GameObject stingHitBox;
    
    public bool attackClick;

    [HideInInspector] public float prevAtkTime;
    [HideInInspector] public int attackIndex;
    GameObject blockArea;

    protected override void Awake()
    {
        base.Awake();
        blockArea = transform.GetChild(6).gameObject;
        hitBox = transform.GetChild(7).gameObject;
        earthQuakeHitBox = transform.GetChild(8).gameObject;
        stingHitBox = transform.GetChild(9).gameObject;
        
        blockArea.SetActive(false);
        GameManager.Instance.CurPlayerClass = EnumTypes.Class.SWORD;
        stateMachine = new StateMachine<PlayerSwordMan>();
        stateMachine.AddState((int)SwordManStates.None, new SwordManState.None());
        stateMachine.AddState((int)SwordManStates.Start, new SwordManState.Start());
        stateMachine.AddState((int)SwordManStates.Combo, new SwordManState.Combo());
        stateMachine.AddState((int)SwordManStates.Finish, new SwordManState.Finish());

        stateMachine.InitState(this, stateMachine.GetState((int)SwordManStates.None));

        attackStateDuration = 1.5f;
        HpMax = 100f;
        HpCur = 100f;
        Defense = 3f;
        AttackPower = 10f;
        MoveSpeed += 3.5f;
        jumpPower = 8f;
        possibleJumpNum = 2;
        firstSkillCoolDown = 8f;
        secondSkillCoolDown = 5f;
        dodgeSkillCoolDown = 0f;
        afterFirstSkill = -firstSkillCoolDown;
        afterSecondSkill = -secondSkillCoolDown;
        afterDodgeSkill = -dodgeSkillCoolDown;

        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //Debug.Log("공격력 : "+ AttackPower);
    }
    protected void FixedUpdate()
    {
        LayerMask layer = 1 << 6;
        somethingInFront = Physics.Raycast(transform.position, moveDir, 3f, layer);
        if(somethingInFront) Debug.Log("바로 앞에 보스가 있음!");
    }
    protected override void Update()
    {
        if(GameManager.Instance.IsPause) return;
        base.Update();
        CheckAttackKey();
        stateMachine.Execute();
        UseSkill();
        Dodge();
    }

    void CheckAttackKey()
    {
        if(IsJump || IsDodge || IsCast) return;

        attackClick = Input.GetMouseButtonDown(0);

        if(attackClick)
        {
            if(IsMove)
            {
                moveKeyDown = false;
                IsMove = false;
                animator.SetBool("Move", IsMove);
            }
            Attack();
        }
    }
    public void Attack()
    {
        base.Awake();
        //공격을 하지 않는 상태라면 첫번째 공격을 실행한다.
        if(stateMachine.CurrentState == stateMachine.GetState((int)SwordManStates.None))
            stateMachine.ChangeState(stateMachine.GetState((int)SwordManStates.Start));
    }


    public void OnAttackCollision()
    {
        hitBox.SetActive(true);
    }
    public void OnEarthQuakeCollision()
    {
        earthQuakeHitBox.SetActive(true);
    }
    public void OnStingCollision()
    {
        stingHitBox.SetActive(true);
    }

    public void UseSkill()
    {
        if(IsMove || IsJump || IsDodge || IsCast || IsAttack) return;

        if(Input.GetButtonDown("Skill1") && Time.time - afterFirstSkill >= firstSkillCoolDown)
        {
            IsCast = true;
            animator.SetTrigger("SkillOne");
            StartCoroutine(OffCast("SkillOne"));
        }
        else if(Input.GetButtonDown("Skill2") && Time.time - afterSecondSkill >= secondSkillCoolDown)
        {
            IsCast = true;
            animator.SetTrigger("SkillTwo");
            StartCoroutine(OffCast("SKillTwo"));
        }
    }

    IEnumerator OffCast(string skillTag)
    {
        if(skillTag == "SkillOne")
        {
            yield return new WaitForSeconds(2.0f);
            afterFirstSkill = Time.time;
            IsCast = false;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            afterSecondSkill = Time.time;
            IsCast = false;
        }
    }

    public void Dodge()
    {

        if(IsAttack || IsJump || IsCast) return; // 공격중이거나 점프중이라면

        afterDodgeSkill = Time.time;

        if(Input.GetMouseButton(1))
        {
            if(IsMove) // 움직이는 중이였다면
            {
                animator.SetBool("Move", false);
                IsMove = false;
            }
            IsDodge = true;
            animator.SetBool("Block", true);
            blockArea.SetActive(true);
        }
        else
        {
            IsDodge = false;
            animator.SetBool("Block", false);
            blockArea.SetActive(false);
        }
    }

    public void GetHit(float _damage)
    {
        HpCur -= _damage;
        HpCur = Mathf.Clamp(HpCur, 0, HpMax);
        Debug.Log("현재 체력 : " + HpCur);
        if(HpCur <= 0) Die();
    }
}
