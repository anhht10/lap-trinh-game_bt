using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Energy")]
    [SerializeField]
    [Min(0f)]
    private float _maxHealth = 100f;

    private float _currentHealth;

    public float CurrentHealth => _currentHealth;

    public float MaxHealth => _maxHealth;

    public bool IsDead => _currentHealth <= 0f;

    public event Action OnDied;

    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPosition = default)
    {
        if (damage <= 0f)
            return;

        if (IsDead)
            return;

        _currentHealth -= damage;

        _currentHealth = Mathf.Max(_currentHealth, 0f);

        // Debug.Log(
        //     $"{gameObject.name} took {damage} damage. " +
        //     $"HP: {_currentHealth}/{_maxHealth}"
        // );

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        OnDied?.Invoke();
    }
}

