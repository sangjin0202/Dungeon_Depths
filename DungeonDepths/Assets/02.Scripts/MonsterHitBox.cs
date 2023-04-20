using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.SendMessage("SetTakedDamage", this.gameObject.transform.parent.GetComponent<MonsterBase>().Damage);
            //Debug.Log(this.gameObject.transform.parent.name + " Damage " + this.gameObject.transform.parent.GetComponent<MonsterBase>().Damage);
        }
    }
}
