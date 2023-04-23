using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
public class BossUI : MonoBehaviour
{
    BossBaseFSM boss;
    Image bossHp;
    private void OnEnable()
    {
        if(GameManager.Instance.IsPlaying)
        {
            if(StageManager.Instance.CurMap.mapData.Type == MapType.BOSS)
                boss = GameObject.FindWithTag("Boss").GetComponent<BossBaseFSM>();
        }
        bossHp = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        bossHp.fillAmount = boss.BossCurHp / boss.BossMaxHp;
        if(boss.isDead)
            Invoke("OffUI", 3f);
    }

    void OffUI()
    {
        this.gameObject.SetActive(false);
    }
}
