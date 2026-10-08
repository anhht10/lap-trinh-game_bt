using System;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{


    [SerializeField]
    private ExplosionData explosionData;

    [Header("Lifetime")]
    [SerializeField] private float destroyDelay = 5f;

    [Header("Effects")]
    [SerializeField] private AudioSource audioSource;

    private ParticleSystem[] particleSystems;

    private readonly HashSet<Health> damagedTargets = new();

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    public void Initialize(ExplosionData explosion)
    {
        explosionData = explosion;
    }

    private void OnEnable()
    {
        PlayExplosion();
    }

    private void PlayExplosion()
    {
        PlayParticles();
        PlaySound();
        ApplyExplosionDamage();
        ApplyExplosionForce();

        Destroy(gameObject, destroyDelay);
    }

    // =========================================================
    // PARTICLES
    // =========================================================

    private void PlayParticles()
    {
        if (particleSystems == null)
            return;

        // foreach (ParticleSystem particle in particleSystems)
        // {
        //     if (particle == null)
        //         continue;

        //     particle.Clear(true);
        //     particle.Play(true);
        // }
    }

    // =========================================================
    // AUDIO
    // =========================================================

    private void PlaySound()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    // =========================================================
    // DAMAGE
    // =========================================================

    private void ApplyExplosionDamage()
    {
        damagedTargets.Clear();

        Collider[] colliders = Physics.OverlapSphere(
                transform.position,
                explosionData.radius
            );

        foreach (Collider collider in colliders)
        {
            if (collider == null)
                continue;

            ProcessCollider(collider);
        }
    }

    private void ProcessCollider(Collider collider)
    {
        // Tìm Health của object bị nổ
        Health health = collider.GetComponentInParent<Health>();

        if (health == null)
            return;

        // Một target chỉ nhận damage một lần.
        if (damagedTargets.Contains(health))
            return;

        // -----------------------------------------------------
        // TEAM
        // -----------------------------------------------------

        CombatTeam targetTeam = GetTargetTeam(collider);

        if (targetTeam == explosionData.damageData.sourceTeam)
            return;

        // -----------------------------------------------------
        // SHIELD
        // -----------------------------------------------------

        Shield shield = collider.GetComponentInParent<Shield>();

        float remainingDamage = explosionData.damageData.baseDamage;

        if (shield != null)
        {
            if (shield.Team == explosionData.damageData.sourceTeam)
                return;

            remainingDamage = shield.AbsorbDamage(remainingDamage);

            Debug.Log(
                $"Explosion hit Shield on {health.name}. " +
                $"Remaining Damage = {remainingDamage}"
            );

            // Shield hấp thụ toàn bộ.
            if (remainingDamage <= 0f)
                return;
        }

        // -----------------------------------------------------
        // HURTBOX
        // -----------------------------------------------------

        if (!collider.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            // Collider hiện tại có thể là collider của Rigidbody
            // hoặc collider khác của target.
            hurtbox = health.GetComponentInChildren<Hurtbox>();
        }

        if (hurtbox == null)
            return;

        if (hurtbox.Team == explosionData.damageData.sourceTeam)
            return;

        // -----------------------------------------------------
        // DAMAGE FALLOFF
        // -----------------------------------------------------

        float distance = Vector3.Distance(transform.position, hurtbox.transform.position);

        float distanceMultiplier = CalculateDistanceMultiplier(distance);

        remainingDamage *= distanceMultiplier;

        if (remainingDamage <= 0f)
            return;

        // -----------------------------------------------------
        // DAMAGE CALCULATOR
        // -----------------------------------------------------

        float finalDamage = DamageCalculator.CalculateDamage(
                remainingDamage,
                explosionData.damageData,
                hurtbox,
                out bool isCritical
            );

        // -----------------------------------------------------
        // APPLY
        // -----------------------------------------------------

        health.TakeDamage(
            finalDamage,
            hurtbox.transform.position
        );

        damagedTargets.Add(health);

        Debug.Log(
            $"Explosion hit {health.name}. " +
            $"Damage = {finalDamage}. " +
            $"Critical = {isCritical}. " +
            $"Distance = {distance:F2}"
        );
    }

    // =========================================================
    // DAMAGE FALLOFF
    // =========================================================

    private float CalculateDistanceMultiplier(float distance)
    {
        if (!explosionData.useDamageFalloff)
            return 1f;

        if (explosionData.radius <= 0f)
            return 1f;

        float normalizedDistance = Mathf.Clamp01(distance / explosionData.radius);

        return Mathf.Lerp(
            1f,
            explosionData.minDamageMultiplier,
            normalizedDistance
        );
    }

    // =========================================================
    // TEAM
    // =========================================================

    private CombatTeam GetTargetTeam(Collider collider)
    {

        if (collider.TryGetComponent<Hurtbox>(out var hurtbox))
            return hurtbox.Team;


        if (collider.TryGetComponent<Shield>(out var shield))
            return shield.Team;

        return CombatTeam.Neutral;
    }

    // =========================================================
    // PHYSICS
    // =========================================================

    private void ApplyExplosionForce()
    {
        if (!explosionData.applyExplosionForce)
            return;

        Collider[] colliders = Physics.OverlapSphere(
                transform.position,
                explosionData.radius
            );

        HashSet<Rigidbody> rigidbodies = new();

        foreach (Collider collider in colliders)
        {
            if (collider == null)
                continue;

            Rigidbody rb = collider.attachedRigidbody;

            if (rb == null)
                continue;

            if (rigidbodies.Contains(rb))
                continue;

            rigidbodies.Add(rb);

            rb.AddExplosionForce(
                explosionData.explosionForce,
                transform.position,
                explosionData.radius,
                explosionData.upwardsModifier,
                ForceMode.Impulse
            );
        }
    }

    // =========================================================
    // GIZMOS
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionData.radius
        );
    }
}

