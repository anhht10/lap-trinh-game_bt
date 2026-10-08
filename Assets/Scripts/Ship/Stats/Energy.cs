using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
  [Header("Energy")]
  [SerializeField]
  [Min(0f)]
  private float _maxEnergy = 100f;

  private float _currentEnergy;

  [Header("Regeneration")]
  [SerializeField]
  [Min(0f)]
  private float _regenerationRate = 10f;

  [SerializeField]
  [Min(0f)]
  private float _regenerationDelay = 7f;

  private float _lastConsumeTime;

  public float MaxEnergy => _maxEnergy;
  public float CurrentEnergy => _currentEnergy;
  public float RegenerationRate => _regenerationRate;

  public bool IsFull => _currentEnergy >= _maxEnergy;
  public bool IsEmpty => _currentEnergy <= 0f;

  public float EnergyPercent => _maxEnergy > 0f ? _currentEnergy / _maxEnergy : 0f;

  public event Action<float, float> OnEnergyChanged;
  public event Action OnEnergyEmpty;
  public event Action OnEnergyFull;

  private void Awake()
  {
    _currentEnergy = _maxEnergy;
  }

  private void Update()
  {
    Regenerate();
  }

  private void Regenerate()
  {
    if (_regenerationRate <= 0f)
      return;

    if (IsFull)
      return;

    if (Time.time - _lastConsumeTime < _regenerationDelay)
      return;

    float oldEnergy = _currentEnergy;

    _currentEnergy += _regenerationRate * Time.deltaTime;
    _currentEnergy = Mathf.Min(_currentEnergy, _maxEnergy);

    if (!Mathf.Approximately(oldEnergy, _currentEnergy))
    {
      OnEnergyChanged?.Invoke(_currentEnergy, _maxEnergy);

      if (IsFull)
      {
        OnEnergyFull?.Invoke();
      }
    }
  }

  public bool TryConsume(float amount)
  {
    if (amount <= 0f)
      return true;

    Debug.Log($"Trying to consume {_currentEnergy}/{amount} energy. Current energy: {_currentEnergy}/{_maxEnergy}");

    if (_currentEnergy < amount)
      return false;

    SetEnergy(_currentEnergy - amount);

    _lastConsumeTime = Time.time;

    return true;
  }

  public void Restore(float amount)
  {
    if (amount <= 0f)
      return;

    SetEnergy(_currentEnergy + amount);
  }

  public void RestoreFull()
  {
    SetEnergy(_maxEnergy);
  }

  public void SetEnergy(float value)
  {
    float oldEnergy = _currentEnergy;

    _currentEnergy = Mathf.Clamp(value, 0f, _maxEnergy);

    if (Mathf.Approximately(oldEnergy, _currentEnergy))
      return;

    OnEnergyChanged?.Invoke(_currentEnergy, _maxEnergy);

    if (_currentEnergy <= 0f)
    {
      OnEnergyEmpty?.Invoke();
    }
    else if (_currentEnergy >= _maxEnergy)
    {
      OnEnergyFull?.Invoke();
    }
  }

  public bool CanConsume(float amount)
  {
    return amount <= _currentEnergy;
  }
}
