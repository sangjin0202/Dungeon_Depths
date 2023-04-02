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
    void Update()
    {
        sm.Execute();

    }
    public override void Init()
    {
        damage = 10;
        attackDistance = 10f;
        traceDistance = 15f;
        moveSpeed = 3f;
        rotSpeed = 90f;
        attackSpeed = 0.5f;
        curHp = maxHp;
    }
    public override void GetHit()
    {
        //ToDo ¡÷¿∫
    }
}