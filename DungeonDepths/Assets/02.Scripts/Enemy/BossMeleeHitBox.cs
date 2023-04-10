using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeHitBox : MonoBehaviour
{
    public BossGolem boss;
    public PlayerBase player;
    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }
    private void Awake()
    {
        boss = transform.GetComponentInParent<BossGolem>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBase>();
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        float damage = boss.AttackDamage;
        if(other.CompareTag("Player"))
        {
            Debug.Log("보스 공격 성공 : " + other.name);
            player.GetHit(damage);
        }
    }
}
