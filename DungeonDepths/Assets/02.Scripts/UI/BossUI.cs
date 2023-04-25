using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
public class BossUI : MonoBehaviour
{
    BossBaseFSM boss;
    FinalBoss finalBoss;
    Image bossHp;
    Text bossName;
    private void OnEnable()
    {
        bossHp = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        bossName = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        if(GameManager.Instance.IsPlaying)
        {
            if (StageManager.Instance.CurMap.mapData.Type == MapType.BOSS)
            {
                boss = GameObject.FindWithTag("Boss").GetComponent<BossBaseFSM>();
                bossName.text = boss.gameObject.name;
            }
            else if (StageManager.Instance.CurMap.mapData.Type == MapType.FINALBOSS)
            {
                finalBoss = StageManager.Instance.CurMap.transform.GetChild(3).GetChild(0).GetComponent<FinalBoss>();
                bossName.text = finalBoss.gameObject.name;
            }
        }
        //TODO 보스 이름 설정 추가
    }
    void Update()
	{
        if(StageManager.Instance.CurMap.mapData.Type == MapType.BOSS)
		    BossHPUpdate();
        else if(StageManager.Instance.CurMap.mapData.Type == MapType.FINALBOSS)
            FinalBossHPUpdate();
    }

	private void BossHPUpdate()
	{
		bossHp.fillAmount = boss.BossCurHp / boss.BossMaxHp;
		if (boss.isDead)
			Invoke("OffUI", 3f);
	}

    private void FinalBossHPUpdate()
    {
        bossHp.fillAmount = finalBoss.hpCur / finalBoss.hpMax;
        if (finalBoss.isDead)
            Invoke("OffUI", 3f);
    }

    void OffUI()
    {
        this.gameObject.SetActive(false);
    }
}
