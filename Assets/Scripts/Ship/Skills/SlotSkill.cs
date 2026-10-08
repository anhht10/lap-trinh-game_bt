using System;
using UnityEngine;

[Serializable]
public class SkillSlot
{
	[SerializeField]
	private SkillData _data;

	private ShipSkill _skill;
	private float _cooldownTimer;
	private Ship _attacker;

	public SkillData Data => _data;

	public event Action<float> OnCooldownChanged;

	public bool CanUse =>
		_data != null &&
		_skill != null &&
		_cooldownTimer <= 0f;
	//  && _skill.CanUse;

	public bool IsInitialized => _skill != null;

	public void Initialize(Transform owner, Ship attack)
	{
		if (_data == null)
		{
			Debug.LogError(
				"SkillData is missing."
			);

			return;
		}
		_attacker = attack;

		_skill = _data.CreateSkill(owner);
	}

	public void Update(float deltaTime)
	{
		if (_cooldownTimer <= 0f)
			return;

		_cooldownTimer -= deltaTime;

		OnCooldownChanged?.Invoke(_cooldownTimer);

		if (_cooldownTimer < 0f)
			_cooldownTimer = 0f;
	}

	public void Use()
	{
		if (!CanUse)
			return;
		if (_attacker.Energy == null || _attacker.Energy.IsEmpty)
			return;

		if (_data == null)
			return;

		bool isConsumed = _attacker.Energy.TryConsume(_data.EnergyCost);

		if (!isConsumed)
			return;

		_skill.Use(_attacker.Attack);

		_cooldownTimer = _data.Cooldown;
	}

}