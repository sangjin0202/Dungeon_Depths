using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
public class PlayerUI : MonoBehaviour
{
    PlayerBase player;
    [SerializeField] Image playerHpBar;
    [SerializeField] Image firstSkillCool;
    [SerializeField] Image secondSkillCool;
    [SerializeField] Image dodgeSkillCool;
    void Awake()
    {
        playerHpBar = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        firstSkillCool = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>();
        secondSkillCool = transform.GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
        dodgeSkillCool = transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetComponent<Image>();
    }

    public void UpdatePlayerUI(PlayerBase _player)
    {
        playerHpBar.fillAmount = _player.HpCur / _player.HpMax;
        firstSkillCool.fillAmount = (Time.time - _player.afterFirstSkill) / _player.firstSkillCoolDown;
        secondSkillCool.fillAmount = (Time.time - _player.afterSecondSkill) / _player.secondSkillCoolDown;
    }
}
