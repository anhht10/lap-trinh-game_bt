using System.Collections;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    [SerializeField] private float _attack = 10f;
    [SerializeField][Min(0f)] private float _damageMultiplier = 1f;

    [SerializeField][Range(0f, 5f)] private float _cooldownTime = 0.25f;

    [SerializeField] private Projectile _baseProjectilePrefab;

    [SerializeField] private Transform _firePoint;

    private float _cooldown = 0f;

    private ShotSkillData _skillData;

    private float _countSkill = 0f;

    private Projectile _projectile;

    private Coroutine _skillTimerCoroutine;

    public bool CanFire
    {
        get { return _cooldown <= 0f; }
    }

    private void Awake()
    {
        _projectile = _baseProjectilePrefab;
    }

    private void Update()
    {
        _cooldown = Mathf.Max(0f, _cooldown - Time.deltaTime);
    }

    public bool TryFire(Ship owner, float baseAttack, float damageMultiplier)
    {
        if (!CanFire)
            return false;

        _cooldown = _cooldownTime;
        float attack = _attack + baseAttack;
        float dmgMultiplier = damageMultiplier * _damageMultiplier;
        if (_skillData != null)
        {
            dmgMultiplier *= _skillData.DamageMultiplier;

            attack *= dmgMultiplier;
            Projectile projectile = Instantiate(_projectile, _firePoint.position, transform.rotation);
            projectile.ApplyDamage(owner, attack);

            if (!_skillData.IsTimeSkill)
            {
                _countSkill++;
                if (_countSkill >= _skillData.Count)
                {
                    ResetSkillData();
                }
            }
        }
        else
        {

            attack *= dmgMultiplier;
            Projectile projectile = Instantiate(_projectile, _firePoint.position, transform.rotation);
            projectile.ApplyDamage(owner, attack);
        }

        return true;
    }

    public void SetSkillData(ShotSkillData skilldata)
    {
        _skillData = skilldata;
        if (skilldata.ProjectilePrefab != null)
        {
            _projectile = skilldata.ProjectilePrefab;
        }

        _countSkill = 0f;
        if (_skillTimerCoroutine != null)
        {
            StopCoroutine(_skillTimerCoroutine);
            _skillTimerCoroutine = null;
        }

        if (skilldata.IsTimeSkill)
        {
            _skillTimerCoroutine = StartCoroutine(SkillTimer());
        }
    }

    private IEnumerator SkillTimer()
    {
        yield return new WaitForSeconds(_skillData.Duration);
        ResetSkillData();
    }

    private void ResetSkillData()
    {
        _skillData = null;
        _countSkill = 0f;
        _projectile = _baseProjectilePrefab;
    }

}

