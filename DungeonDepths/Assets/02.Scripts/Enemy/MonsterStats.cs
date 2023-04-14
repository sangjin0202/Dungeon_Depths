using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats
{
    protected int maxHP;
    protected int curHP;
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
    public MonsterStats(int _maxHp)
    {

    }

    public void InitStats(int _difficulty)
    {
        maxHP = (int)(maxHP * _difficulty);
        curHP = maxHP;
    }
    public void InitStatsAsZero()
    {
        curHP = 0;
    }
}
