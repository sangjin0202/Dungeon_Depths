using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;


//public abstract class AbilityEffect
//{
//    public abstract void ApplyEffect(PlayerBase _player, CardData _cardData);
//}
public class AbilityEffect// : AbilityEffect
{
    float moveSpeedCoefficient = 2f;
    // 공격속도 15프로 증가
    float attackSpeedCoefficient = 1.15f;
    float attackRangeCoefficient = 1f;
    float attackPowerCoefficient = 10f;
    int jumpNum = 1;
    float defenseCoefficient = 5f;
    float coolDownCoefficient = 0.3f;

    public bool Explode { get; set; }
    public bool IsExecute { get; set; }
    public bool Sting { get; set; }
    public bool Flash { get; set; }
    public bool Meteor { get; set; }
    public bool FrozenField { get; set; }
    public bool Roll { get; set; }
    public bool FlashBang { get; set; }
    public bool Genocide { get; set; }
    public bool HasPet { get; set; }
    public void StatBoostEffect(PlayerBase _player, CardData _cardData)
    {
        // StatType에 따라 이동속도, 공속 등의 스탯을 증가시킵니다.
        switch (_cardData.CardID)
        {
            case CardID.CARD_SPRINT:
                _player.MoveSpeed += moveSpeedCoefficient;
                break;
            case CardID.CARD_FRENZY:
                _player.animator.SetFloat("AttackSpeed", attackSpeedCoefficient);
                _player.attackStateDuration -= _player.attackStateDuration * 0.15f;
                break;
            case CardID.CARD_SNIPER:
                //_player.AttackRange += attackRangeCoefficient;
                _player.HasSniper = true;
                break;
            case CardID.CARD_POWER:
                _player.AttackPower += attackPowerCoefficient;
                break;
            case CardID.CARD_JUMP:
                _player.possibleJumpNum += jumpNum;
                break;
            case CardID.CARD_DEFENSE:
                _player.Defense += defenseCoefficient;
                break;
            case CardID.CARD_COOLDOWN:
                _player.firstSkillCoolDown -= 3f;
                _player.secondSkillCoolDown -= 3f;
                _player.dodgeSkillCoolDown -= 3f;
                Mathf.Clamp(_player.dodgeSkillCoolDown, 0f, 10f);
                break;
            case CardID.CARD_REBIRTH:
                _player.HasRebirth = true;
                break;
            case CardID.CARD_BARRIER:
                _player.HasBarrier = 100f;
                break;
            case CardID.CARD_LIFESTEAL:
                _player.HasLifeSteal = true;
                break;
            case CardID.CARD_REGEN:
                _player.HasRegen = true;
                _player.lastHitTimer = Time.time;
                break;
            case CardID.CARD_BERSERK:
                _player.HasBerserk = true;
                break;
            case CardID.CARD_EXPLODE:
                break;
            case CardID.CARD_EXECUTE:
                break;
            case CardID.CARD_AMPLIFY:
                _player.HasAmplify = true;
                break;
            case CardID.CARD_BOSS:
                _player.HasBossBonus = true;
                break;
            case CardID.CARD_POISON:
                _player.HasPoison = true;
                break;
            case CardID.CARD_SHIELD:
                _player.HasCounter = true;
                break;
            case CardID.CARD_EARTHQUAKE:
                _player.EarthQuake = true;
                break;
            case CardID.CARD_STING:
                break;
            case CardID.CARD_FLASH:
                break;
            case CardID.CARD_METEOR:
                break;
            case CardID.CARD_FROZENFIELD:
                break;
            case CardID.CARD_ROLL:
                break;
            case CardID.CARD_FLASHBANG:
                break;
            case CardID.CARD_GENOCIDE:
                break;
            case CardID.CARD_PET:
                break;
            default:
                break;
        }
    }

    
}

/*===============value 증감류===============
 * 이속 증가
 * 공속 증가
 * 공격 범위(사거리) 증가 
 * 공격력 증가
 * 점프 횟수 증가
 * 
 * 피격 데미지 감소
*/

/*============================== value 증감류==============================
 * 스킬 쿨타임 감소
 */

//==============================특수 능력류==============================

//===============활성화류===============
// 부활
// 보호막
// 기본 공격 피흡 
// 일정시간 노피격시 체력 재생
// 버서커
// 몹 처치시 폭발하며 광역 피해
// 일정량 이하의 hp 잡몹 처형
//크리티컬 시 데미지 증폭(확률 고정해놓고 데미지 증폭만 시키게)
// 기본 공격시 독, 얼음, 불 속성 추가
// 보스 추뎀
// TODO 펫소환 : 장식

/*===============활성화류(스킬강화)===============
 * 검사| 회피기(전방막기) 성공시, 스턴 효과
 * 검사| 땅찍기 맞으면 스턴
 * => 스턴 함수 하나만 있으면 됨
 * 검사| 찌르기 히트박스 증가
 */

//TODO 총잡이 마법사 RARE 능력 구현


