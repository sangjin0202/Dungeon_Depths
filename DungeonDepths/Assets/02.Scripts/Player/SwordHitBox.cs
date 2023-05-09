using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwordHitBox : MonoBehaviour
{
    LayerMask layer;
    [SerializeField] BossBaseFSM boss;
    [SerializeField] FinalBoss finalBoss;
    [SerializeField] Collider[] colliders;
    [SerializeField] float swordDamage;
    [SerializeField] PlayerBase player;
    [SerializeField] TrailRenderer slashEffect;
    [SerializeField] ParticleSystem quakeEffect;
    [SerializeField] ParticleSystem stingEffect;
    [SerializeField] bool isExpanded;
    BoxCollider hitBoxSize;
    private void OnEnable()
    {
        if(player.HasSniper && !isExpanded)
        {
            ExpandHitBox();
            isExpanded = true;
        }

        if(this.gameObject.name == "SwordHitBox") slashEffect.enabled = true;

        StartCoroutine(AutoDisable());
    }

    private void Awake()
    {
        slashEffect = transform.parent.GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<TrailRenderer>();
        hitBoxSize = this.GetComponent<BoxCollider>();

        player = GameObject.FindWithTag("Player").GetComponent<PlayerBase>();
        boss = GameObject.FindWithTag("Boss").GetComponent<BossBaseFSM>();
        //finalBoss = GameObject.FindWithTag("FinalBoss").GetComponent<FinalBoss>();
    }
    private void Start()
    {
        //boss = GameObject.FindWithTag("Boss").GetComponent<BossBaseFSM>();
        //Debug.Log("칼 공격력 : " + swordDamage);
        this.gameObject.SetActive(false);
        slashEffect.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        layer = 1 << 7;
        //Debug.Log("충돌체: " + other.gameObject.name);
        if(other.CompareTag("Enemy"))
        {
            colliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2, Quaternion.identity, layer);

            foreach(Collider _collider in colliders)
            {
                CheckCritical();
                Debug.Log("검출하기 : " + _collider.tag);
                Debug.Log("때리기" + swordDamage);
                _collider.SendMessage("GetDamage", swordDamage);
                if(player.HasPoison)
                    _collider.SendMessage("GetDotDamage");
                if(player.HasLifeSteal)
                {
                    player.HpCur += 5f;
                    if(player.HpCur > 100)
                        player.HpCur = 100f;
                }
            }
        }
        else if(other.CompareTag("Boss"))
        {
            if(player.HasBossBonus)
                swordDamage += 10;
            CheckCritical();
            boss.GetHit(swordDamage);
        }
        //else if(other.CompareTag("FinalBoss"))
        //{
        //    if(player.BossBonus)
        //        swordDamage += 10;
        //    CheckCritical();
        //    finalBoss.GetHit(swordDamage);
        //}

    }

    private void CheckCritical()
    {
        if(this.gameObject.name == "SwordHitBox") swordDamage = player.AttackPower;
        else if(this.gameObject.name == "Skill1HitBox") swordDamage = player.AttackPower * 5;
        else if(this.gameObject.name == "Skill2HitBox") swordDamage = player.AttackPower * 3;
        if(Random.Range(0, 10) < 3 && player.HasAmplify)
            swordDamage *= 2f;
        Debug.Log("칼 데미지" + swordDamage);
    }

    // 소드맨 히트박스 확장 함수
    // 특성카드 먹으면 호출 
    private void ExpandHitBox()
    {
        if(this.gameObject.name == "SwordHitBox")
            hitBoxSize.size *= 2f;
    }
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
        slashEffect.enabled = false;
    }
}
