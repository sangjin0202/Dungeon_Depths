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
    [HideInInspector] public int numOfClicks;
    [HideInInspector] public float stateDuration;
    [HideInInspector] public float prevAtkTime;
    [HideInInspector] public int attackIndex;
    GameObject blockArea;

    void Awake()
    {
        blockArea = transform.GetChild(6).gameObject;
        hitBox = transform.GetChild(7).gameObject;
        earthQuakeHitBox = transform.GetChild(8).gameObject;
        blockArea.SetActive(false);
        GameManager.Instance.CurPlayerClass = EnumTypes.Class.SWORD;
        stateMachine = new StateMachine<PlayerSwordMan>();
        stateMachine.AddState((int)SwordManStates.None, new SwordManState.None());
        stateMachine.AddState((int)SwordManStates.Start, new SwordManState.Start());
        stateMachine.AddState((int)SwordManStates.Combo, new SwordManState.Combo());
        stateMachine.AddState((int)SwordManStates.Finish, new SwordManState.Finish());

        stateMachine.InitState(this, stateMachine.GetState((int)SwordManStates.None));

        HpMax = 100f;
        HpCur = 100f;
        AttackPower = 10f;
        MoveSpeed = 3.5f;
        AttackDelay = 1f;
        AttackRange = 2f;
        jumpPower = 8f;
        possibleJumpNum = 2; //점프 최대 2번
        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //Debug.Log("공격력 : "+ AttackPower);
    }

    protected override void Update()
    {
        base.Update();

        CheckAttackKey();
        stateMachine.Execute();

        UseSkill();
        Dodge();
    }

    void CheckAttackKey()
    {
        if (IsJump || IsDodge || IsCast) return;

        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    public void Attack()
    {
        if (numOfClicks == 0)
        {
            stateMachine.ChangeState(stateMachine.GetState((int)SwordManStates.Start));
        }
        numOfClicks++;
        numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);
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
        if (IsMove || IsJump || IsDodge || IsCast || IsAttack) return;

        if (Input.GetButtonDown("Skill1"))
        {
            IsCast = true;
            animator.SetTrigger("SkillOne");
            StartCoroutine(OffCast("SkillOne"));
        }
        else if (Input.GetButtonDown("Skill2"))
        {
            IsCast = true;
            animator.SetTrigger("SkillTwo");
            StartCoroutine(OffCast("SKillTwo"));
        }
    }

    IEnumerator OffCast(string skillTag)
    {
        if (skillTag == "SkillOne")
        {
            yield return new WaitForSeconds(3.0f);
            IsCast = false;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            IsCast = false;
        }
    }

    public void Dodge()
    {

        if (IsAttack || IsJump || IsCast) return; // 공격중이거나 점프중이라면


        if (Input.GetMouseButton(1))
        {
            if (IsMove) // 움직이는 중이였다면
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
        if (HpCur <= 0) Die();
    }
}
