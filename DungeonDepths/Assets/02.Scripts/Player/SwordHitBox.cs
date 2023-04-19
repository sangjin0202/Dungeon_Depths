using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    LayerMask layer;
    [SerializeField] BossBaseFSM boss;
    [SerializeField] Collider[] colliders;
    [SerializeField] float swordDamage;

    //This function is called when the object becomes enabled and active.
    
    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }
    private void Start()
    {
        //boss = GameObject.FindWithTag("Boss").GetComponent<BossBaseFSM>();
        swordDamage = GameObject.FindWithTag("Player").GetComponent<PlayerSwordMan>().AttackPower;
        //Debug.Log("칼 공격력 : " + swordDamage);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        layer = 1 << 7;
        //Debug.Log("충돌체: " + other.gameObject.name);
        if(other.CompareTag("Enemy"))
        {
            //other.GetComponent<MonsterBase>().GetHit(5);
            colliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2, Quaternion.identity, layer);
                Debug.Log("검출하기" + swordDamage);
            foreach(Collider _collider in colliders)
            {
                Debug.Log("때리기" + swordDamage);
                _collider.SendMessage("GetDamage", swordDamage);
            }
        }
        else if(other.CompareTag("Boss"))
        {
            //boss.GetHit(swordDamage);
        }
        
    }
    //public void SendDamage()
    //{
    //    float swordDamage = transform.GetComponent<PlayerSwordMsan>().AttackPower;
    //    Debug.Log("데미지 전달!");
    //}
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
