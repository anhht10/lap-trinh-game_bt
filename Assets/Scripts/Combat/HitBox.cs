// Đây là nơi bắt đầu toàn bộ flow.

// Nhưng có một điểm quan trọng:

// Nếu projectile/attack đập vào Shield trước thì nó phải gặp Shield Collider trước.

// Vì vậy mình sẽ tách:

// Hitbox
//  ↓
// Shield
//  ↓
// remainingDamage giảm
//  ↓
// Hitbox tiếp tục
//  ↓
// Hurtbox
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private DamageData damageData;

    [Header("Piercing")]
    [SerializeField] private bool canPierce = true;

    [SerializeField] private int maxHits = 1;

    private float remainingDamage;

    private int hitCount;

    private Collider hitboxCollider;

    private readonly HashSet<Collider> ignoredColliders = new();
    private Ship _owner;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        remainingDamage = damageData.baseDamage;
        hitCount = 0;
        ignoredColliders.Clear();
        enabled = true;
    }

    /// <summary>
    /// Gọi khi bắt đầu một attack.
    /// </summary>
    public void Activate(Ship owner, float attack)
    {
        _owner = owner;
        damageData.baseDamage = attack;
        remainingDamage = damageData.baseDamage;
        hitCount = 0;
        ignoredColliders.Clear();
        enabled = true;
    }

    /// <summary>
    /// Gọi khi attack kết thúc.
    /// </summary>
    public void Deactivate()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"1 Hitbox triggered by {other.name}");
        if (other == null)
            return;

        // Collider này đã được xử lý rồi.
        if (ignoredColliders.Contains(other))
            return;

        // Debug.Log($"2 Hitbox triggered by {other.name}");
        // =====================================
        // SHIELD
        // =====================================

        if (other.TryGetComponent<Shield>(out var shield))
        {
            // Không đánh đồng đội.
            // if (shield.Team == damageData.sourceTeam)
            // {
            //     IgnoreCollider(other);
            //     return;
            // }
            if (_owner != null && shield.Owner != null && _owner == shield.Owner)
            {
                IgnoreCollider(other);
                return;
            }

            remainingDamage = shield.AbsorbDamage(remainingDamage);

            Debug.Log(
                $"Hitbox hit Shield. " +
                $"Remaining Damage = {remainingDamage}"
                + $"baseDamage = {damageData.baseDamage}"
            );

            // Shield chặn hết.
            if (remainingDamage <= 0f)
            {
                StopAttack();
                return;
            }

            // Shield đã bị xuyên.
            //
            // Hitbox không destroy.
            // Nó tiếp tục đi tới collider tiếp theo.
            IgnoreCollider(other);
            return;
        }

        // =====================================
        // HURTBOX
        // =====================================
        if (other.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            // Không đánh đồng đội.
            if (hurtbox.Team == damageData.sourceTeam)
            {
                Debug.Log($"Hitbox hit friendly Hurtbox. Ignoring.");
                IgnoreCollider(other);
                return;
            }


            float finalDamage =
                DamageCalculator.CalculateDamage(
                    remainingDamage,
                    damageData,
                    hurtbox,
                    out bool isCritical
                );

            Health health = hurtbox.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(finalDamage);
            }

            // Debug.Log(
            //     $"Hitbox hit Hurtbox. " +
            //     $"Damage = {finalDamage}, " +
            //     $"Critical = {isCritical}"
            // );

            hitCount++;

            IgnoreCollider(other);

            // Không cho xuyên tiếp.
            if (!canPierce)
            {
                StopAttack();
                return;
            }

            if (hitCount >= maxHits)
            {
                StopAttack();
                return;
            }
        }
    }

    private void IgnoreCollider(Collider other)
    {
        if (other == null)
            return;

        ignoredColliders.Add(other);

        if (hitboxCollider != null)
        {
            Physics.IgnoreCollision(
                hitboxCollider,
                other,
                true
            );
        }
    }

    private void StopAttack()
    {
        Deactivate();
    }


}
