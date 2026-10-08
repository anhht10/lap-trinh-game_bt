using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Shield : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private float _minIntensity = -10f, _maxIntensity = 0f;
    private ShieldSkillData _shieldSkillData;


    // [Header("Shield")]

    // [SerializeField] private float _maxShield = 100f;
    // [SerializeField] private float _currentShield;

    [Header("Owner")]
    [SerializeField] private CombatTeam _team;

    private Renderer _renderer;
    private Color _baseColor;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public CombatTeam Team => _team;
    public Ship Owner { get; private set; }
    public event System.Action OnShieldDestroyed;

    private float _currentDurability;

    private Coroutine _destroyCoroutine;

    private void Awake()
    {

        if (Owner == null)
            Owner = GetComponentInParent<Ship>();

        _renderer = GetComponent<Renderer>();
        _baseColor = _renderer.material.color;
        enabled = false;
    }

    public void Activate(ShieldSkillData shieldSkillData, Ship owner)
    {
        Owner = owner;
        if (Owner == null)
            Owner = GetComponentInParent<Ship>();

        _shieldSkillData = shieldSkillData;

        enabled = true;
        // Debug.Log($"Shield activated with durability: {_currentDurability}");
        RetsetDurability(1);
    }

    public void RetsetDurability(int i)
    {
        _currentDurability = _shieldSkillData.MaxDurability;

        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = null;
        }

        // Tạo timer mới
        if (_shieldSkillData.Duration > 0f)
        {
            _destroyCoroutine = StartCoroutine(
                DestroyAfterDuration(_shieldSkillData.Duration)
            );
        }
    }

    private IEnumerator DestroyAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _destroyCoroutine = null;
        DestroyShield();
    }

    public float AbsorbDamage(float damage)
    {
        return AbsorbDamage(damage, out _);
    }

    // public float AbsorbDamage(float damage, out float reflectDamage)
    // {
    //     StartCoroutine(FlashShield());
    //     reflectDamage = 0f;

    //     if (damage <= 0f)
    //         return 0f;

    //     if (_currentDurability <= 0f)
    //         return damage;

    //     // Block hoàn toàn hit
    //     if (Random.value < _shieldSkillData.BlockRate)
    //     {
    //         return 0f;
    //     }

    //     float reducedDamage = damage * (1f - _shieldSkillData.DamageReduction);

    //     float absorbableDamage = Mathf.Min(reducedDamage, _shieldSkillData.MaxAbsorption);

    //     float absorbed = Mathf.Min(absorbableDamage, _currentDurability);
    //     _currentDurability -= (absorbed  + reducedDamage * _shieldSkillData.DamageReduction);

    //     Debug.Log($"Shield absorbed {absorbed} damage. Remaining durability: {_currentDurability}");

    //     reflectDamage = absorbed * _shieldSkillData.ReflectPercent;

    //     if (_currentDurability <= 0f)
    //     {
    //         _currentDurability = 0f;
    //         DestroyShield();
    //     }
    //     return reducedDamage - absorbed;
    // }

    public float AbsorbDamage(float damage, out float reflectDamage)
    {
        StartCoroutine(FlashShield());
        reflectDamage = 0f;

        if (damage <= 0f)
            return 0f;

        if (_currentDurability <= 0f)
            return damage;

        // Block hoàn toàn hit
        if (Random.value < _shieldSkillData.BlockRate)
        {
            return 0f;
        }

        // Damage được Shield giảm theo %
        float reductionDamage = damage * _shieldSkillData.DamageReduction;

        // Giới hạn lượng damage được giảm trong một hit
        float absorbed = reductionDamage;

        if (_shieldSkillData.MaxAbsorption > 0f)
        {
            absorbed = Mathf.Min(absorbed, _shieldSkillData.MaxAbsorption);
        }

        // Không được hấp thụ quá Durability
        absorbed = Mathf.Min(absorbed, _currentDurability);

        // Durability giảm đúng bằng damage Shield thực sự hấp thụ
        _currentDurability -= absorbed;

        // Damage còn lại đi vào nhân vật
        float remainingDamage = damage - absorbed;

        reflectDamage = absorbed * _shieldSkillData.ReflectPercent;

        Debug.Log(
            $"Damage: {damage} | " +
            $"Absorbed: {absorbed} | " +
            $"Remaining: {remainingDamage} | " +
            $"Durability: {_currentDurability}"
        );

        if (_currentDurability <= 0f)
        {
            _currentDurability = 0f;
            DestroyShield();
        }

        return remainingDamage;
    }

    // public void RestoreShield(float amount)
    // {
    //     if (amount <= 0f)
    //         return;

    //     _currentShield =
    //         Mathf.Min(_currentShield + amount, _maxShield);
    // }

    private IEnumerator FlashShield()
    {
        Color shieldColor;
        Color emissionColor = _renderer.material.GetColor(EmissionColor);
        float elapsedTime = 0f;
        while (elapsedTime < _flashDuration)
        {
            shieldColor = Color.Lerp(_flashColor, _baseColor, elapsedTime / _flashDuration);
            float intensity = Mathf.Lerp(_maxIntensity, _minIntensity, elapsedTime / _flashDuration);
            _renderer.material.color = shieldColor;
            _renderer.material.SetColor(EmissionColor, emissionColor * Mathf.Pow(2, intensity));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // float elapsedTime = 0f;

        // while (elapsedTime < _flashDuration)
        // {
        //     float t = elapsedTime / _flashDuration;
        //     float intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        //     Color flashColor = _flashColor * intensity;

        //     _renderer.material.SetColor(EmissionColor, flashColor);

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        // _renderer.material.SetColor(EmissionColor, _baseColor);
    }

    private void DestroyShield()
    {
        StopAllCoroutines();
        OnShieldDestroyed?.Invoke();
        Destroy(gameObject);
    }

    // public bool IsBroken()
    // {
    //     return _currentShield <= 0f;
    // }
}

// Ví dụ:
// Shield = 70
// Incoming Damage = 100
// Kết quả:
// 100
//  ↓
// Shield
//  ↓
// 70 absorbed
//  ↓
// 30 remaining
// 30 mới được đưa vào bước tiếp theo.