using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monstser Data", menuName = "Scriptable Object/MonsterStat Data", order = int.MaxValue)]
public class MonsterStatData : ScriptableObject
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float attackSpeed;
    public float MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public float AttackSpeed
    {
        get => attackSpeed;
        set => moveSpeed = value;
    }
}