[Serializable]
public class ExplosionData
{
    [Header("Damage")]
    // Toàn bộ thông tin sát thương của vụ nổ:
    // - baseDamage: sát thương cơ bản
    // - critChance: tỉ lệ chí mạng
    // - critMultiplier: hệ số sát thương chí mạng
    // - damageType: loại sát thương (Physical, Fire, ...)
    // - sourceTeam: đội của nguồn gây sát thương
    public DamageData damageData;


    // Bán kính ảnh hưởng của vụ nổ.
    // Các target nằm ngoài bán kính này sẽ không bị explosion xử lý.
    // Đồng thời được dùng cho AddExplosionForce().
    public float radius = 5f;


    [Header("Damage Falloff")]
    // Bật/tắt việc giảm sát thương theo khoảng cách.
    //
    // true:
    //     Càng xa tâm vụ nổ → càng ít damage.
    //
    // false:
    //     Mọi target trong radius đều nhận damage như nhau.
    public bool useDamageFalloff = true;


    // Sát thương nhỏ nhất mà target ở rìa explosion có thể nhận,
    // tính theo phần trăm damage ban đầu.
    //
    // Ví dụ:
    //     baseDamage = 100
    //     minDamageMultiplier = 0.25
    //
    // Target ở gần tâm:
    //     ≈ 100 damage
    //
    // Target ở sát mép radius:
    //     ≈ 25 damage
    //
    // Giá trị nằm từ 0 → 1.
    [Range(0f, 1f)]
    public float minDamageMultiplier = 0.25f;


    [Header("Physics")]

    // Bật/tắt lực vật lý do vụ nổ tạo ra.
    //
    // true:
    //     Rigidbody trong bán kính explosion bị đẩy.
    //
    // false:
    //     Explosion chỉ gây damage, không đẩy vật thể.
    public bool applyExplosionForce = true;


    // Độ mạnh của lực đẩy từ vụ nổ.
    //
    // Giá trị càng lớn → Rigidbody bị đẩy càng mạnh.
    //
    // Nó KHÔNG phải damage.
    // Nó chỉ ảnh hưởng đến vật lý.
    public float explosionForce = 25f;


    // Điều chỉnh lực theo hướng lên trên.
    //
    // 0:
    //     Chỉ đẩy ra xa tâm vụ nổ.
    //
    // > 0:
    //     Có thêm lực hướng lên.
    //
    // Ví dụ:
    //     1 = có thêm một lượng lực hướng lên
    //     2 = hiệu ứng hất tung mạnh hơn
    public float upwardsModifier = 1f;
}
