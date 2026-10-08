using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField] private CombatTeam team;

    [Header("Damage")]
    [SerializeField] private float damageMultiplier = 1f;

    [Header("Critical")]
    [SerializeField] private float criticalChanceMultiplier = 1f;
    [SerializeField] private bool canCritical = true;

    public CombatTeam Team => team;

    public float DamageMultiplier => damageMultiplier;

    public float CriticalChanceMultiplier => criticalChanceMultiplier;

    public bool CanCritical => canCritical;
}


// Head
//     Damage Multiplier = 1
//     Crit Chance Multiplier = 3

// Chest
//     Damage Multiplier = 1
//     Crit Chance Multiplier = 1

// Arm
//     Damage Multiplier = 0.5
//     Crit Chance Multiplier = 0.5