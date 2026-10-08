using System;
using UnityEngine;

[Serializable]
public struct DamageData
{
    public float baseDamage;

    [Range(0f, 1f)]
    public float critChance;
    public float critMultiplier;

    public DamageType damageType;

    public CombatTeam sourceTeam;
}


public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Electric,
    Poison
}

// Ví dụ một thanh kiếm có:

// Base Damage     = 100
// Crit Chance     = 0.2
// Crit Multiplier = 2
// Damage Type     = Physical
// Source Team     = Player
