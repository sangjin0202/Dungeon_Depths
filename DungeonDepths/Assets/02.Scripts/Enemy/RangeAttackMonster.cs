using EnumTypes;
using UnityEngine;

public class RangeAttackMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        AttackDistance = 3f;
        TraceDistance = 10f;
    }
    public override void Init(MapDifficulty _mapDifficulty)
    {
        stat.Damage = stat.Damage * (float)_mapDifficulty * 0.5f;
        stat.MaxHP = stat.MaxHP * (float)_mapDifficulty * 0.5f;
    }
}
