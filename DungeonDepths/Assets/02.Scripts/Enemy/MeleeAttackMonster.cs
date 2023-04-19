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
    private void OnTriggerEnter(Collider other)
    {
        
    }

    public override void Init(MapDifficulty _mapDifficulty)
    {
        Damage = stat.Damage * (float)_mapDifficulty * 0.5f;
        MaxHP = stat.MaxHP * (float)_mapDifficulty * 0.5f;
        CurHP = MaxHP;
    }
}