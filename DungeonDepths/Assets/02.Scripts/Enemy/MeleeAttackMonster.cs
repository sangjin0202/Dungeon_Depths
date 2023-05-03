using EnumTypes;
using UnityEngine;

public class MeleeAttackMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        TraceDistance = 10f;
        AttackDistance = 3f;
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Init(MapDifficulty _mapDifficulty)
    {
        base.Init(_mapDifficulty);
        Damage = stat.Damage * (float)_mapDifficulty * 0.5f;
        MaxHP = stat.MaxHP * (float)_mapDifficulty * 0.5f;
        CurHP = MaxHP;
    }
}