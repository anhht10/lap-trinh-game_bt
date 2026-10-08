// using UnityEngine;

// public class ShieldSkill : ShipSkill
// {
// 	// [SerializeField] private ShieldSkillData _data;

// 	// private Shield _shield;
// 	// private float _cooldownRemaining;

// 	// public bool IsReady => _cooldownRemaining <= 0f;

// 	// private void Awake()
// 	// {
// 	// 	_shield = GetComponentInChildren<Shield>();
// 	// }

// 	// private void Update()
// 	// {
// 	// 	_cooldownRemaining = Mathf.Max(0f, _cooldownRemaining - Time.deltaTime);

// 	// }

// 	// public override bool TryActivate()
// 	// {
// 	// 	if (!IsReady || _data == null || _shield == null)
// 	// 		return false;

// 	// 	_shield.Activate();
// 	// 	_cooldownRemaining = _data.Cooldown;
// 	// 	return true;
// 	// }
// }


using UnityEngine;

public class ShieldSkill : ShipSkill
{
	private readonly ShieldSkillData _data;

	private bool _isUsing;

	public override bool CanUse => !_isUsing;

	private Shield _shield;

	public ShieldSkill(Transform owner, ShieldSkillData data) : base(owner)
	{
		_data = data;
	}

	public override void Use(ShipAttack attack)
	{
		if (_isUsing && _shield != null)
		{
			_shield.RetsetDurability(2);

			return;
		}
		if (_data.ShieldPrefab == null)
			return;

		_isUsing = true;

		_shield = Object.Instantiate(_data.ShieldPrefab, Owner.position, Owner.rotation, Owner);
		_shield.Activate(_data, Owner.GetComponent<Ship>());
		_shield.OnShieldDestroyed += OnShieldDestroyed;

	}

	private void OnShieldDestroyed()
	{
		if (_shield != null)
		{
			_shield.OnShieldDestroyed -= OnShieldDestroyed;
			_shield = null;
		}

		_isUsing = false;
	}
}

