using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
  [SerializeField]
  private BarUI _hpBar;

  [SerializeField]
  private BarUI _mpBar;

  private Ship ship;

  private void Start()
  {
    ship = LocalPlayerManager.Instance.LocalPlayer;

    if (ship == null)
      return;

    Initialize();
  }

  private void Update()
  {
    if (ship != null)
      return;

    ship = LocalPlayerManager.Instance.LocalPlayer;

    if (ship == null)
      return;

    Initialize();
  }

  private void Initialize()
  {
    if (ship != null && ship.Health != null)
    {
      _hpBar.SetProgress(ship.Health.CurrentHealth, ship.Health.MaxHealth);
      ship.Health.OnHealthChanged += OnHealthChanged;
    }

    if (ship != null && ship.Energy != null)
    {
      _mpBar.SetProgress(ship.Energy.CurrentEnergy, ship.Energy.MaxEnergy);
      ship.Energy.OnEnergyChanged += OnEnergyChanged;
    }
  }

  private void OnHealthChanged(float currentHealth, float maxHealth)
  {
    _hpBar.SetProgress(currentHealth, maxHealth);
  }

  private void OnEnergyChanged(float currentEnergy, float maxEnergy)
  {
    _mpBar.SetProgress(currentEnergy, maxEnergy);
  }
}
