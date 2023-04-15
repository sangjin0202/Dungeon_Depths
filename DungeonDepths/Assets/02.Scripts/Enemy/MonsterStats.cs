using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats
{ 
    protected int maxHP;
    protected int curHP;
    protected int damage;
    protected float moveSpeed;
    protected float attackSpeed; 

    //TODO 주은 | 스탯 별 증가 감소 함수 구현

    public int MaxHP 
    {
        get => maxHP;
    }
    public int CurHP 
    {
        get => curHP;
        set => curHP = value;
    }
    public bool IsDead 
    {
        get
        {
            if (curHP <= 0)
                return true;
            else 
                return false;
        }
    }
    public int Damage
    {
        get => damage;
    }
    public float MoveSpeed 
    { 
        get => moveSpeed; 
    }
    public float AttackSpeed
    {
        get => attackSpeed;
    }
}
