using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Init()
    {
        AttackDistance = 3f;
        TraceDistance = 10f;
        // damage 설정 함수 호출
        // moveSpeed 설정 함수 호출
        // attackSpeed 설정 함수 호출
        //monsterStats.CurHP = monsterStats.MaxHP;
    }
}