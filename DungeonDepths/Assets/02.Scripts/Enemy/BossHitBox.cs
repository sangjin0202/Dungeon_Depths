using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBox : MonoBehaviour
{
    public BossGolem boss;
    public PlayerBase player;
    public TrailRenderer effectLeftHand, effectRightHand;
    private void OnEnable()
    {
        if(gameObject.name == "MeleeHitBox1") effectLeftHand.enabled = true;
        else if(gameObject.name == "MeleeHitBox2") effectRightHand.enabled = true;
        StartCoroutine(AutoDisable());
    }
    private void Awake()
    {
        effectLeftHand = transform.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TrailRenderer>();
        effectRightHand = transform.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(7).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TrailRenderer>();
    }
    private void Start()
    {
        Debug.Log("보스 팔 TrailRenderer");    
        boss = transform.GetComponentInParent<BossGolem>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBase>();
        this.gameObject.SetActive(false);
        effectLeftHand.enabled = false;
        effectRightHand.enabled = false;
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.2f);
        if(gameObject.name == "MeleeHitBox1") effectLeftHand.enabled = false;
        else if(gameObject.name == "MeleeHitBox2") effectRightHand.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("충돌체 검출 : " + other.name);
        if(other.CompareTag("Player"))
        {
            float damage = boss.AttackDamage;
            Debug.Log("보스 공격 성공 : " + other.name);
            player.SetTakedDamage(damage);
        }
    }
}
