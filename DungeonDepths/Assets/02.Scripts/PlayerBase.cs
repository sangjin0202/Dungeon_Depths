using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

    //구본혁
    /*TODO
     * 특성카드 적용 구현, 모델, 애니메이터 적용
     * 방어, 공격, 회피를 나타낼 플래그
     * 해당 플래그가 true인 동안 필요할만한 상태 구현 (회피중 공격 불가 등)
     */
    float hpMax;
    public float HpMax {
        get { return hpMax; }
        set {
            hpMax = value;
        }
    }

    float hpCur;
    public float HpCur {
        get { return hpCur; }
        set {
            hpCur = value;
        }
    }

    float attackPower;
    public float AttackPower {
        get { return attackPower; }
        set {
            attackPower = value;
        }
    }

    float moveSpeed;
    public float MoveSpeed {
        get { return moveSpeed; }
        set {
            moveSpeed = value;
        }
    }

    float attackDelay;
    public float AttackDelay {
        get { return attackDelay; }
        set {
            attackDelay = value;
        }
    }

    float attackRange;
    public float AttackRange {
        get { return attackRange; }
        set {
            attackRange = value;
        }
    }

    float attackWidth;
    public float AttackWidth {
        get { return attackWidth; }
        set {
            attackWidth = value;
        }
    }

    float jumpPower;
    public float JumpPower {
        get { return jumpPower; }
        set {
            jumpPower = value;
        }
    }

    int possibleJumpNum;
    public int PossibleJumpNum {
        get { return possibleJumpNum; }
        set {
            possibleJumpNum = value;
        }
    }
    bool enableMultipleJump = false;
    public bool EnableMultipleJump {
        set { enableMultipleJump = value; }
    }
    int jumpedCnt;

    Rigidbody rbody;
    public Rigidbody Rbody {
        set {
            rbody = value;
        }
    }

    float hDir, vDir;
    Vector3 moveDir;
    float xRot, yRot;

    bool runKey, jumpKey, attackKey, dodgeKey, skillOneKey, skillTwoKey;
    public bool AttackKey{
        get{return attackKey ;}
    }
    public bool DodgeKey {
        get { return dodgeKey; }
    }
    public bool SkillOneKey {
        get { return skillOneKey; }
    }
    public bool SkillTwoKey {
        get { return skillTwoKey; }
    }

    bool isJump;
    string floorTag = "Floor";

    public void GetInput() {

        hDir = Input.GetAxisRaw("Horizontal");
        vDir = Input.GetAxisRaw("Vertical");

        runKey = Input.GetButton("Run");
        jumpKey = Input.GetButtonDown("Jump");

        attackKey = Input.GetMouseButtonDown(0);
        dodgeKey = Input.GetMouseButtonDown(1);

        skillOneKey = Input.GetButtonDown("Skill1");
        skillTwoKey = Input.GetButtonDown("Skill2");

        if(hDir != 0 || vDir != 0 || runKey && jumpKey) {
            
        }
    }

    public void CharacterRotate() {
        transform.LookAt(transform.position + moveDir);
    }

    public void Move() {
        moveDir = new Vector3(hDir, 0f, vDir).normalized;
        transform.position += moveDir * moveSpeed * ( runKey ? 2f : 1f) * Time.deltaTime;
    }

    public void Jump() {
        if (!enableMultipleJump) {
            if (!isJump && jumpKey) {
                rbody.velocity = Vector3.up * jumpPower;
                isJump = true;
            }
        } else {
            if (jumpedCnt < possibleJumpNum) {
                if (jumpKey) {
                    rbody.velocity = Vector3.up * jumpPower;
                    ++jumpedCnt;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == floorTag) {
            isJump = false;
            jumpedCnt = 0;
        }
    }

    public void GetCard() { // 선택한 특성카드 적용

    }
}
interface IAttack {
    public void Attack();
}

interface ISkill {
    public void UseSkill();

}

interface IDodge {
    public void Dodge();
}
