using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
public class FinalBossEntrance : MonoBehaviour
{
    FinalBoss finalBoss;
    void Awake()
    {
        finalBoss = transform.GetChild(0).GetComponent<FinalBoss>();
    }

	private void OnEnable()
	{
		finalBoss.gameObject.SetActive(false);
	}
	private void OnTriggerEnter(Collider _other)
	{
		if(_other.CompareTag("Player"))
		{
			finalBoss.gameObject.SetActive(true);
		}
	}
}
