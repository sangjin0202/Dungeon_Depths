using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // Áïµô
    void TakeDamage(float _damage);
    // µµÆ®µô
    void TakeDamageOverTime(float _damagePerSecond, float _duration);
    // Á×À½
    void OnDeath();
}
