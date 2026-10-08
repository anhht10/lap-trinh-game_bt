using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(
        float remainingDamage,
        DamageData damageData,
        Hurtbox hurtbox,
        out bool isCritical)
    {
        isCritical = false;

        if (hurtbox == null)
            return 0f;

        // Body-part modifier
        float damage = remainingDamage * hurtbox.DamageMultiplier;

        // Critical
        if (!hurtbox.CanCritical)
            return damage;

        float critChance = damageData.critChance * hurtbox.CriticalChanceMultiplier;

        critChance = Mathf.Clamp01(critChance);

        if (Random.value < critChance)
        {
            damage *= damageData.critMultiplier;

            isCritical = true;
        }

        return damage;
    }
}

// Ví dụ:
// Base Damage = 100

// Head:
// Damage Multiplier = 1
// Crit Chance Multiplier = 2

// Nếu weapon có:
// Crit Chance = 20%
// Crit Multiplier = 2

// thì:
// Crit Chance:
// 20% × 2
// = 40%

// Nếu crit:
// 100 × 1 × 2
// = 200 damage